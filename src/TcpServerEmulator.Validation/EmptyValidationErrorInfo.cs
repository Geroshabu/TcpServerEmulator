namespace TcpServerEmulator.Validation
{
    /// <summary>
    /// 必須入力項目が入力されていないエラー
    /// </summary>
    public class EmptyValidationErrorInfo : IValidationErrorInfo
    {
        /// <inheritdoc cref="IValidationErrorInfo.Message"/>
        public string Message => "値を入力してください。";
    }
}
