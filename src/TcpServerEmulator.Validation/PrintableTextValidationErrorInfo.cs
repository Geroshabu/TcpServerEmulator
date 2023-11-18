namespace TcpServerEmulator.Validation
{
    /// <summary>
    /// 印字可能な文字列でないエラー
    /// </summary>
    public class PrintableTextValidationErrorInfo : IValidationErrorInfo
    {
        /// <inheritdoc cref="IValidationErrorInfo.Message"/>
        public string Message => "空白以外の文字を入力してください。";
    }
}
