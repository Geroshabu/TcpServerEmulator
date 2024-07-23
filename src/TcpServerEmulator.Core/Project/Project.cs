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
        [DataMember]
        public PortNumber PortNumber { get; set; }

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
