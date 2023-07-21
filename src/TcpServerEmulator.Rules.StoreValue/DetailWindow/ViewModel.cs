using System.Diagnostics.CodeAnalysis;
using Prism.Mvvm;
using Prism.Regions;

namespace TcpServerEmulator.Rules.StoreValue.DetailWindow
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

        /// <summary>ユーザーが入力した「設定コマンドで受け取るデータ」</summary>
        /// <exception cref="InvalidOperationException">
        ///   この ViewModel が操作対象となるモデルを持っていない場合に設定しようとしたとき。
        ///   <seealso cref="HasModel"/>
        /// </exception>
        public string SetterReceiveDataText
        {
            get => model?.SetterReceiveDataText ?? string.Empty;
            set
            {
                assertHasModel();
                model.SetterReceiveDataText = value;
            }
        }

        /// <summary>ユーザーが入力した「設定コマンドで返却するデータ」</summary>
        /// <exception cref="InvalidOperationException">
        ///   この ViewModel が操作対象となるモデルを持っていない場合に設定しようとしたとき。
        ///   <seealso cref="HasModel"/>
        /// </exception>
        public string SetterResponseDataText
        {
            get => model?.SetterResponseDataText ?? string.Empty;
            set
            {
                assertHasModel();
                model.SetterResponseDataText = value;
            }
        }

        /// <summary>ユーザーが入力した「取得コマンドで受け取るデータ」</summary>
        /// <exception cref="InvalidOperationException">
        ///   この ViewModel が操作対象となるモデルを持っていない場合に設定しようとしたとき。
        ///   <seealso cref="HasModel"/>
        /// </exception>
        public string GetterReceiveDataText
        {
            get => model?.GetterReceiveDataText ?? string.Empty;
            set
            {
                assertHasModel();
                model.GetterReceiveDataText = value;
            }
        }

        /// <summary>ユーザーが入力した「取得コマンドで返却するデータ」</summary>
        /// <exception cref="InvalidOperationException">
        ///   この ViewModel が操作対象となるモデルを持っていない場合に設定しようとしたとき。
        ///   <seealso cref="HasModel"/>
        /// </exception>
        public string GetterResponseDataText
        {
            get => model?.GetterResponseDataText ?? string.Empty;
            set
            {
                assertHasModel();
                model.GetterResponseDataText = value;
            }
        }

        /// <summary>ユーザーが入力した「保持する値の初期値」</summary>
        /// <exception cref="InvalidOperationException">
        ///   この ViewModel が操作対象となるモデルを持っていない場合に設定しようとしたとき。
        ///   <seealso cref="HasModel"/>
        /// </exception>
        public string InitialValuesText
        {
            get => model?.InitialValuesText ?? string.Empty;
            set
            {
                assertHasModel();
                model.InitialValuesText = value;
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
