namespace TcpServerEmulator.Rules
{
    /// <summary>
    /// ルールのプラグインを登録する機能を表す
    /// </summary>
    public interface IRulePluginRegister
    {
        /// <summary>
        /// プラグインを登録する
        /// </summary>
        /// <param name="rulePlugin">登録するルールプラグイン</param>
        void Register(IRulePlugin rulePlugin);
    }
}
