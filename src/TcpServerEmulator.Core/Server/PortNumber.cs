using System.Diagnostics.CodeAnalysis;

namespace TcpServerEmulator.Core.Server
{
    /// <summary>
    /// ポート番号を表すクラス
    /// </summary>
    // 等価判定が正しく行われなくなる可能性があるので継承を禁じる
    public sealed class PortNumber : IEquatable<PortNumber>
    {
        /// <summary>
        /// ポート番号を表す整数
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// ポート番号を指定して<see cref="PortNumber"/>インスタンスを生成する。
        /// </summary>
        /// <param name="value">ポート番号(0-65535)</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value"/>が範囲外の場合。
        /// </exception>
        public PortNumber(int value)
        {
            if (!isInRange(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            Value = value;
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
        public bool Equals(PortNumber? other)
            => other is not null
            && Value == other.Value;

        /// <inheritdoc cref="object.Equals(object?)"/>
        public override bool Equals(object? obj)
            => obj is not null
            && GetType() == obj.GetType()
            && Equals((PortNumber)obj);

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(PortNumber? left, PortNumber? right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        public static bool operator !=(PortNumber? left, PortNumber? right) => !(left == right);

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => Value.ToString();

        /// <summary>
        /// 文字列を、<see cref="PortNumber"/>インスタンスに変換できるかを試みる
        /// </summary>
        /// <param name="text">変換したい文字列</param>
        /// <param name="result">変換後のインスタンス</param>
        /// <returns>変換が成功した場合は<c>true</c>、それ以外は<c>false</c></returns>
        public static bool TryParse(string text, [NotNullWhen(true)] out PortNumber? result)
        {
            result = null;
            if (int.TryParse(text, out var number) && isInRange(number))
            {
                result = new PortNumber(number);
            }
            return result != null;
        }

        private static bool isInRange(int number) => number >= 0 && number <= 65535;
    }
}
