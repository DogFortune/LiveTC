using System.Diagnostics;
using LiveTC.Maui.Models;
using LiveTC.Maui.Models.Chapters;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace LiveTC.Maui.ViewModels;

public class MainPageViewModel : ViewModelBase
{
    public MainPageViewModel(AppData model)
    {
        Model = model;
        ElapsedTime = model.ToReactivePropertySlimAsSynchronized(m => m.ElapsedTime).AddTo(CompositeDisposable);
        DisplayTimeCode = model.ObserveProperty(m => m.DisplayTimeCode)
            .ToReadOnlyReactivePropertySlim()
            .AddTo(CompositeDisposable);
        ChapterList = model.ChapterList.ToReadOnlyReactiveCollection().AddTo(CompositeDisposable);

        MainTimer = Application.Current.Dispatcher.CreateTimer();
        MainTimer.Interval = TimeSpan.FromMilliseconds(10);
        MainTimer.Tick += (s, e) =>
        {
            var timeSpan = ElapsedTime.Value.Add(TimeSpan.FromMilliseconds(10));
            Model.UpdateDisplayTimeCode(timeSpan);
        };

        InitButton();
    }

    /// <summary>
    ///     ボタンの初期化。主にイベントハンドラー登録。
    /// </summary>
    private void InitButton()
    {
        StartTimeCode.Subscribe(_ =>
        {
            if (MainTimer.IsRunning) return;
            MainTimer.Start();
        });

        StopTimeCode.Subscribe(_ =>
        {
            if (!MainTimer.IsRunning) return;
            MainTimer.Stop();
        });

        ResetTimeCode.Subscribe(_ =>
        {
            if (MainTimer.IsRunning) return;
            Model.UpdateDisplayTimeCode(TimeSpan.Zero);
        });

        IncrementHour.Subscribe(_ =>
        {
            if (MainTimer.IsRunning) return;
            var timeSpan = ElapsedTime.Value.Add(TimeSpan.FromSeconds(1));
            Model.UpdateDisplayTimeCode(timeSpan);
        });
    }

    /// <summary>
    ///     モデル
    /// </summary>
    private AppData Model { get; }

    /// <summary>
    ///     ストップウォッチ更新用タイマー
    /// </summary>
    private IDispatcherTimer MainTimer { get; }

    /// <summary>
    ///     表示用タイム
    /// </summary>
    private ReactivePropertySlim<TimeSpan> ElapsedTime { get; set; }

    /// <summary>
    ///     スタート
    /// </summary>
    public ReactiveCommand StartTimeCode { get; } = new();

    /// <summary>
    ///     ストップ
    /// </summary>
    public ReactiveCommand StopTimeCode { get; } = new();

    /// <summary>
    ///     リセット
    /// </summary>
    public ReactiveCommand ResetTimeCode { get; } = new();

    /// <summary>
    ///     総合タイム
    /// </summary>
    public ReadOnlyReactivePropertySlim<string?> DisplayTimeCode { get; }

    /// <summary>
    ///     加算：時
    /// </summary>
    public ReactiveCommand IncrementHour { get; } = new();

    /// <summary>
    ///     減算：時
    /// </summary>
    public ReactiveCommand DecrementHour { get; } = new();

    /// <summary>
    ///     チャプターリスト
    /// </summary>
    public ReadOnlyReactiveCollection<Chapter> ChapterList { get; }
}