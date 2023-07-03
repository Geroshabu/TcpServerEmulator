using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace TcpServerEmulator.Rules.ReturnConstant.DetailWindow
{
    internal class ViewModel : BindableBase, IDialogAware
    {
        private Rule? _model = null;
        [DisallowNull]
        private Rule? model
        {
            get => _model;
            set
            {
                _model = value;
                RaisePropertyChanged(nameof(canExecuteOk));
            }
        }

        /// <summary>この ViewModel が、操作の対象となるモデルを持っているか否か</summary>
        [MemberNotNullWhen(true, nameof(model), nameof(_model))]
        public bool HasModel => model != null;

        [MemberNotNull(nameof(model), nameof(_model))]
        private void assertHasModel()
        {
            if (!HasModel)
            {
                throw new InvalidOperationException();
            }
        }

        /// <inheritdoc cref="IDialogAware.Title"/>
        public string Title => "定数返却ルールの編集";

        private DelegateCommand? okCommand;
        /// <summary>OKボタンのコマンド</summary>
        public ICommand OkCommand =>
            okCommand ?? (okCommand = new DelegateCommand(() =>
            {
                if (HasModel)
                {
                    var dialogParameter = new DialogParameters
                    {
                        { nameof(IRule), model }
                    };
                    RaiseRequestClose(new DialogResult(ButtonResult.OK, dialogParameter));
                }
            })
            .ObservesCanExecute(() => canExecuteOk));

        private bool canExecuteOk => HasModel && model.IsValid;

        private void handleIsValidChanged(object? sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(canExecuteOk));
        }

        private DelegateCommand? cancelCommand;
        /// <summary>キャンセルボタンのコマンド</summary>
        public ICommand CancelCommand =>
            cancelCommand ?? (cancelCommand = new DelegateCommand(() =>
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
            }));

        /// <inheritdoc cref="IDialogAware.RequestClose"/>
        public event Action<IDialogResult>? RequestClose;

        /// <summary>ユーザーが入力した「受け取るデータ」</summary>
        /// <exception cref="InvalidOperationException">
        ///   この ViewModel が操作対象となるモデルを持っていない場合にアクセスされたとき。
        ///   <seealso cref="HasModel"/>
        /// </exception>
        public string ReceiveDataText
        {
            get => model?.ReceiveDataText ?? throw new InvalidOperationException();
            set
            {
                assertHasModel();
                model.ReceiveDataText = value;
            }
        }

        /// <summary>ユーザーが入力した「返却するデータ」</summary>
        /// <exception cref="InvalidOperationException">
        ///   この ViewModel が操作対象となるモデルを持っていない場合にアクセスされたとき。
        ///   <seealso cref="HasModel"/>
        /// </exception>
        public string ResponseDataText
        {
            get => model?.ResponseDataText ?? throw new InvalidOperationException();
            set
            {
                assertHasModel();
                model.ResponseDataText = value;
            }
        }

        public string Name
        {
            get => model?.Name ?? string.Empty;
            set
            {
                if (HasModel)
                {
                    model.Name = value;
                }
            }
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        /// <inheritdoc cref="IDialogAware.CanCloseDialog"/>/>
        public bool CanCloseDialog() => true;

        /// <inheritdoc cref="IDialogAware.OnDialogClosed"/>
        public void OnDialogClosed()
        {
            if (HasModel)
            {
                model.IsValidChanged -= handleIsValidChanged;
            }
        }

        /// <inheritdoc cref="IDialogAware.OnDialogOpened(IDialogParameters)"/>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue<Rule>(nameof(IRule), out var rule))
            {
                model = rule;
                model.IsValidChanged += handleIsValidChanged;
            }
        }
    }
}
