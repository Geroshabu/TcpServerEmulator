using System.Diagnostics.CodeAnalysis;
using Prism.Mvvm;
using Prism.Regions;

namespace TcpServerEmulator.Rules.ReturnConstant.DetailWindow
{
    internal class ViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        [DisallowNull]
        private Rule? model { get; set; }

        /// <summary>この ViewModel が、操作の対象となるモデルを持っているか否か</summary>
        [MemberNotNullWhen(true, nameof(model))]
        public bool HasModel => model != null;

        [MemberNotNull(nameof(model))]
        private void assertHasModel()
        {
            if (!HasModel)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>ユーザーが入力した「受け取るデータ」</summary>
        /// <exception cref="InvalidOperationException">
        ///   この ViewModel が操作対象となるモデルを持っていない場合に設定しようとしたとき。
        ///   <seealso cref="HasModel"/>
        /// </exception>
        public string ReceiveDataText
        {
            get => model?.ReceiveDataText ?? string.Empty;
            set
            {
                assertHasModel();
                model.ReceiveDataText = value;
            }
        }

        /// <summary>ユーザーが入力した「返却するデータ」</summary>
        /// <exception cref="InvalidOperationException">
        ///   この ViewModel が操作対象となるモデルを持っていない場合に設定しようとしたとき。
        ///   <seealso cref="HasModel"/>
        /// </exception>
        public string ResponseDataText
        {
            get => model?.ResponseDataText ?? string.Empty;
            set
            {
                assertHasModel();
                model.ResponseDataText = value;
            }
        }

        /// <inheritdoc cref="IRegionMemberLifetime.KeepAlive"/>
        public bool KeepAlive => false;

        /// <inheritdoc cref="INavigationAware.OnNavigatedTo(NavigationContext)"/>
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue(nameof(IEditableRule), out Rule rule))
            {
                model = rule;
                RaisePropertyChanged(string.Empty);
            }
        }

        /// <inheritdoc cref="INavigationAware.IsNavigationTarget(NavigationContext)"/>
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        /// <inheritdoc cref="INavigationAware.OnNavigatedFrom(NavigationContext)"/>
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
