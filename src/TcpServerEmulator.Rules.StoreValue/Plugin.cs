using TcpServerEmulator.Rules.StoreValue.DetailWindow;

namespace TcpServerEmulator.Rules.StoreValue
{
    /// <summary>
    /// 得た値を保持し、後で返却可能なルールの生成者
    /// </summary>
    internal class Plugin : IRulePlugin
    {
        /// <summary>このプラグインを識別するためのID</summary>
        public static Guid Id { get; } = new("d186ea6b-5d95-441c-b38d-dc9cd7f0c703");

        /// <inheritdoc cref="IRulePlugin.Name"/>
        public string Name => "値の保持";

        /// <inheritdoc cref="IRulePlugin.EditWindowName"/>
        public string EditWindowName => typeof(View).FullName!;

        /// <inheritdoc cref="IRulePlugin.CreateInitialRule"/>
        public IEditableRule CreateInitialRule() => new Rule();
    }
}
