using System;
using System.Diagnostics;
using System.Windows.Input;
using Prism.Services.Dialogs;
using TcpServerEmulator.Core;
using TcpServerEmulator.Rules;

namespace TcpServerEmulator.MainWindow.Commands
{
    internal class AddRuleCommand : ICommand
    {
        private readonly IDialogService dialogService;
        private readonly RuleHolder ruleHolder;

        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler? CanExecuteChanged;

        public AddRuleCommand(
            IDialogService dialogService,
            RuleHolder ruleHolder)
        {
            this.dialogService = dialogService;
            this.ruleHolder = ruleHolder;
        }

        /// <inheritdoc cref="ICommand.CanExecute(object?)"/>
        public bool CanExecute(object? parameter) => true;

        /// <inheritdoc cref="ICommand.Execute(object?)"/>
        public void Execute(object? parameter)
        {
            if (parameter is IRulePlugin plugin)
            {
                dialogService.ShowDialog(plugin.EditWindowName, result =>
                {
                    if (result.Result == ButtonResult.OK &&
                        result.Parameters.TryGetValue("Rule", out IRule rule))
                    {
                        ruleHolder.AddRule(rule);
                    }
                });
            }
        }
    }
}
