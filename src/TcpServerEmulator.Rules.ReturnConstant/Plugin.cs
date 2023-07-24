using TcpServerEmulator.Rules.ReturnConstant.DetailWindow;

namespace TcpServerEmulator.Rules.ReturnConstant
{
    /// <summary>
    /// 定数を返すルールのプラグイン
    /// </summary>
    internal class Plugin : IRulePlugin
    {
        /// <summary>このプラグインを識別するためのID</summary>
        public static Guid Id { get; } = new("57c1d6a8-3e28-41f5-b5aa-f392061c3f3c");

        /// <inheritdoc cref="IRulePlugin.Name"/>
        public string Name => "定数返却";

        /// <inheritdoc cref="IRulePlugin.EditWindowName"/>
        public string EditWindowName => typeof(View).FullName!;

        /// <inheritdoc cref="IRulePlugin.CreateInitialRule"/>
        public IEditableRule CreateInitialRule() => new Rule();
    }
}
