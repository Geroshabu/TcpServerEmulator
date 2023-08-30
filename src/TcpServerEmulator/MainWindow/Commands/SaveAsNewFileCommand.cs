using System;
using System.Windows.Input;
using Microsoft.Win32;
using TcpServerEmulator.Core.Perpetuation;
using TcpServerEmulator.Core.Project;

namespace TcpServerEmulator.MainWindow.Commands
{
    /// <summary>
    /// 現在の設定を新しいファイルとして保存するコマンド
    /// </summary>
    internal class SaveAsNewFileCommand : ICommand
    {
        private readonly ISave saver;
        private readonly ProjectHolder projectHolder;

#pragma warning disable 0067 // 本コマンドは非活性制御をせず、本イベントを使用しない
        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        public SaveAsNewFileCommand(
            ISave saver,
            ProjectHolder projectHolder)
        {
            this.saver = saver;
            this.projectHolder = projectHolder;

            this.projectHolder.CurrentProjectChanged += (_, _) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
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
                saver.SaveProject(dialog.FileName, projectHolder.Current);
            }
        }
    }
}
