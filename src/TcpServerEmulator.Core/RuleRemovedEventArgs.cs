using TcpServerEmulator.Rules;

namespace TcpServerEmulator.Core
{
    /// <summary>
    /// ルール一覧からルールが削除されたときのイベントデータ
    /// </summary>
    public class RuleRemovedEventArgs
    {
        /// <summary>
        /// 削除されたルール
        /// </summary>
        public IRule RemovedRule { get; }

        /// <summary>
        /// <see cref="RuleRemovedEventArgs"/>インスタンスの生成と初期化
        /// </summary>
        /// <param name="removedRule">削除されたルール</param>
        public RuleRemovedEventArgs(IRule removedRule)
        {
            RemovedRule = removedRule;
        }
    }
}
