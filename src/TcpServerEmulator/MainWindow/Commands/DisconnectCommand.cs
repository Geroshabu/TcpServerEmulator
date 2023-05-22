using System;
using System.Windows.Input;
using TcpServerEmulator.Core.Server;

namespace TcpServerEmulator.MainWindow.Commands
{
    internal class DisconnectCommand : ICommand
    {
        private readonly TcpServer server;

        public event EventHandler? CanExecuteChanged;

        public DisconnectCommand(TcpServer server)
        {
            this.server = server;
            this.server.IsRunningChanged += (_, _) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter) => server.IsRunning;

        public void Execute(object? parameter)
        {
            server.Stop();
        }
    }
}
