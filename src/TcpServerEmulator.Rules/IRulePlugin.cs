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
        /// 新しいルールの編集可能なインスタンスを作成する
        /// </summary>
        /// <returns>新しいルールの編集可能なインスタンス</returns>
        IEditableRule CreateInitialRule();

        /// <summary>
        /// このプラグインが扱うことのできる、
        /// <see cref="IRule"/>の実装クラスの型
        /// </summary>
        Type RuleType { get; }
    }
}
