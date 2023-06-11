using TcpServerEmulator.Rules.StoreValue.DetailWindow;

namespace TcpServerEmulator.Rules.StoreValue
{
    /// <summary>
    /// 得た値を保持し、後で返却可能なルールの生成者
    /// </summary>
    internal class RuleGenerator : IRuleGenerator
    {
        /// <inheritdoc cref="IRuleGenerator.Name"/>
        public string Name => "値の保持";

        /// <inheritdoc cref="IRuleGenerator.EditWindowName"/>
        public string EditWindowName => typeof(View).FullName!;
    }
}
