using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using TcpServerEmulator.Core;
using TcpServerEmulator.Core.Server;
using TcpServerEmulator.MainWindow.Commands;
using TcpServerEmulator.Rules;

namespace TcpServerEmulator.MainWindow
{
    internal class ViewModel : BindableBase
    {
        private readonly RuleGeneratorHolder ruleGeneratorHolder;
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
        /// ルールジェネレータの選択肢
        /// </summary>
        public ObservableCollection<IRuleGenerator> RuleGenerators { get; }

        private IRuleGenerator? selectedRuleGenerator;
        /// <summary>
        /// 現在選択されているルールジェネレータ。
        /// まだルールジェネレータが一つも読み込まれていないされていない場合はnull。
        /// </summary>
        public IRuleGenerator? SelectedRuleGenerator
        {
            get => selectedRuleGenerator;
            set => SetProperty(ref selectedRuleGenerator, value);
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
            RuleGeneratorHolder ruleGeneratorHolder,
            RuleHolder ruleHolder,
            TcpServer server,
            Logger.OnMemory.Logger logger,
            ConnectCommand connectCommand,
            DisconnectCommand disconnectCommand,
            AddRuleCommand addRuleCommand,
            RemoveRuleCommand removeRuleCommand)
        {
            this.ruleGeneratorHolder = ruleGeneratorHolder;
            this.ruleHolder = ruleHolder;
            this.server = server;
            this.logger = logger;
            ConnectCommand = connectCommand;
            DisconnectCommand = disconnectCommand;
            AddRuleCommand = addRuleCommand;
            //RemoveRuleCommand = removeRuleCommand;

            RuleGenerators = new ObservableCollection<IRuleGenerator>(ruleGeneratorHolder.Generators);
            RuleItems = new ObservableCollection<RuleItemViewModel>(
                ruleHolder.Rules.Select(rule => new RuleItemViewModel(rule)));
            SelectedRuleGenerator = RuleGenerators.FirstOrDefault();

            this.ruleGeneratorHolder.GeneratorRegistered += handleGeneratorRegistered;
            this.ruleHolder.RuleAdded += (_, e) => RuleItems.Add(new RuleItemViewModel(e.NewRule));
            this.ruleHolder.RuleRemoved += (_, e) => RuleItems.Remove(RuleItems.First(item => item.Rule == e.RemovedRule));
            this.logger.MessageAdded += (_, _) => RaisePropertyChanged(nameof(CommunicationHistory));
        }

        private void handleGeneratorRegistered(object? sender, RuleGeneratorRegisteredEventArgs e)
        {
            RuleGenerators.Add(e.NewRuleGenerator);
            if (RuleGenerators.Count == 1)
            {
                SelectedRuleGenerator = RuleGenerators[0];
            }
        }
    }
}
