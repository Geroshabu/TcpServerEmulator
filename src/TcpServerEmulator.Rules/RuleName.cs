using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using TcpServerEmulator.Validation;

namespace TcpServerEmulator.Rules
{
    /// <summary>
    /// ルールの名前を表すクラス
    /// </summary>
    [DataContract]
    public class RuleName
    {
        /// <summary>
        /// ルールの名前を表す文字列
        /// </summary>
        [DataMember]
        public string Value { get; }

        /// <summary>
        /// ルール名の文字列を指定して<see cref="RuleName"/>インスタンスを生成する。
        /// なお、指定した文字列に下記が含まれていた場合、取り除かたものがルール名となる。
        /// <list type="bullet">
        ///   <item><description>制御文字</description></item>
        ///   <item><description>先頭と末尾の空白文字</description></item>
        /// </list>
        /// </summary>
        /// <param name="value">ルール名</param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="value"/>が空または空白文字または制御文字のみの場合。
        /// </exception>
        public RuleName(string value)
        {
            if (!isValidName(value))
            {
                throw new ArgumentException("The rule name is empty or white space.");
            }
            Value =  new string(value.Where(c => !char.IsControl(c)).ToArray()).Trim();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => Value;

        private class Factory : IValueFactory<RuleName>
        {
            /// <inheritdoc cref="IValueFactory{T}.TryParse(string, out T?, out IValidationErrorInfo?)"/>
            public bool TryParse(
                string text,
                [NotNullWhen(true)] out RuleName? result,
                [NotNullWhen(false)] out IValidationErrorInfo? validationErrorInfo)
            {
                result = null;
                validationErrorInfo = null;

                if (string.IsNullOrEmpty(text))
                {
                    validationErrorInfo = new EmptyValidationErrorInfo();
                    return false;
                }
                if (!isValidName(text))
                {
                    validationErrorInfo = new PrintableTextValidationErrorInfo();
                    return false;
                }
                result = new RuleName(text);
                return true;
            }
        }

        /// <summary>
        /// <see cref="RuleName"/>インスタンスを生成するためのファクトリオブジェクトを取得する。
        /// </summary>
        /// <returns>
        ///   <see cref="RuleName"/>インスタンスを生成するためのファクトリオブジェクト
        /// </returns>
        public static IValueFactory<RuleName> GetFactory() => new Factory();

        private static bool isValidName(string name)
            => !string.IsNullOrWhiteSpace(name)
            && !name.All(c => char.IsControl(c));

        /// <summary>
        /// デフォルトのルール名
        /// </summary>
        public static RuleName Default { get; } = new RuleName("The rule name");
    }
}
