using TcpServerEmulator.Rules.ReturnConstant.DetailWindow;

namespace TcpServerEmulator.Rules.ReturnConstant
{
    /// <summary>
    /// 定数を返すルールのプラグイン
    /// </summary>
    internal class Plugin : IRulePlugin
    {
        /// <inheritdoc cref="IRulePlugin.Name"/>
        public string Name => "定数返却";

        /// <inheritdoc cref="IRulePlugin.EditWindowName"/>
        public string EditWindowName => typeof(View).FullName!;

        /// <inheritdoc cref="IRulePlugin.CreateInitialRule"/>
        public IRule CreateInitialRule() => new Rule();
    }
}
