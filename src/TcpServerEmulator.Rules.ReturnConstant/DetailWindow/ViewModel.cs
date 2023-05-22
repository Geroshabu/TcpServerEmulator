using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace TcpServerEmulator.Rules.ReturnConstant.DetailWindow
{
    internal class ViewModel : BindableBase, IDialogAware
    {
        /// <inheritdoc cref="IDialogAware.Title"/>
        public string Title => "定数返却ルールの編集";

        private DelegateCommand<string>? closeDialogCommand;
        /// <summary>OKまたはCancelでダイアログを閉じるときのコマンド</summary>
        public ICommand CloseDialogCommand =>
            closeDialogCommand ?? (closeDialogCommand = new DelegateCommand<string>(parameter =>
        {
            ButtonResult result = ButtonResult.None;

            var dialogParameter = new DialogParameters();

            if (parameter?.ToLower() == "true")
            {
                result = ButtonResult.OK;


                if (!ReceiveDataText.Split(',').All(num => byte.TryParse(num, out _)) ||
                    !ResponseDataText.Split(',').All(num => byte.TryParse(num, out _)))
                {
                    MessageBox.Show("入力データがbyte[]に変換できません。");
                    return;
                }
                dialogParameter.Add("Rule", new Rule(Name, ReceiveDataText, ResponseDataText));
                result = ButtonResult.OK;
            }
            else if (parameter?.ToLower() == "false")
            {
                result = ButtonResult.Cancel;
            }

            RaiseRequestClose(new DialogResult(result, dialogParameter));
        }));

        /// <inheritdoc cref="IDialogAware.RequestClose"/>
        public event Action<IDialogResult>? RequestClose;

        /// <summary>ユーザーが入力した「受け取るデータ」</summary>
        public string ReceiveDataText { get; set; } = string.Empty;

        /// <summary>ユーザーが入力した「返却するデータ」</summary>
        public string ResponseDataText { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        protected virtual void CloseDialog(string parameter)
        {
            ButtonResult result = ButtonResult.None;

            if (parameter?.ToLower() == "true")
                result = ButtonResult.OK;
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.Cancel;

            RaiseRequestClose(new DialogResult(result));
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
        }

        /// <inheritdoc cref="IDialogAware.OnDialogOpened(IDialogParameters)"/>
        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
