using TcpServerEmulator.Rules;

namespace TcpServerEmulator.Core
{
    /// <summary>
    /// コレクション内のルールが新しいルールに入れ替わったときのイベントデータ
    /// </summary>
    public class RuleReplacedEventArgs
    {
        /// <summary>入れ替わったルールの一覧内での位置</summary>
        public int Index { get; }

        /// <summary>入れ替わる前のルール</summary>
        public IRule OldRule { get; }

        /// <summary>入れ替わった後のルール</summary>
        public IRule NewRule { get; }

        /// <summary>
        /// <see cref="RuleReplacedEventArgs"/>インスタンスの生成と初期化
        /// </summary>
        /// <param name="index">入れ替わったルールの一覧内での位置</param>
        /// <param name="oldRule">入れ替わる前のルール</param>
        /// <param name="newRule">入れ替わった後のルール</param>
        public RuleReplacedEventArgs(int index, IRule oldRule, IRule newRule)
        {
            Index = index;
            OldRule = oldRule;
            NewRule = newRule;
        }
    }
}
