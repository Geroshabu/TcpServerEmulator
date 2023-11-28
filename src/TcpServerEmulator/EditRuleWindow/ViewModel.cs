using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using TcpServerEmulator.Rules;
using TcpServerEmulator.UI.Common.Wpf;

namespace TcpServerEmulator.EditRuleWindow
{
    internal class ViewModel : ValidatableBindableBase, IDialogAware
    {
        public IRegionManager RegionManager { get; }

        private const string regionName = "EditRuleRegion";

        private IEditableRule? _model = null;
        [DisallowNull]
        private IEditableRule? model
        {
            get => _model;
            set
            {
                _model = value;
                RaisePropertyChanged(nameof(canExecuteOk));
            }
        }

        /// <summary>この ViewModel が、操作の対象となるモデルを持っているか否か</summary>
        [MemberNotNullWhen(true, nameof(model), nameof(_model))]
        public bool HasModel => model != null;

        [MemberNotNull(nameof(model), nameof(_model))]
        private void assertHasModel()
        {
            if (!HasModel)
            {
                throw new InvalidOperationException();
            }
        }

        /// <inheritdoc cref="IDialogAware.Title"/>
        public string Title => "ルール編集";

        private RuleName name
        {
            get => model?.Name ?? RuleName.Default;
            set
            {
                if (HasModel)
                {
                    model.Name = value;
                }
            }
        }

        private string nameText = string.Empty;
        /// <summary>ルールの名前</summary>
        public string NameText
        {
            get => nameText;
            set => SetPropertyWithValidate(ref nameText, value, () => name, RuleName.GetFactory());
        }

        private DelegateCommand? okCommand;
        /// <summary>OKボタンのコマンド</summary>
        public ICommand OkCommand =>
            okCommand ?? (okCommand = new DelegateCommand(() =>
            {
                assertHasModel();

                var dialogParameter = new DialogParameters
                {
                    { nameof(IRule), model.AsImmutableRule() }
                };
                RaiseRequestClose(new DialogResult(ButtonResult.OK, dialogParameter));
            })
            .ObservesCanExecute(() => canExecuteOk));

        private bool canExecuteOk => HasModel && model.IsValid && !HasErrors;
        private void handleIsValidChanged(object? sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(canExecuteOk));
        }

        private void handleErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(canExecuteOk));
        }

        private DelegateCommand? cancelCommand;
        /// <summary>キャンセルボタンのコマンド</summary>
        public ICommand CancelCommand =>
            cancelCommand ?? (cancelCommand = new DelegateCommand(() =>
            {
                RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
            }));

        /// <inheritdoc cref="IDialogAware.RequestClose"/>
        public event Action<IDialogResult>? RequestClose;

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public ViewModel(IRegionManager regionManager)
        {
            RegionManager = regionManager.CreateRegionManager();

            ErrorsChanged += handleErrorsChanged;
        }

        /// <inheritdoc cref="IDialogAware.CanCloseDialog"/>
        public bool CanCloseDialog() => true;

        /// <inheritdoc cref="IDialogAware.OnDialogClosed"/>
        public void OnDialogClosed()
        {
            if (HasModel)
            {
                model.IsValidChanged -= handleIsValidChanged;
            }
            RegionManager.Regions[regionName].RemoveAll();
            RegionManager.Regions.Remove(regionName);
        }

        /// <inheritdoc cref="IDialogAware.OnDialogOpened(IDialogParameters)"/>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue(nameof(IEditableRule), out IEditableRule rule) &&
                parameters.TryGetValue(nameof(RuleEditMode), out RuleEditMode mode))
            {
                model = rule;
                model.IsValidChanged += handleIsValidChanged;

                var navigationParameters = new NavigationParameters
                {
                    { nameof(IEditableRule), rule }
                };
                RegionManager.RequestNavigate(regionName, rule.Id.ToString(), navigationParameters);

                if (mode == RuleEditMode.Update)
                {
                    reflectModelToUi(model);
                }
            }
        }

        private void reflectModelToUi(IEditableRule model)
        {
            nameText = model.Name.Value;
            RaisePropertyChanged(nameof(NameText));
        }
    }
}
