using System.Runtime.Serialization;

namespace TcpServerEmulator.Core.Project
{
    /// <summary>
    /// TcpServerEmulator のプロジェクトを表すクラス
    /// </summary>
    [DataContract]
    public class Project
    {
        [DataMember]
        public RuleCollection RuleCollection { get; private set; } = new();
    }
}
