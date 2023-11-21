using System;
using System.Windows.Input;
using Prism.Services.Dialogs;
using TcpServerEmulator.Core.Project;
using TcpServerEmulator.Rules;
using TcpServerEmulator.UI.Common.Wpf;

namespace TcpServerEmulator.MainWindow.Commands
{
    /// <summary>
    /// ルールを追加するコマンド
    /// </summary>
    internal class AddRuleCommand : ICommand
    {
        private readonly IDialogService dialogService;
        private readonly ProjectHolder projectHolder;

#pragma warning disable 0067
        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        public AddRuleCommand(
            IDialogService dialogService,
            ProjectHolder projectHolder)
        {
            this.dialogService = dialogService;
            this.projectHolder = projectHolder;
        }

        /// <inheritdoc cref="ICommand.CanExecute(object?)"/>
        public bool CanExecute(object? parameter) => true;

        /// <inheritdoc cref="ICommand.Execute(object?)"/>
        public void Execute(object? parameter)
        {
            if (parameter is IRulePlugin plugin)
            {
                var initialRule = plugin.CreateInitialRule();

                var dialogParameters = new DialogParameters
                {
                    { nameof(IEditableRule), initialRule },
                    { nameof(RuleEditMode), RuleEditMode.Add }
                };

                dialogService.ShowDialog(typeof(EditRuleWindow.View).FullName, dialogParameters, result =>
                {
                    if (result.Result == ButtonResult.OK &&
                        result.Parameters.TryGetValue(nameof(IRule), out IRule rule))
                    {
                        projectHolder.Current.RuleCollection.AddRule(rule);
                    }
                });
            }
        }
    }
}
