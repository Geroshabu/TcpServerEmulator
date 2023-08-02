using System;
using System.Windows.Input;
using Microsoft.Win32;
using TcpServerEmulator.Core;

namespace TcpServerEmulator.MainWindow.Commands
{
    /// <summary>
    /// 現在の設定を新しいファイルとして保存するコマンド
    /// </summary>
    internal class SaveAsNewFileCommand : ICommand
    {
        private readonly RuleHolder ruleHolder;

        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler? CanExecuteChanged;

        public SaveAsNewFileCommand(
            RuleHolder ruleHolder)
        {
            this.ruleHolder = ruleHolder;
        }

        /// <inheritdoc cref="ICommand.CanExecute(object?)"/>
        public bool CanExecute(object? parameter) => true;

        /// <inheritdoc cref="ICommand.Execute(object?)"/>
        public void Execute(object? parameter)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "TcpServerEmulatorプロジェクトファイル|*.tse";
            if (dialog.ShowDialog() == true)
            {
                System.Diagnostics.Debug.WriteLine($"selected: {dialog.FileName}");
            }
        }
    }
}
