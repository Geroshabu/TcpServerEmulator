using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TcpServerEmulator.Rules.StoreValue
{
    /// <summary>
    /// 得た値を保持し、後で返却可能なルール
    /// </summary>
    internal class Rule : IRule
    {
        /// <inheritdoc cref="IRule.Name"/>
        public string Name { get; }

        /// <summary>設定コマンドで受け取るデータとしてユーザが入力した文字列</summary>
        public string SetterReceiveDataText { get; }

        /// <summary>設定コマンドで返却するデータとしてユーザが入力した文字列</summary>
        public string SetterResponseDataText { get; }

        /// <summary>取得コマンドで受け取るデータとしてユーザが入力した文字列</summary>
        public string GetterReceiveDataText { get; }

        /// <summary>取得コマンドで返却するデータとしてユーザが入力した文字列</summary>
        public string GetterResponseDataText { get; }

        /// <summary>保持する値の初期値としてユーザが入力した文字列</summary>
        public string[] InitialValues { get; }

        /// <summary>設定コマンドで受け取るデータとしてユーザが入力した内容のバイト表現</summary>
        private byte[] setterReceiveByteData { get; }

        /// <summary>設定コマンドで返却するデータとしてユーザが入力した内容のバイト表現</summary>
        private byte[] setterResponseByteData { get; }

        /// <summary>取得コマンドで受け取るデータとしてユーザが入力した内容のバイト表現</summary>
        private byte[] getterReceiveByteData { get; }

        /// <summary>取得コマンドで返却するデータとしてユーザが入力した内容のバイト表現</summary>
        private byte[] getterResponseByteData { get; }

        private ushort[] setterReceiveMatchingData { get; }
        private ushort[] getterReceiveMatchingData { get; }

        private Range[] setterReceiveMatchRanges { get; }
        private Range[] setterReceivePlaceholderRanges { get; }

        private List<byte[]> storedData { get; set; }

        /// <inheritdoc cref="IRule.Description"/>
        public string Description => string.Empty;

        /// <summary>
        /// <see cref="Rule"/>インスタンスの生成と初期化
        /// </summary>
        /// <param name="name">ルールの名前</param>
        /// <param name="setterReceiveData">設定コマンドで受け取ったデータとしてユーザが入力した文字列</param>
        /// <param name="setterResponseData">設定コマンドで返却するデータとしてユーザが入力した文字列</param>
        /// <param name="getterReceiveData">取得コマンドで受け取ったデータとしてユーザが入力した文字列</param>
        /// <param name="getterResponseData">取得コマンドで返却するデータとしてユーザが入力した文字列</param>
        /// <param name="initialValuesData">保持する値の初期値としてユーザが入力した文字列</param>
        /// <exception cref="ArgumentException">引数の形式が正しくないとき</exception>
        public Rule(
            string name,
            string setterReceiveData,
            string setterResponseData,
            string getterReceiveData,
            string getterResponseData,
            string initialValuesData)
        {
            Name = name;
            SetterReceiveDataText = setterReceiveData;
            SetterResponseDataText = setterResponseData;
            GetterReceiveDataText = getterReceiveData;
            GetterResponseDataText = getterResponseData;

            (setterReceiveByteData, setterReceiveMatchRanges, setterReceivePlaceholderRanges) = parseReceiveMatchData(setterReceiveData);
            setterResponseByteData = parseByteArray(setterResponseData).ToArray();
            getterReceiveByteData = parseByteArray(getterReceiveData).ToArray();
            getterResponseByteData = parseResponseMatchData(getterResponseData, setterReceivePlaceholderRanges);
            storedData = parseStoredData(initialValuesData).ToList();
        }

        /// <inheritdoc cref="IRule.CanResponse(byte[])"/>
        public bool CanResponse(byte[] receivedData)
        {
            if (receivedData.SequenceEqual(getterReceiveByteData))
            {
                return true;
            }

            return receivedData.Length == setterReceiveByteData.Length
                && setterReceiveMatchRanges.All(range => sequenceEqualsRange(setterReceiveByteData, receivedData, range));
        }

        private bool sequenceEqualsRange(byte[] expected, byte[] actual, Range range)
        {
            return expected.AsSpan(range).SequenceEqual(actual.AsSpan(range));
        }

        /// <inheritdoc cref="IRule.GetResponse(byte[])"/>
        public byte[] GetResponse(byte[] receivedData)
        {
            if (receivedData.SequenceEqual(getterReceiveByteData))
            {
                for (int i = 0; i < setterReceivePlaceholderRanges.Length; i++)
                {
                    storedData[i].CopyTo(getterResponseByteData, setterReceivePlaceholderRanges[i].Start.Value);
                }
                return getterResponseByteData;
            }

            storedData = setterReceivePlaceholderRanges
                .Select(range => receivedData.AsSpan(range).ToArray())
                .ToList();

            return setterResponseByteData;
        }

        private (byte[] Data, Range[] MatchRanges, Range[] PlaceholderRanges) parseReceiveMatchData(string text)
        {
            var matchRanges = new List<Range>();
            var placeholderRanges = new List<Range>();
            var data = new List<byte>();
            var matchStartPosition = 0;
            var matchLength = 0;
            var index = 0;

            foreach (string substring in text.Split(','))
            {
                var regex = new Regex(@"^\*(\d+)$");
                var match = regex.Match(substring);
                if (match.Success)
                {
                    if (!int.TryParse(match.Groups[1].Value, out int count))
                    {
                        throw new ArgumentException($"{substring} is invalid");
                    }

                    if (matchLength > 0)
                    {
                        matchRanges.Add(new Range(matchStartPosition, index));
                        matchLength = 0;
                    }

                    data.AddRange(Enumerable.Repeat<byte>(0xFF, count));
                    placeholderRanges.Add(new Range(index, index + count));
                    index += count;
                }
                else
                {
                    if (!byte.TryParse(substring, out byte byteData))
                    {
                        throw new ArgumentException($"{substring} is invalid");
                    }

                    if (matchLength == 0)
                    {
                        matchStartPosition = index;
                    }
                    matchLength++;

                    data.Add(byteData);
                    index++;
                }
            }

            if (matchLength > 0)
            {
                matchRanges.Add(new Range(matchStartPosition, index));
            }

            return (data.ToArray(), matchRanges.ToArray(), placeholderRanges.ToArray());
        }

        private byte[] parseResponseMatchData(string text, Range[] placeHolderRanges)
        {
            var data = new List<byte>();

            var placeholderIndex = 0;

            foreach (string substring in text.Split(',').Select(item => item.Trim()))
            {
                if (substring == "*")
                {
                    if (placeholderIndex >= placeHolderRanges.Length)
                    {
                        throw new ArgumentException($"{substring} is invalid");
                    }
                    var placeholderRange = placeHolderRanges[placeholderIndex];
                    var placeholderLength = placeholderRange.End.Value - placeholderRange.Start.Value;

                    data.AddRange(Enumerable.Repeat<byte>(0xFF, placeholderLength));

                    placeholderIndex++;
                }
                else
                {
                    if (!byte.TryParse(substring, out byte byteData))
                    {
                        throw new ArgumentException($"{substring} is invalid");
                    }
                    data.Add(byteData);
                }
            }

            return data.ToArray();
        }

        private IEnumerable<byte> parseByteArray(string text)
        {
            foreach (var substring in text.Split(','))
            {
                if (!byte.TryParse(substring, out byte byteData))
                {
                    throw new ArgumentException($"{substring} is invalid");
                }
                yield return byteData;
            }
        }

        private IEnumerable<byte[]> parseStoredData(string text)
        {
            foreach (var substring in text.Split(Environment.NewLine))
            {
                yield return parseByteArray(substring).ToArray();
            }
        }

        //private readonly struct Range
        //{
        //    public int Position { get; init; }
        //    public int Length { get; init; }
        //}

        private IEnumerable<ushort> parseMatchData(string text)
        {
            foreach (string substring in text.Split(','))
            {
                if (!ushort.TryParse(substring, out ushort data) || data > byte.MaxValue)
                {
                    throw new ArgumentException($"{substring} is invalid");
                }
                yield return data;
            }
        }
    }
}
