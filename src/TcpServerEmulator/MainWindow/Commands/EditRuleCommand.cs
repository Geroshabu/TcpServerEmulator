using System;
using System.Windows.Input;
using Prism.Services.Dialogs;
using TcpServerEmulator.Core;
using TcpServerEmulator.Rules;
using TcpServerEmulator.UI.Common.Wpf;

namespace TcpServerEmulator.MainWindow.Commands
{
    /// <summary>
    /// 既存のルールを編集するコマンド
    /// </summary>
    internal class EditRuleCommand : ICommand
    {
        private readonly IDialogService dialogService;
        private readonly RuleCollection ruleCollection;
        private readonly IRule targetRule;

#pragma warning disable 0067 // 本コマンドは非活性制御をせず、本イベントを使用しない
        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        public EditRuleCommand(
            IDialogService dialogService,
            RuleCollection ruleCollection,
            IRule targetRule)
        {
            this.dialogService = dialogService;
            this.ruleCollection = ruleCollection;
            this.targetRule = targetRule;
        }

        /// <inheritdoc cref="ICommand.CanExecute(object?)"/>
        public bool CanExecute(object? parameter) => true;

        /// <inheritdoc cref="ICommand.Execute(object?)"/>
        public void Execute(object? parameter)
        {
            var dialogParameters = new DialogParameters
            {
                { nameof(IEditableRule), targetRule.AsEditableRule() },
                { nameof(RuleEditMode), RuleEditMode.Update }
            };

            dialogService.ShowDialog(typeof(EditRuleWindow.View).FullName, dialogParameters, result =>
            {
                if (result.Result == ButtonResult.OK &&
                    result.Parameters.TryGetValue(nameof(IRule), out IRule rule))
                {
                    ruleCollection.ReplaceRule(targetRule, rule);
                }
            });
        }
    }
}
