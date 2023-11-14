using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Prism.Services.Dialogs;
using TcpServerEmulator.Core;
using TcpServerEmulator.Core.Project;
using TcpServerEmulator.Core.Server;
using TcpServerEmulator.MainWindow.Commands;
using TcpServerEmulator.Rules;
using TcpServerEmulator.UI.Common.Wpf;

namespace TcpServerEmulator.MainWindow
{
    internal class ViewModel : ValidatableBindableBase, INotifyDataErrorInfo
    {
        private readonly RulePluginHolder ruleGeneratorHolder;
        private readonly ProjectHolder projectHolder;
        private readonly TcpServer server;
        private readonly Logger.OnMemory.Logger logger;
        private readonly IDialogService dialogService;

        /// <summary>プロジェクトを開くコマンド</summary>
        public ICommand OpenProjectCommand { get; }

        public ICommand SaveAsNewFileCommand { get; }

        public ICommand ConnectCommand { get; }

        public ICommand DisconnectCommand { get; }

        public ICommand AddRuleCommand { get; }

        private PortNumber _port = new(0);
        private PortNumber port
        {
            get => _port;
            set
            {
                _port = value;
                server.Port = _port;
            }
        }

        private string portText = string.Empty;
        /// <summary>
        /// 使用するポート番号
        /// </summary>
        public string Port
        {
            get => portText;
            set
            {

                if (SetProperty(ref portText, value))
                {
                    _ = SetValidatedProperty(ref _port, value, PortNumber.GetFactory());
                }
            }
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
            this.server = server;
            this.logger = logger;
            this.dialogService = dialogService;
            ConnectCommand = connectCommand;
            DisconnectCommand = disconnectCommand;
            OpenProjectCommand = openProjectCommand;
            SaveAsNewFileCommand = saveAsNewFileCommand;
            AddRuleCommand = addRuleCommand;

            RulePlugins = new ObservableCollection<IRulePlugin>(ruleGeneratorHolder.Plugins);
            RuleItems = new ObservableCollection<RuleItemViewModel>();
            SelectedRulePlugin = RulePlugins.FirstOrDefault();

            this.ruleGeneratorHolder.Registered += handlePluginRegistered;
            this.projectHolder.CurrentProjectChanged += handleProjectChanged;
            subscribeProjectEvents(projectHolder.Current);
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

        private void handleProjectChanged(object? sender, CurrentProjectChangedEventArgs e)
        {
            unsubscribeProjectEvents(e.OldProject);
            resetRuleList(e.NewProject.RuleCollection);
            subscribeProjectEvents(e.NewProject);
        }

        private void subscribeProjectEvents(Project project)
        {
            project.RuleCollection.RuleAdded += handleRuleAdded;
            project.RuleCollection.RuleReplaced += handleRuleReplaced;
            project.RuleCollection.RuleRemoved -= handleRuleRemoved;
        }

        private void unsubscribeProjectEvents(Project project)
        {
            project.RuleCollection.RuleAdded -= handleRuleAdded;
            project.RuleCollection.RuleReplaced -= handleRuleReplaced;
            project.RuleCollection.RuleRemoved -= handleRuleRemoved;
        }

        private void resetRuleList(RuleCollection rules)
        {
            SelectedRuleIndex = -1;
            RuleItems.Clear();
            foreach (var rule in rules)
            {
                RuleItems.Add(createViewModel(rule, rules, dialogService));
            }
        }

        private void handleRuleAdded(object? sender, RuleAddedEventArgs e)
            => RuleItems.Add(createViewModel(e.NewRule, projectHolder.Current.RuleCollection, dialogService));

        private void handleRuleReplaced(object? sender, RuleReplacedEventArgs e)
            => RuleItems[e.Index] = createViewModel(e.NewRule, projectHolder.Current.RuleCollection, dialogService);

        private void handleRuleRemoved(object? sender, RuleRemovedEventArgs e)
            => RuleItems.Remove(RuleItems.First(item => item.Rule == e.RemovedRule));
    }
}
