using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using TcpServerEmulator.Core;
using TcpServerEmulator.Core.Project;
using TcpServerEmulator.Core.Server;
using TcpServerEmulator.MainWindow.Commands;
using TcpServerEmulator.Rules;

namespace TcpServerEmulator.MainWindow
{
    internal class ViewModel : BindableBase
    {
        private readonly RulePluginHolder ruleGeneratorHolder;
        private readonly ProjectHolder projectHolder;
        private readonly Project project;
        private readonly TcpServer server;
        private readonly Logger.OnMemory.Logger logger;

        /// <summary>プロジェクトを開くコマンド</summary>
        public ICommand OpenProjectCommand { get; }

        public ICommand SaveAsNewFileCommand { get; }

        public ICommand ConnectCommand { get; }

        public ICommand DisconnectCommand { get; }

        public ICommand AddRuleCommand { get; }

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
            ProjectHolder projectHolder,
            RuleCollection ruleCollection,
            TcpServer server,
            Logger.OnMemory.Logger logger,
            ConnectCommand connectCommand,
            DisconnectCommand disconnectCommand,
            OpenProjectCommand openProjectCommand,
            SaveAsNewFileCommand saveAsNewFileCommand,
            AddRuleCommand addRuleCommand,
            IDialogService dialogService)
        {
            this.ruleGeneratorHolder = ruleGeneratorHolder;
            this.projectHolder = projectHolder;
            project = projectHolder.Current;
            this.server = server;
            this.logger = logger;
            ConnectCommand = connectCommand;
            DisconnectCommand = disconnectCommand;
            OpenProjectCommand = openProjectCommand;
            SaveAsNewFileCommand = saveAsNewFileCommand;
            AddRuleCommand = addRuleCommand;

            RulePlugins = new ObservableCollection<IRulePlugin>(ruleGeneratorHolder.Plugins);
            RuleItems = new ObservableCollection<RuleItemViewModel>(
                ruleCollection.Select(rule => createViewModel(rule, ruleCollection, dialogService)));
            SelectedRulePlugin = RulePlugins.FirstOrDefault();

            this.ruleGeneratorHolder.Registered += handlePluginRegistered;
            project.RuleCollection.RuleAdded += (_, e) => RuleItems.Add(createViewModel(e.NewRule, ruleCollection, dialogService));
            project.RuleCollection.RuleReplaced += (_, e) => RuleItems[e.Index] = createViewModel(e.NewRule, ruleCollection, dialogService);
            project.RuleCollection.RuleRemoved += (_, e) => RuleItems.Remove(RuleItems.First(item => item.Rule == e.RemovedRule));
            this.logger.MessageAdded += (_, _) => RaisePropertyChanged(nameof(CommunicationHistory));
        }

        private RuleItemViewModel createViewModel(IRule rule, RuleCollection ruleCollection, IDialogService dialogService)
        {
            return new RuleItemViewModel(
                rule,
                new EditRuleCommand(dialogService, ruleCollection, rule),
                new RemoveRuleCommand(ruleCollection, rule));
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
