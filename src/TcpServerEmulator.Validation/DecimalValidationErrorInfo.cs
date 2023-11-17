namespace TcpServerEmulator.Validation
{
    /// <summary>
    /// 10進整数ではないエラー
    /// </summary>
    public class DecimalValidationErrorInfo : IValidationErrorInfo
    {
        /// <inheritdoc cref="IValidationErrorInfo.Message"/>
        public string Message => "整数を入力してください。";
    }
}
