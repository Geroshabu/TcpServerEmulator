namespace TcpServerEmulator.Validation
{
    /// <summary>
    /// 入力バリデーションエラーを表す
    /// </summary>
    public interface IValidationErrorInfo
    {
        /// <summary>
        /// ユーザがエラー内容を理解するためのテキスト
        /// </summary>
        string Message { get; }
    }
}
