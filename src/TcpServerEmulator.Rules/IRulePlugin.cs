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

        /// <summary>
        /// 新しいルールのインスタンスを作成する
        /// </summary>
        /// <returns>新しいルールのインスタンス</returns>
        IRule CreateInitialRule();
    }
}
