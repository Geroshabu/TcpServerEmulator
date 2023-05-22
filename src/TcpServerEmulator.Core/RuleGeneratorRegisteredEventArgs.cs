using TcpServerEmulator.Rules;

namespace TcpServerEmulator.Core
{
    /// <summary>
    /// ルールジェネレータが新しく登録されたときのイベントデータ
    /// </summary>
    public class RuleGeneratorRegisteredEventArgs : EventArgs
    {
        /// <summary>
        /// 新しく登録されたルールジェネレータ
        /// </summary>
        public IRuleGenerator NewRuleGenerator { get; }

        /// <summary>
        /// <see cref="RuleGeneratorRegisteredEventArgs"/>インスタンスの初期化と生成
        /// </summary>
        /// <param name="newRuleGenerator">新しく登録されたルールジェネレータ</param>
        public RuleGeneratorRegisteredEventArgs(IRuleGenerator newRuleGenerator)
        {
            NewRuleGenerator = newRuleGenerator;
        }
    }
}
