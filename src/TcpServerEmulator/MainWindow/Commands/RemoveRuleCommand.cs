using System;
using System.Windows.Input;
using TcpServerEmulator.Core;
using TcpServerEmulator.Rules;

namespace TcpServerEmulator.MainWindow.Commands
{
    internal class RemoveRuleCommand : ICommand
    {
        private readonly RuleHolder ruleHolder;

        public event EventHandler? CanExecuteChanged;

        public RemoveRuleCommand(RuleHolder ruleHolder)
        {
            this.ruleHolder = ruleHolder;
        }

        /// <inheritdoc cref="ICommand.CanExecute(object?)"/>
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is IRule selectedRule)
            {
                ruleHolder.RemoveRule(selectedRule);
            }
        }
    }
}
