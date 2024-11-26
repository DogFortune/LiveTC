using System.Reactive.Disposables;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LiveTC.Maui.ViewModels
{
    /// ViewModelの基底クラス
    public abstract class ViewModelBase : ObservableObject, IDisposable
    {
        /// ReactiveProperty一括破棄用フィールド
        protected CompositeDisposable CompositeDisposable = new();

        /// 二重破棄防止用フラグ
        private bool _disposed;

        /// リソースの破棄処理
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// リソースの破棄処理の実態
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                /* マネージリソースの破棄処理 */
                CompositeDisposable?.Dispose();
            }

            try
            {
                /* アンマネージドリソースの破棄処理 */
            }
            catch
            {
            }

            _disposed = true;
        }

        /// ファイナライザ
        ~ViewModelBase() => Dispose(false);

        /// ViewModel破棄
        public void Destroy() => Dispose();
    }
}