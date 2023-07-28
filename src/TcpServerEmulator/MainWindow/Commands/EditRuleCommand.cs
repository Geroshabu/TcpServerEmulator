using System;
using System.Windows.Input;
using Prism.Services.Dialogs;
using TcpServerEmulator.Core;
using TcpServerEmulator.Rules;

namespace TcpServerEmulator.MainWindow.Commands
{
    /// <summary>
    /// 既存のルールを編集するコマンド
    /// </summary>
    internal class EditRuleCommand : ICommand
    {
        private readonly IDialogService dialogService;
        private readonly RuleHolder ruleHolder;
        private readonly IRule targetRule;

#pragma warning disable 0067 // 本コマンドは非活性制御をせず、本イベントを使用しない
        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        public EditRuleCommand(
            IDialogService dialogService,
            RuleHolder ruleHolder,
            IRule targetRule)
        {
            this.dialogService = dialogService;
            this.ruleHolder = ruleHolder;
            this.targetRule = targetRule;
        }

        /// <inheritdoc cref="ICommand.CanExecute(object?)"/>
        public bool CanExecute(object? parameter) => true;

        /// <inheritdoc cref="ICommand.Execute(object?)"/>
        public void Execute(object? parameter)
        {
            var dialogParameters = new DialogParameters
            {
                { nameof(IEditableRule), targetRule.AsEditableRule() }
            };

            dialogService.ShowDialog(typeof(EditRuleWindow.View).FullName, dialogParameters, result =>
            {
                if (result.Result == ButtonResult.OK &&
                    result.Parameters.TryGetValue(nameof(IRule), out IRule rule))
                {
                    ruleHolder.ReplaceRule(targetRule, rule);
                }
            });
        }
    }
}
