using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace TcpServerEmulator.Rules.ReturnConstant
{
    /// <summary>
    /// 受け取った値に対して、固定値を応答するルール
    /// </summary>
    [DataContract]
    internal class Rule : IRule, IEditableRule
    {
        /// <inheritdoc cref="IEditableRule.Id"/>
        public Guid Id => Plugin.Id;

        /// <inheritdoc cref="IRule.Name"/>
        /// <inheritdoc cref="IEditableRule.Name"/>
        [DataMember]
        public string Name { get; set; } = string.Empty;

        private string receiveDataText = string.Empty;
        /// <summary>受け取るデータとしてユーザが入力した文字列</summary>
        [DataMember]
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
        [DataMember]
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

        /// <inheritdoc cref="IEditableRule.IsValid"/>
        [MemberNotNullWhen(true, nameof(receiveDataMatch), nameof(responseDataMatch))]
        public bool IsValid => receiveDataMatch != null && responseDataMatch != null;

        /// <inheritdoc cref="IEditableRule.IsValidChanged"/>
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
            ReceiveDataText = source.ReceiveDataText;
            ResponseDataText = source.ResponseDataText;
        }

        /// <inheritdoc cref="IEditableRule.AsImmutableRule"/>
        public IRule AsImmutableRule() => new Rule(this);

        /// <inheritdoc cref="IRule.AsEditableRule"/>
        public IEditableRule AsEditableRule() => new Rule(this);

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
