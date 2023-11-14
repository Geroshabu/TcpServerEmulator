using System.Diagnostics.CodeAnalysis;
using TcpServerEmulator.Validation;

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

        private class Factory : IValueFactory<PortNumber>
        {
            public bool TryParse(string text, [NotNullWhen(true)] out PortNumber? result)
            {
                result = null;
                if (int.TryParse(text, out var number) && isInRange(number))
                {
                    result = new PortNumber(number);
                }
                return result != null;
            }
        }

        public static IValueFactory<PortNumber> GetFactory() => new Factory();

        private static bool isInRange(int number) => number >= 0 && number <= 65535;
    }
}
