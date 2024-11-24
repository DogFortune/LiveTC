using System.Diagnostics;
using System.Reactive.Linq;
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
        DisplayTimeCode = Model.ObserveProperty(m => m.DisplayTimeCode)
            .ToReadOnlyReactivePropertySlim()
            .AddTo(CompositeDisposable);
        ChapterList = Model.ChapterList.ToReadOnlyReactiveCollection().AddTo(CompositeDisposable);
        SelectedChapter = Model.ToReactivePropertySlimAsSynchronized(m => m.SelectedChapter).AddTo(CompositeDisposable);
        SelectedChapterTimeCode = Model.ObserveProperty(m => m.DisplaySelectedChapterTimeCode)
            .ToReadOnlyReactivePropertySlim()
            .AddTo(CompositeDisposable);

        MainTimer = Application.Current.Dispatcher.CreateTimer();
        MainTimer.Interval = TimeSpan.FromMilliseconds(10);
        MainTimer.Tick += (s, e) =>
        {
            Model.IncrementElapsedTime(TimeSpan.FromMilliseconds(10));
            Model.IncrementChapterTime(TimeSpan.FromMilliseconds(10));
        };

        InitButton();
    }

    /// <summary>
    ///     ボタンの初期化。主にイベントハンドラー登録。
    /// </summary>
    private void InitButton()
    {
        // TODO: ReactiveCommandの非活性化を使ってボタンを押せないようにすると便利。
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
            Model.ResetElapsedTime();
        });

        IncrementHour.Subscribe(_ =>
        {
            if (MainTimer.IsRunning) return;
            Model.IncrementElapsedTime(TimeSpan.FromHours(1));
        });

        DecrementHour.Subscribe(_ =>
        {
            if (MainTimer.IsRunning) return;
            Model.DecrementElapsedTime(TimeSpan.FromHours(1));
        });

        ChangeChapterMode.Subscribe(_ => { ShowChapterTimeCode.Value = !ShowChapterTimeCode.Value; });
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
    ///     チャプタータイム
    /// </summary>
    public ReadOnlyReactivePropertySlim<string?> SelectedChapterTimeCode { get; }

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

    /// <summary>
    ///     選択中のチャプター
    /// </summary>
    public ReactivePropertySlim<Chapter> SelectedChapter { get; }

    /// <summary>
    ///     チャプターTC切り替えコマンド
    /// </summary>
    public ReactiveCommand ChangeChapterMode { get; } = new();

    /// <summary>
    ///     チャプターTCの表示・非表示
    /// </summary>
    public ReactivePropertySlim<bool> ShowChapterTimeCode { get; } = new(true);
}