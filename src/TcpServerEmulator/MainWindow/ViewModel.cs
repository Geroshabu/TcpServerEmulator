using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using TcpServerEmulator.Core;
using TcpServerEmulator.Core.Server;
using TcpServerEmulator.MainWindow.Commands;
using TcpServerEmulator.Rules;

namespace TcpServerEmulator.MainWindow
{
    internal class ViewModel : BindableBase
    {
        private readonly RulePluginHolder ruleGeneratorHolder;
        private readonly RuleHolder ruleHolder;
        private readonly TcpServer server;
        private readonly Logger.OnMemory.Logger logger;

        public ICommand ConnectCommand { get; }

        public ICommand DisconnectCommand { get; }

        public ICommand AddRuleCommand { get; }

        private DelegateCommand removeRuleCommand;
        public ICommand RemoveRuleCommand => removeRuleCommand ?? (removeRuleCommand = new DelegateCommand(() =>
        {
            if (SelectedRuleIndex < RuleItems.Count)
            {
                ruleHolder.RemoveRule(RuleItems[SelectedRuleIndex].Rule);
            }
        }));

        public int Port
        {
            get => server.Port;
            set => server.Port = value;
        }

        /// <summary>
        /// ルールのプラグインの選択肢
        /// </summary>
        public ObservableCollection<IRulePlugin> RulePlugins { get; }

        private IRulePlugin? selectedRulePlugin;
        /// <summary>
        /// 現在選択されているルールのプラグイン。
        /// まだルールプラグインが一つも読み込まれていないされていない場合はnull。
        /// </summary>
        public IRulePlugin? SelectedRulePlugin
        {
            get => selectedRulePlugin;
            set => SetProperty(ref selectedRulePlugin, value);
        }

        /// <summary>
        /// ルール一覧
        /// </summary>
        public ObservableCollection<RuleItemViewModel> RuleItems { get; }

        /// <summary>選択されているルールのインデックス</summary>
        public int SelectedRuleIndex { get; set; }

        /// <summary>やり取りの履歴</summary>
        public string CommunicationHistory => logger.JoinedMessage;

        public ViewModel(
            RulePluginHolder ruleGeneratorHolder,
            RuleHolder ruleHolder,
            TcpServer server,
            Logger.OnMemory.Logger logger,
            ConnectCommand connectCommand,
            DisconnectCommand disconnectCommand,
            AddRuleCommand addRuleCommand,
            RemoveRuleCommand removeRuleCommand,
            IDialogService dialogService)
        {
            this.ruleGeneratorHolder = ruleGeneratorHolder;
            this.ruleHolder = ruleHolder;
            this.server = server;
            this.logger = logger;
            ConnectCommand = connectCommand;
            DisconnectCommand = disconnectCommand;
            AddRuleCommand = addRuleCommand;
            //RemoveRuleCommand = removeRuleCommand;

            RulePlugins = new ObservableCollection<IRulePlugin>(ruleGeneratorHolder.Plugins);
            RuleItems = new ObservableCollection<RuleItemViewModel>(
                ruleHolder.Rules.Select(rule => createViewModel(rule, ruleHolder, dialogService)));
            SelectedRulePlugin = RulePlugins.FirstOrDefault();

            this.ruleGeneratorHolder.Registered += handlePluginRegistered;
            this.ruleHolder.RuleAdded += (_, e) => RuleItems.Add(createViewModel(e.NewRule, ruleHolder, dialogService));
            this.ruleHolder.RuleReplaced += (_, e) => RuleItems[e.Index] = createViewModel(e.NewRule, ruleHolder, dialogService);
            this.ruleHolder.RuleRemoved += (_, e) => RuleItems.Remove(RuleItems.First(item => item.Rule == e.RemovedRule));
            this.logger.MessageAdded += (_, _) => RaisePropertyChanged(nameof(CommunicationHistory));
        }

        private RuleItemViewModel createViewModel(IRule rule, RuleHolder ruleHolder, IDialogService dialogService)
        {
            return new RuleItemViewModel(rule, new EditRuleCommand(dialogService, ruleHolder, rule));
        }

        private void handlePluginRegistered(object? sender, RulePluginRegisteredEventArgs e)
        {
            RulePlugins.Add(e.NewRulePlugin);
            if (RulePlugins.Count == 1)
            {
                SelectedRulePlugin = RulePlugins[0];
            }
        }
    }
}
