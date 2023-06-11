using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows;

namespace TcpServerEmulator.Rules.StoreValue.DetailWindow
{
    internal class ViewModel : BindableBase, IDialogAware
    {
        /// <inheritdoc cref="IDialogAware.Title"/>
        public string Title => "値保持ルールの編集";

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

                    try
                    {
                        var rule = new Rule(
                            Name,
                            SetterReceiveDataText,
                            SetterResponseDataText,
                            GetterReceiveDataText,
                            GetterResponseDataText,
                            InitialValuesText);
                        dialogParameter.Add("Rule", rule);
                    }
                    catch (ArgumentException e)
                    {
                        MessageBox.Show(e.Message);
                        return;
                    }
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

        /// <summary>ユーザーが入力した「設定コマンドで受け取るデータ」</summary>
        public string SetterReceiveDataText { get; set; } = string.Empty;

        /// <summary>ユーザーが入力した「設定コマンドで返却するデータ」</summary>
        public string SetterResponseDataText { get; set; } = string.Empty;

        /// <summary>ユーザーが入力した「取得コマンドで受け取るデータ」</summary>
        public string GetterReceiveDataText { get; set; } = string.Empty;

        /// <summary>ユーザーが入力した「取得コマンドで返却するデータ」</summary>
        public string GetterResponseDataText { get; set; } = string.Empty;

        /// <summary>ユーザーが入力した「保持する値の初期値」</summary>
        public string InitialValuesText { get; set; } = string.Empty;

        /// <summary>ルールの名前</summary>
        public string Name { get; set; } = string.Empty;

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        /// <inheritdoc cref="IDialogAware.CanCloseDialog"/>
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
