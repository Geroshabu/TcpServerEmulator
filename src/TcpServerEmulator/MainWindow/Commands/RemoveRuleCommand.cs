using System;
using System.Windows.Input;
using TcpServerEmulator.Core;
using TcpServerEmulator.Rules;

namespace TcpServerEmulator.MainWindow.Commands
{
    /// <summary>
    /// 一覧からルールを削除するコマンド
    /// </summary>
    internal class RemoveRuleCommand : ICommand
    {
        private readonly RuleCollection ruleCollection;
        private readonly IRule targetRule;

#pragma warning disable 0067 // 本コマンドは非活性制御をせず、本イベントを使用しない
        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        public RemoveRuleCommand(
            RuleCollection ruleCollection,
            IRule targetRule)
        {
            this.ruleCollection = ruleCollection;
            this.targetRule = targetRule;
        }

        /// <inheritdoc cref="ICommand.CanExecute(object?)"/>
        public bool CanExecute(object? parameter) => true;

        /// <inheritdoc cref="ICommand.Execute(object?)"/>
        public void Execute(object? parameter)
        {
            ruleCollection.RemoveRule(targetRule);
        }
    }
}
