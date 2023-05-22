namespace TcpServerEmulator.Rules
{
    /// <summary>
    /// ルール生成者
    /// </summary>
    public interface IRuleGenerator
    {
        /// <summary>
        /// 生成するルールの種類名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 編集ウインドウ名
        /// </summary>
        string EditWindowName { get; }
    }
}
