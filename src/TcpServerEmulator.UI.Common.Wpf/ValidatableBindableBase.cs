using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Mvvm;
using TcpServerEmulator.Validation;

namespace TcpServerEmulator.UI.Common.Wpf
{
    /// <summary>
    /// 入力バリデーション機能を持った ViewModel
    /// </summary>
    public abstract class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        private readonly ErrorsContainer<string> errorsContainer;

        /// <inheritdoc cref="INotifyDataErrorInfo.HasErrors"/>
        public bool HasErrors => errorsContainer.HasErrors;

        /// <inheritdoc cref="INotifyDataErrorInfo.ErrorsChanged"/>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public ValidatableBindableBase()
        {
            errorsContainer = new ErrorsContainer<string>(raiseErrorsChanged);
        }

        /// <inheritdoc cref="INotifyDataErrorInfo.GetErrors(string?)"/>
        public IEnumerable GetErrors(string? propertyName)
            => errorsContainer.GetErrors(propertyName);

        /// <summary>
        /// ユーザが入力した文字列のバリデーションを行い、
        /// 入力された値のインスタンスを生成し、指定されたフィールドに設定する。
        /// バリデーションでエラーがある場合は、インスタンスを生成せず、エラー通知を行う。
        /// </summary>
        /// <typeparam name="T">文字列から生成したいインスタンスの型</typeparam>
        /// <param name="storage">生成したインスタンスを設定するフィールド</param>
        /// <param name="value">ユーザが入力した文字列</param>
        /// <param name="valueFactory">文字列から<typeparamref name="T"/>インスタンスを生成するファクトリ</param>
        /// <param name="propertyName">バリデーションでエラーがある場合に、エラー通知をするプロパティ名</param>
        /// <returns>インスタンスを設定したか否か</returns>
        protected virtual bool SetValidatedProperty<T>(
            ref T storage,
            string value,
            IValueFactory<T> valueFactory,
            [CallerMemberName] string? propertyName = null)
        {
            if (valueFactory.TryParse(value, out var result))
            {
                storage = result;
                errorsContainer.ClearErrors(propertyName);
                return true;
            }
            else
            {
                errorsContainer.SetErrors(propertyName, new[] { "入力エラー" });
                return false;
            }
        }

        private void raiseErrorsChanged(string propertyName)
            => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}
