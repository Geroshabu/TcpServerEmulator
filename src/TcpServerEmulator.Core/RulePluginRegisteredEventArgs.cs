using TcpServerEmulator.Rules;

namespace TcpServerEmulator.Core
{
    /// <summary>
    /// ルールのプラグインが新しく登録されたときのイベントデータ
    /// </summary>
    public class RulePluginRegisteredEventArgs : EventArgs
    {
        /// <summary>
        /// 新しく登録されたプラグイン
        /// </summary>
        public IRulePlugin NewRulePlugin { get; }

        /// <summary>
        /// <see cref="RulePluginRegisteredEventArgs"/>インスタンスの初期化と生成
        /// </summary>
        /// <param name="newRulePlugin">新しく登録されたルールのプラグイン</param>
        public RulePluginRegisteredEventArgs(IRulePlugin newRulePlugin)
        {
            NewRulePlugin = newRulePlugin;
        }
    }
}
