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
        public string Name => Rule.Name;

        /// <summary>
        /// このルールの説明文
        /// </summary>
        public string Description => Rule.Description;

        /// <summary>
        /// <see cref="RuleItemViewModel"/>インスタンスの生成と初期化
        /// </summary>
        /// <param name="rule">モデルとなるルールインスタンス</param>
        public RuleItemViewModel(
            IRule rule)
        {
            Rule = rule;
        }
    }
}
