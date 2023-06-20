namespace TcpServerEmulator.Rules
{
    /// <summary>
    /// ルールに関するプラグインを表すインターフェイス
    /// </summary>
    public interface IRulePlugin
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
