using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using TcpServerEmulator.Core.Server;

namespace TcpServerEmulator.Core.Project
{
    /// <summary>
    /// TcpServerEmulator のプロジェクトを表すクラス
    /// </summary>
    [DataContract]
    public class Project
    {
        /// <summary>
        /// ポート番号
        /// </summary>
        public PortNumber PortNumber { get; set; }

        [DataMember]
        private string portNumberText
        {
            get => PortNumber.Value.ToString();
            set
            {
                if (PortNumber.GetFactory().TryParse(value, out var portNumber, out _))
                {
                    PortNumber = portNumber;
                }
                else
                {
                    throw new SerializationException($"Text \"{value}\" is invalid for port number");
                }
            }
        }

        /// <summary>
        /// ルール一覧
        /// </summary>
        [DataMember]
        public RuleCollection RuleCollection { get; private set; }

        /// <summary>
        /// <see cref="Project"/>インスタンスの初期化と生成
        /// </summary>
        public Project() => initialize();

        [OnDeserializing]
        private void onDeserializing(StreamingContext context) => initialize();

        [MemberNotNull(nameof(PortNumber), nameof(RuleCollection))]
        private void initialize()
        {
            PortNumber = new PortNumber(0);
            RuleCollection = new RuleCollection();
        }
    }
}
