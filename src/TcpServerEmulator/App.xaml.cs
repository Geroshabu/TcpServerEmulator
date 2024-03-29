﻿using System.Windows;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using TcpServerEmulator.Core;
using TcpServerEmulator.Core.Perpetuation;
using TcpServerEmulator.Core.Project;
using TcpServerEmulator.Core.Server;
using TcpServerEmulator.Logger;
using TcpServerEmulator.Logger.OnMemory;
using TcpServerEmulator.MainWindow;
using TcpServerEmulator.MainWindow.Commands;
using TcpServerEmulator.Rules;

namespace TcpServerEmulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <inheritdoc cref="Prism.PrismApplicationBase.RegisterTypes(IContainerRegistry)"/>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<View>();
            containerRegistry.RegisterManySingleton<RulePluginHolder>(
                typeof(RulePluginHolder),
                typeof(IRulePluginRegister));
            containerRegistry.RegisterSingleton<ProjectHolder>();
            containerRegistry.Register<ConnectCommand>();
            containerRegistry.Register<DisconnectCommand>();
            containerRegistry.Register<AddRuleCommand>();
            containerRegistry.Register<OpenProjectCommand>();
            containerRegistry.Register<SaveAsNewFileCommand>();
            containerRegistry.RegisterSingleton<TcpServer>();
            containerRegistry.RegisterManySingleton<Logger.OnMemory.Logger>(
                typeof(Logger.OnMemory.Logger),
                typeof(ILogger));
            containerRegistry.RegisterMany<Perpetuator>(typeof(ISave), typeof(ILoad));

            containerRegistry.RegisterDialog<EditRuleWindow.View, EditRuleWindow.ViewModel>(typeof(EditRuleWindow.View).FullName);
        }

        /// <inheritdoc cref="Prism.PrismApplicationBase.CreateShell"/>
        protected override Window CreateShell()
        {
            return Container.Resolve<View>();
        }

        /// <inheritdoc cref="Prism.PrismApplicationBase.CreateModuleCatalog"/>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
        }
    }
}
