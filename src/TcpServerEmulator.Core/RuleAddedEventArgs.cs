using TcpServerEmulator.Rules;

namespace TcpServerEmulator.Core
{
    /// <summary>
    /// ルール一覧に新しくルールが追加されたときのイベントデータ
    /// </summary>
    public class RuleAddedEventArgs
    {
        /// <summary>
        /// 新しく追加されたルール
        /// </summary>
        public IRule NewRule { get; }

        /// <summary>
        /// <see cref="RuleAddedEventArgs"/>インスタンスの生成と初期化
        /// </summary>
        /// <param name="rule">新しく追加されたルール</param>
        public RuleAddedEventArgs(IRule rule)
        {
            NewRule = rule;
        }
    }
}
