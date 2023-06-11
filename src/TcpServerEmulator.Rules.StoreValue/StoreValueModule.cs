using Prism.Ioc;
using Prism.Modularity;
using TcpServerEmulator.Rules.StoreValue.DetailWindow;

namespace TcpServerEmulator.Rules.StoreValue
{
    public class StoreValueModule : IModule
    {
        /// <inheritdoc cref="IModule.OnInitialized(IContainerProvider)"/>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var ruleRegister = containerProvider.Resolve<IRuleGeneratorRegister>();
            var rule = containerProvider.Resolve<RuleGenerator>();
            ruleRegister.Register(rule);
        }

        /// <inheritdoc cref="IModule.RegisterTypes(IContainerRegistry)"/>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<RuleGenerator>();

            containerRegistry.RegisterDialog<View, ViewModel>(typeof(View).FullName);
        }
    }
}