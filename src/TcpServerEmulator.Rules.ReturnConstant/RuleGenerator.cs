using Prism.Services.Dialogs;
using TcpServerEmulator.Rules.ReturnConstant.DetailWindow;

namespace TcpServerEmulator.Rules.ReturnConstant
{
    /// <summary>
    /// 定数を返すルールの生成者
    /// </summary>
    internal class RuleGenerator : IRuleGenerator
    {
        /// <inheritdoc cref="IRuleGenerator.Name"/>
        public string Name => "定数返却";

        /// <inheritdoc cref="IRuleGenerator.EditWindowName"/>
        public string EditWindowName => typeof(View).FullName!;

        public RuleGenerator()
        {
        }
    }
}
