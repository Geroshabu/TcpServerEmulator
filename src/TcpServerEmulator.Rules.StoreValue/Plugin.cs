using TcpServerEmulator.Rules.StoreValue.DetailWindow;

namespace TcpServerEmulator.Rules.StoreValue
{
    /// <summary>
    /// 得た値を保持し、後で返却可能なルールの生成者
    /// </summary>
    internal class Plugin : IRulePlugin
    {
        /// <inheritdoc cref="IRulePlugin.Name"/>
        public string Name => "値の保持";

        /// <inheritdoc cref="IRulePlugin.EditWindowName"/>
        public string EditWindowName => typeof(View).FullName!;
    }
}
