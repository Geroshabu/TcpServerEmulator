namespace TcpServerEmulator.Validation
{
    /// <summary>
    /// 範囲超過エラー
    /// </summary>
    /// <typeparam name="T">エラーを起こした値の型</typeparam>
    public class RangeValidationErrorInfo<T> : IValidationErrorInfo where T : IComparable<T>
    {
        private readonly T minimum;
        private readonly T maximum;

        /// <inheritdoc cref="IValidationErrorInfo.Message"/>
        public string Message => $"{minimum}～{maximum}の値を入力してください。";

        public RangeValidationErrorInfo(T minimum, T maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => Message;
    }
}
