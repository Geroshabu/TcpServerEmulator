namespace TcpServerEmulator.Rules
{
    /// <summary>
    /// 編集可能なルールを表すインターフェイス
    /// </summary>
    /// <remarks>
    /// ルールの編集を確定するときには、<see cref="AsImmutableRule"/>を呼び、
    /// 完成したルールを得る。
    /// </remarks>
    public interface IEditableRule
    {
        /// <summary>このルールを扱えるプラグインを表すID</summary>
        Guid Id { get; }

        /// <summary>ルールの名前</summary>
        RuleName Name { get; set; }

        /// <summary>
        /// このルールが機能するのに必要な条件を満たしているか
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// <see cref="IsValid"/>の値に影響のある変更がされた場合に発生する
        /// </summary>
        event EventHandler IsValidChanged;

        /// <summary>
        /// ルールの編集を確定し、内容変更できないルールを取得する。
        /// </summary>
        /// <returns>編集内容が確定されたルール</returns>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="IsValid"/>が<c>false</c>の場合
        /// </exception>
        IRule AsImmutableRule();
    }
}
