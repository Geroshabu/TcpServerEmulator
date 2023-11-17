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
        /// ユーザが入力した文字列を指定されたフィールドに設定し、変更通知を行う。
        /// ただし、同じ文字列を保持していた場合は何もしない。
        /// また、文字列のバリデーションを行い、入力された値のインスタンスを生成し、
        /// 指定されたフィールドに設定する。
        /// バリデーションでエラーがある場合は、インスタンスを生成せず、エラー通知を行う。
        /// </summary>
        /// <typeparam name="T">文字列から生成したいインスタンスの型</typeparam>
        /// <param name="textStorage">ユーザが入力した文字列を設定するフィールド</param>
        /// <param name="text">ユーザが入力した文字列</param>
        /// <param name="storage">生成したインスタンスを設定するフィールド</param>
        /// <param name="valueFactory">文字列から<typeparamref name="T"/>インスタンスを生成するファクトリ</param>
        /// <param name="propertyName">バリデーションでエラーがある場合に、エラー通知をするプロパティ名</param>
        /// <returns>インスタンスを設定したか否か</returns>
        protected virtual bool SetPropertyWithValidate<T>(
            ref string textStorage,
            string text,
            ref T storage,
            IValueFactory<T> valueFactory,
            [CallerMemberName] string? propertyName = null)
        {
            if (SetProperty(ref textStorage, text))
            {
                if (valueFactory.TryParse(text, out var result, out var validationErrorInfo))
                {
                    storage = result;
                    errorsContainer.ClearErrors(propertyName);
                    return true;
                }
                else
                {
                    errorsContainer.SetErrors(propertyName, new[] { validationErrorInfo.Message });
                    return false;
                }
            }
            return false;
        }

        private void raiseErrorsChanged(string propertyName)
            => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}
