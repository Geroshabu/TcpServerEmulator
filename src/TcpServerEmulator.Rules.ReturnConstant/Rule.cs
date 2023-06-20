using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace TcpServerEmulator.Rules.ReturnConstant
{
    /// <summary>
    /// 受け取った値に対して、固定値を応答するルール
    /// </summary>
    internal class Rule : IRule
    {
        /// <inheritdoc cref="IRule.Name"/>
        public string Name { get; set; } = string.Empty;

        private string receiveDataText = string.Empty;
        /// <summary>受け取るデータとしてユーザが入力した文字列</summary>
        public string ReceiveDataText
        {
            get => receiveDataText;
            set
            {
                receiveDataText = value;
                if (value.Split(',').All(num => byte.TryParse(num, out _)))
                {
                    receiveDataMatch = value.Split(',').Select(byte.Parse).ToArray();
                }
                else
                {
                    receiveDataMatch = null;
                }
            }
        }

        private string responseDataText = string.Empty;
        /// <summary>返却するデータとしてユーザが入力した文字列</summary>
        public string ResponseDataText
        {
            get => responseDataText;
            set
            {
                responseDataText = value;
                if (value.Split(',').All(num => byte.TryParse(num, out _)))
                {
                    responseDataMatch = value.Split(',').Select(byte.Parse).ToArray();
                }
                else
                {
                    responseDataMatch = null;
                }
            }
        }

        private byte[]? _receiveDataMatch = null;
        private byte[]? receiveDataMatch
        {
            get => _receiveDataMatch;
            set
            {
                _receiveDataMatch = value;
                IsValidChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private byte[]? _responseDataMatch = null;
        private byte[]? responseDataMatch
        {
            get => _responseDataMatch;
            set
            {
                _responseDataMatch = value;
                IsValidChanged?.Invoke(this,EventArgs.Empty);
            }
        }

        /// <inheritdoc cref="IRule.IsValid"/>
        [MemberNotNullWhen(true, nameof(receiveDataMatch), nameof(responseDataMatch))]
        public bool IsValid => receiveDataMatch != null && responseDataMatch != null;

        /// <inheritdoc cref="IRule.IsValidChanged"/>
        public event EventHandler? IsValidChanged;

        /// <inheritdoc cref="IRule.Description"/>
        public string Description
        {
            get
            {
                if (!IsValid)
                {
                    return string.Empty;
                }
                return "{" + string.Join(',', receiveDataMatch.Take(10))
                    + (receiveDataMatch.Length > 10 ? "..." : string.Empty) + "}に対し{"
                    + string.Join(',', responseDataMatch.Take(10))
                    + (responseDataMatch.Length > 10 ? "..." : string.Empty) + "}を返します。";
            }
        }

        /// <inheritdoc cref="IRule.CanResponse(byte[])"/>
        public bool CanResponse(byte[] receivedData)
        {
            return IsValid && receiveDataMatch.SequenceEqual(receivedData);
        }

        /// <inheritdoc cref="IRule.GetResponse(byte[])"/>
        public byte[] GetResponse(byte[] receivedData)
        {
            return IsValid ? responseDataMatch : throw new InvalidOperationException();
        }
    }
}
