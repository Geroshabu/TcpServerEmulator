using TcpServerEmulator.Rules;

namespace TcpServerEmulator.Core
{
    /// <summary>
    /// ルールのプラグインを保持しておくクラス
    /// </summary>
    public class RulePluginHolder : IRulePluginRegister
    {
        private readonly List<IRulePlugin> plugins = new();

        /// <summary>
        /// 保持しているプラグイン
        /// </summary>
        public IEnumerable<IRulePlugin> Plugins => plugins.AsReadOnly();

        /// <summary>
        /// 新しくプラグインが登録されたときに発生する
        /// </summary>
        public event EventHandler<RulePluginRegisteredEventArgs>? Registered;

        /// <inheritdoc cref="IRulePluginRegister.Register(IRulePlugin)"/>
        public void Register(IRulePlugin rulePlugin)
        {
            plugins.Add(rulePlugin);
            Registered?.Invoke(this, new RulePluginRegisteredEventArgs(rulePlugin));
        }
    }
}
