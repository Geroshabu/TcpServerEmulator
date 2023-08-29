using System;
using System.Windows.Input;
using Microsoft.Win32;
using TcpServerEmulator.Core.Perpetuation;
using TcpServerEmulator.Core.Project;

namespace TcpServerEmulator.MainWindow.Commands
{
    /// <summary>
    /// ファイルからプロジェクトを読み込むコマンド
    /// </summary>
    internal class OpenProjectCommand : ICommand
    {
        private readonly ILoad loader;
        private readonly ProjectHolder projectHolder;

#pragma warning disable 0067 // 本コマンドは非活性制御をせず、本イベントを使用しない
        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        public OpenProjectCommand(
            ILoad loader,
            ProjectHolder projectHolder)
        {
            this.loader = loader;
            this.projectHolder = projectHolder;
        }

        /// <inheritdoc cref="ICommand.CanExecute(object?)"/>
        public bool CanExecute(object? parameter) => true;

        /// <inheritdoc cref="ICommand.Execute(object?)"/>
        public void Execute(object? parameter)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "TcpServerEmulatorプロジェクトファイル|*.tse";
            if (dialog.ShowDialog() == true)
            {
                projectHolder.Current = loader.LoadProject(dialog.FileName);
            }
        }
    }
}
