using Prism.Ioc;
using Prism.Modularity;
using TcpServerEmulator.Rules.ReturnConstant.DetailWindow;

namespace TcpServerEmulator.Rules.ReturnConstant
{
    public class ReturnConstantModule : IModule
    {
        /// <inheritdoc cref="IModule.OnInitialized(IContainerProvider)"/>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var ruleRegister = containerProvider.Resolve<IRulePluginRegister>();
            var rule = containerProvider.Resolve<Plugin>();
            ruleRegister.Register(rule);
        }

        /// <inheritdoc cref="IModule.RegisterTypes(IContainerRegistry)"/>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<Plugin>();

            containerRegistry.RegisterDialog<View, ViewModel>(typeof(View).FullName);
        }
    }
}
