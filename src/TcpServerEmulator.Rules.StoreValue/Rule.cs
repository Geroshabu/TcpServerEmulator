using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace TcpServerEmulator.Rules.StoreValue
{
    /// <summary>
    /// 得た値を保持し、後で返却可能なルール
    /// </summary>
    internal class Rule : IRule, IEditableRule
    {
        /// <inheritdoc cref="IEditableRule.Id"/>
        public Guid Id => Plugin.Id;

        /// <inheritdoc cref="IRule.Name"/>
        /// <inheritdoc cref="IEditableRule.Name"/>
        public string Name { get; set; } = string.Empty;

        private string setterReceiveDataText = string.Empty;
        /// <summary>設定コマンドで受け取るデータとしてユーザが入力した文字列</summary>
        public string SetterReceiveDataText
        {
            get => setterReceiveDataText;
            set
            {
                setterReceiveDataText = value;
                reevaluateStoringValue(value, GetterResponseDataText, InitialValuesText);
            }
        }

        private string setterResponseDataText = string.Empty;
        /// <summary>設定コマンドで返却するデータとしてユーザが入力した文字列</summary>
        public string SetterResponseDataText
        {
            get => setterResponseDataText;
            set
            {
                setterResponseDataText = value;
                if (tryParseByteArray(value, out var bytes))
                {
                    setterResponseAsByte = bytes;
                }
                else
                {
                    setterResponseAsByte = null;
                }
            }
        }

        private string getterReceiveDataText = string.Empty;
        /// <summary>取得コマンドで受け取るデータとしてユーザが入力した文字列</summary>
        public string GetterReceiveDataText
        {
            get => getterReceiveDataText;
            set
            {
                getterReceiveDataText = value;
                if (tryParseByteArray(value, out var bytes))
                {
                    getterReceiveAsByte = bytes;
                }
                else
                {
                    getterReceiveAsByte = null;
                }
            }
        }

        private string getterResponseDataText = string.Empty;
        /// <summary>取得コマンドで返却するデータとしてユーザが入力した文字列</summary>
        public string GetterResponseDataText
        {
            get => getterResponseDataText;
            set
            {
                getterResponseDataText = value;
                reevaluateStoringValue(SetterReceiveDataText, value, InitialValuesText);
            }
        }

        private string initialValuesText = string.Empty;
        /// <summary>保持する値の初期値としてユーザが入力した文字列</summary>
        public string InitialValuesText
        {
            get => initialValuesText;
            set
            {
                initialValuesText = value;
                reevaluateStoringValue(SetterReceiveDataText, GetterResponseDataText, value);
            }
        }

        private void reevaluateStoringValue(
            string setterReceiveText,
            string getterResponseText,
            string initialValuesText)
        {
            // Setter の受信パターン解析
            if (tryParseReceiveData(
                setterReceiveText,
                out var setterReceiveAsByte,
                out var matchRanges,
                out var placeholderRanges))
            {
                this.setterReceiveAsByte = setterReceiveAsByte;
                this.matchRanges = matchRanges;
                this.setterPlaceholderRanges = placeholderRanges;
            }
            else
            {
                this.setterReceiveAsByte = null;
                return;
            }

            // Getter で送り返すデータ解析
            if (tryParseResponseData(
                getterResponseText,
                placeholderRanges,
                out var getterResponseAsByte,
                out var getterPlaceholderRanges))
            {
                this.getterResponseAsByte = getterResponseAsByte;
                this.getterPlaceholderRanges = getterPlaceholderRanges;
            }
            else
            {
                this.getterResponseAsByte = null;
                return;
            }

            // 初期値解析
            if (tryParseStoredData(initialValuesText, out var storedDataAsByte))
            {
                this.storedDataAsByte = storedDataAsByte;
            }
            else
            {
                this.storedDataAsByte = null;
                return;
            }

            // 初期値埋め込み
            if (!tryEmbedStoredValue(getterResponseAsByte, storedDataAsByte, getterPlaceholderRanges))
            {
                this.getterResponseAsByte = null;
            }
        }

        // バイト表現

        private byte[]? _setterReceiveAsByte = null;
        private byte[]? setterReceiveAsByte
        {
            get => _setterReceiveAsByte;
            set
            {
                _setterReceiveAsByte = value;
                IsValidChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private byte[]? _setterResponseAsByte = null;
        private byte[]? setterResponseAsByte
        {
            get => _setterResponseAsByte;
            set
            {
                _setterResponseAsByte = value;
                IsValidChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private byte[]? _getterReceiveAsByte = null;
        private byte[]? getterReceiveAsByte
        {
            get => _getterReceiveAsByte;
            set
            {
                _getterReceiveAsByte = value;
                IsValidChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private byte[]? _getterResponseAsByte = null;
        private byte[]? getterResponseAsByte
        {
            get => _getterResponseAsByte;
            set
            {
                _getterResponseAsByte = value;
                IsValidChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private List<byte[]>? _storedDataAsByte = null;
        private List<byte[]>? storedDataAsByte
        {
            get => _storedDataAsByte;
            set
            {
                _storedDataAsByte = value;
                IsValidChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private Range[]? matchRanges { get; set; }
        private Range[]? setterPlaceholderRanges { get; set; }
        private Range[]? getterPlaceholderRanges { get; set; }

        /// <inheritdoc cref="IEditableRule.IsValid"/>
        [MemberNotNullWhen(true,
            nameof(setterReceiveAsByte), nameof(setterResponseAsByte),
            nameof(getterReceiveAsByte), nameof(getterResponseAsByte),
            nameof(storedDataAsByte), nameof(matchRanges),
            nameof(setterPlaceholderRanges), nameof(getterPlaceholderRanges))]
        public bool IsValid =>
            setterReceiveAsByte != null &&
            setterResponseAsByte != null &&
            getterReceiveAsByte != null &&
            getterResponseAsByte != null &&
            storedDataAsByte != null &&
            matchRanges != null &&
            setterPlaceholderRanges != null &&
            getterPlaceholderRanges != null;

        /// <inheritdoc cref="IEditableRule.IsValidChanged"/>
        public event EventHandler? IsValidChanged;

        /// <inheritdoc cref="IRule.Description"/>
        public string Description => string.Empty;

        /// <summary>
        /// <see cref="Rule"/>インスタンスを生成する。
        /// 生成されたインスタンスの各プロパティ値は初期値となる。
        /// 初期値は各プロパティを参照のこと。
        /// </summary>
        public Rule() { }

        /// <summary>コピーコンストラクタ</summary>
        private Rule(Rule source)
        {
            Name = source.Name;
            SetterReceiveDataText = source.SetterReceiveDataText;
            SetterResponseDataText = source.SetterResponseDataText;
            GetterReceiveDataText = source.GetterReceiveDataText;
            GetterResponseDataText = source.GetterResponseDataText;
            InitialValuesText = source.InitialValuesText;
        }

        /// <inheritdoc cref="IRule.CanResponse(byte[])"/>
        public bool CanResponse(byte[] receivedData)
        {
            if (!IsValid)
            {
                return false;
            }

            if (receivedData.SequenceEqual(getterReceiveAsByte))
            {
                return true;
            }

            return receivedData.Length == setterReceiveAsByte.Length
                && matchRanges.All(range => sequenceEqualsRange(setterReceiveAsByte, receivedData, range));
        }

        /// <inheritdoc cref="IEditableRule.AsImmutableRule"/>
        public IRule AsImmutableRule() => new Rule(this);

        private bool sequenceEqualsRange(byte[] expected, byte[] actual, Range range)
        {
            return expected.AsSpan(range).SequenceEqual(actual.AsSpan(range));
        }

        /// <inheritdoc cref="IRule.GetResponse(byte[])"/>
        public byte[] GetResponse(byte[] receivedData)
        {
            if (!IsValid)
            {
                throw new InvalidOperationException();
            }

            if (receivedData.SequenceEqual(getterReceiveAsByte))
            {
                return getterResponseAsByte;
            }

            storedDataAsByte = setterPlaceholderRanges
                .Select(range => receivedData.AsSpan(range).ToArray())
                .ToList();

            if (tryEmbedStoredValue(getterResponseAsByte, storedDataAsByte, getterPlaceholderRanges))
            {
                return setterResponseAsByte;
            }

            throw new InvalidOperationException();
        }

        private bool tryParseReceiveData(
            string text,
            [NotNullWhen(true)] out byte[]? dataAsByte,
            [NotNullWhen(true)] out Range[]? matchRanges,
            [NotNullWhen(true)] out Range[]? placeholderRanges)
        {
            dataAsByte = null;
            matchRanges = null;
            placeholderRanges = null;

            var index = 0;
            var matchStartPosition = 0;
            var matchLength = 0;

            var dataBuffer = new List<byte>();
            var matchRangesBuffer = new List<Range>();
            var placeholderRangesBuffer = new List<Range>();

            var regex = new Regex(@"^\*(\d+)$");

            foreach (string substring in text.Split(','))
            {
                var match = regex.Match(substring);
                if (match.Success) // プレースホルダー "*{N}" の場合
                {
                    // これまで解析済みの数字列を Match Range として残す
                    if (matchLength > 0)
                    {
                        matchRangesBuffer.Add(new Range(matchStartPosition, index));
                        matchLength = 0;
                    }


                    if (!tryAddPlaceholderRange(match.Groups[1].Value, dataBuffer, placeholderRangesBuffer, ref index))
                    {
                        return false;
                    }
                }
                else // 数字の場合
                {
                    if (!byte.TryParse(substring, out byte byteData))
                    {
                        return false;
                    }

                    dataBuffer.Add(byteData);

                    // Range は、連続した数字列が終わるとき
                    // (プレースホルダーの登場 or 入力の終わり) に生成するので、
                    // ここではそのための情報を残すのみ
                    if (matchLength == 0)
                    {
                        matchStartPosition = index;
                    }
                    matchLength++;

                    index++;
                }
            }

            // これまで解析済みの数字列を Match Range として残す
            if (matchLength > 0)
            {
                matchRangesBuffer.Add(new Range(matchStartPosition, index));
            }

            dataAsByte = dataBuffer.ToArray();
            matchRanges = matchRangesBuffer.ToArray();
            placeholderRanges = placeholderRangesBuffer.ToArray();
            return true;
        }

        private bool tryAddPlaceholderRange(
            string text,
            List<byte> dataBuffer,
            List<Range> matchRangesBuffer,
            ref int index)
        {
            if (!int.TryParse(text, out int count))
            {
                return false;
            }

            dataBuffer.AddRange(Enumerable.Repeat<byte>(0xFF, count));
            matchRangesBuffer.Add(new Range(index, index + count));
            index += count;
            return true;
        }

        private bool tryParseResponseData(
            string text,
            Range[] setterPlaceholderRanges,
            [NotNullWhen(true)] out byte[]? dataAsByte,
            [NotNullWhen(true)] out Range[]? getterPlaceholderRanges)
        {
            dataAsByte = null;
            getterPlaceholderRanges = null;
            var dataBuffer = new List<byte>();
            var placeholderBufffer = new List<Range>();

            var placeholderIndex = 0;

            foreach (string substring in text.Split(','))
            {
                if (substring == "*")
                {
                    if (placeholderIndex >= setterPlaceholderRanges.Length)
                    {
                        return false;
                    }
                    var placeholderRange = setterPlaceholderRanges[placeholderIndex];
                    var placeholderLength = placeholderRange.End.Value - placeholderRange.Start.Value;

                    dataBuffer.AddRange(Enumerable.Repeat<byte>(0xFF, placeholderLength));
                    placeholderBufffer.Add(setterPlaceholderRanges[placeholderIndex]);

                    placeholderIndex++;
                }
                else
                {
                    if (!byte.TryParse(substring, out byte byteData))
                    {
                        return false;
                    }
                    dataBuffer.Add(byteData);
                }
            }

            dataAsByte = dataBuffer.ToArray();
            getterPlaceholderRanges = placeholderBufffer.ToArray();
            return true;
        }

        private bool tryParseByteArray(
            string text,
            [NotNullWhen(true)] out byte[]? result)
        {
            result = null;
            var buffer = new List<byte>();
            foreach (var substring in text.Split(','))
            {
                if (!byte.TryParse(substring, out byte byteData))
                {
                    return false;
                }
                buffer.Add(byteData);
            }
            result = buffer.ToArray();
            return true;
        }

        private bool tryParseStoredData(
            string text,
            [NotNullWhen(true)] out List<byte[]>? byteData)
        {
            byteData = null;
            var buffer = new List<byte[]>();
            var input = text
                .Split(Environment.NewLine)
                .Where(substring => !string.IsNullOrWhiteSpace(substring));
            foreach (var substring in input)
            {
                if (!tryParseByteArray(substring, out var initialValueAsByte))
                {
                    return false;
                }
                buffer.Add(initialValueAsByte);
            }

            byteData = buffer;
            return true;
        }

        private bool tryEmbedStoredValue(
            byte[] responseAsByte,
            IReadOnlyList<byte[]> storedValues,
            Range[] placeholderRanegs)
        {
            if (placeholderRanegs.Length > storedValues.Count)
            {
                return false;
            }

            for (int i = 0; i < placeholderRanegs.Length; i++)
            {
                if (placeholderRanegs[i].End.Value > responseAsByte.Length)
                {
                    return false;
                }
                storedValues[i].CopyTo(responseAsByte, placeholderRanegs[i].Start.Value);
            }

            return true;
        }
    }
}
