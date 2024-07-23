using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using TcpServerEmulator.Validation;

namespace TcpServerEmulator.Core.Server
{
    /// <summary>
    /// ポート番号を表すクラス
    /// </summary>
    // 等価判定が正しく行われなくなる可能性があるので継承を禁じる
    [DataContract]
    public sealed class PortNumber : IEquatable<PortNumber>
    {
        /// <summary>
        /// ポート番号を表す整数
        /// </summary>
        public int Value { get; private set; }

        // シリアライズ・デシリアライズ用プロパティ
        [DataMember(IsRequired = true, Name = "Value")]
        private string serializedValue
        {
            get => Value.ToString();
            set
            {
                if (GetFactory().TryParse(value, out var portNumber, out _))
                {
                    Value = portNumber.Value;
                }
                else
                {
                    throw new SerializationException($"Text \"{value}\" is invalid for port number");
                }
            }
        }

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
            /// <inheritdoc cref="IValueFactory{T}.TryParse(string, out T?, out IValidationErrorInfo?)"/>
            public bool TryParse(
                string text,
                [NotNullWhen(true)] out PortNumber? result,
                [NotNullWhen(false)] out IValidationErrorInfo? validationErrorInfo)
            {
                result = null;
                validationErrorInfo = null;
                
                if (string.IsNullOrEmpty(text))
                {
                    validationErrorInfo = new EmptyValidationErrorInfo();
                    return false;
                }
                if (!text.All(char.IsNumber))
                {
                    validationErrorInfo = new DecimalValidationErrorInfo();
                    return false;
                }
                if (!int.TryParse(text, out var number) || !isInRange(number))
                {
                    validationErrorInfo = new RangeValidationErrorInfo<int>(minimum, maximum);
                    return false;
                }
                result = new PortNumber(number);
                return true;
            }
        }

        /// <summary>
        /// <see cref="PortNumber"/>インスタンスを生成するためのファクトリオブジェクトを取得する。
        /// </summary>
        /// <returns>
        ///   <see cref="PortNumber"/>インスタンスを生成するためのファクトリオブジェクト
        /// </returns>
        public static IValueFactory<PortNumber> GetFactory() => new Factory();

        private const int minimum = 0;
        private const int maximum = 65535;

        private static bool isInRange(int number) => number >= minimum && number <= maximum;
    }
}
