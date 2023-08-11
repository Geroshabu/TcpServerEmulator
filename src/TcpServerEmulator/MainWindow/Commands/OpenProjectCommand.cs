using System;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Win32;

namespace TcpServerEmulator.MainWindow.Commands
{
    /// <summary>
    /// ファイルからプロジェクトを読み込むコマンド
    /// </summary>
    internal class OpenProjectCommand : ICommand
    {
#pragma warning disable 0067 // 本コマンドは非活性制御をせず、本イベントを使用しない
        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        /// <inheritdoc cref="ICommand.CanExecute(object?)"/>
        public bool CanExecute(object? parameter) => true;

        /// <inheritdoc cref="ICommand.Execute(object?)"/>
        public void Execute(object? parameter)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "TcpServerEmulatorプロジェクトファイル|*.tse";
            if (dialog.ShowDialog() == true)
            {
                Debug.WriteLine("Load project from " + dialog.FileName);
            }
        }
    }
}
