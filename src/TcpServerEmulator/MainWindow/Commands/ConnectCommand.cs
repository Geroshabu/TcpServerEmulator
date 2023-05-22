using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TcpServerEmulator.Core.Server;

namespace TcpServerEmulator.MainWindow.Commands
{
    internal class ConnectCommand : ICommand
    {
        private readonly TcpServer server;

        public event EventHandler? CanExecuteChanged;

        public ConnectCommand(TcpServer server)
        {
            this.server = server;
            this.server.IsRunningChanged += (_, _) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter) => !server.IsRunning;

        public void Execute(object? parameter)
        {
            server.Run();
        }
    }
}
