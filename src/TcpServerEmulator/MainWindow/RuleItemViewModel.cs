using System.Windows.Input;
using Prism.Mvvm;
using TcpServerEmulator.Rules;

namespace TcpServerEmulator.MainWindow
{
    /// <summary>
    /// ルールの一覧の一要素の ViewModel
    /// </summary>
    internal class RuleItemViewModel : BindableBase
    {
        /// <summary>
        /// このViewModelのモデルとなるルール
        /// </summary>
        public IRule Rule { get; }

        /// <summary>
        /// このルールの名前
        /// </summary>
        public RuleName Name => Rule.Name;

        /// <summary>
        /// このルールの説明文
        /// </summary>
        public string Description => Rule.Description;

        /// <summary>このルールを編集するためのコマンド</summary>
        public ICommand EditCommand { get; }

        /// <summary>このルールを一覧から削除するためのコマンド</summary>
        public ICommand RemoveCommand { get; }

        /// <summary>
        /// <see cref="RuleItemViewModel"/>インスタンスの生成と初期化
        /// </summary>
        /// <param name="rule">モデルとなるルールインスタンス</param>
        /// <param name="editCommand">対象のルールを編集するコマンド</param>
        /// <param name="removeCommand">対象のルールを一覧から削除するコマンド</param>
        public RuleItemViewModel(
            IRule rule,
            ICommand editCommand,
            ICommand removeCommand)
        {
            Rule = rule;
            EditCommand = editCommand;
            RemoveCommand = removeCommand;
        }
    }
}
