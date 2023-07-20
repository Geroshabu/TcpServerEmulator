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

#pragma warning disable 0067
        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

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
                var initialRule = plugin.CreateInitialRule();
                var editWindowName = plugin.EditWindowName;

                var dialogParameters = new DialogParameters
                {
                    { nameof(IRule), initialRule },
                    { nameof(IRulePlugin.EditWindowName), editWindowName }
                };

                dialogService.ShowDialog(typeof(EditRuleWindow.View).FullName, dialogParameters, result =>
                {
                    if (result.Result == ButtonResult.OK &&
                        result.Parameters.TryGetValue(nameof(IRule), out IRule rule))
                    {
                        ruleHolder.AddRule(rule);
                    }
                });
            }
        }
    }
}
