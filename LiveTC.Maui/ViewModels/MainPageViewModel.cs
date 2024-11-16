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
        ElapsedTime = new TimeSpan();
        DisplayTimeCode = new ReactivePropertySlim<string>("00:00:00:00");
        ChapterList = model.ChapterList.ToReadOnlyReactiveCollection().AddTo(CompositeDisposable);

        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(10);
        timer.Tick += (s, e) =>
        {
            var timeSpan = ElapsedTime.Add(TimeSpan.FromMilliseconds(10));
            UpdateDisplayTimeCode(timeSpan);
        };

        StartTimeCode = new ReactiveCommand();
        StartTimeCode.Subscribe(_ =>
        {
            if (timer.IsRunning) return;
            timer.Start();
        });

        StopTimeCode = new ReactiveCommand();
        StopTimeCode.Subscribe(_ =>
        {
            if (!timer.IsRunning) return;
            timer.Stop();
        });
        
        ResetTimeCode = new ReactiveCommand();
        ResetTimeCode.Subscribe(_ =>
        {
            if (timer.IsRunning) return;
            UpdateDisplayTimeCode(TimeSpan.Zero);
        });

        IncrementHour = new ReactiveCommand();
        IncrementHour.Subscribe(_ =>
        {
            if (timer.IsRunning) return;
            var timeSpan = ElapsedTime.Add(TimeSpan.FromSeconds(1));
            UpdateDisplayTimeCode(timeSpan);
        });
    }

    /// <summary>
    ///     ディスプレイタイマー更新
    /// </summary>
    /// <param name="elapsedTime"></param>
    private void UpdateDisplayTimeCode(TimeSpan elapsedTime)
    {
        ElapsedTime = elapsedTime;
        DisplayTimeCode.Value =
            $"{ElapsedTime.Hours:00}:{ElapsedTime.Minutes:00}:{ElapsedTime.Seconds:00}:{ElapsedTime.Milliseconds / 10:00}";
    }

    /// <summary>
    ///     モデル
    /// </summary>
    private AppData Model { get; }

    /// <summary>
    ///     表示用タイム
    /// </summary>
    private TimeSpan ElapsedTime { get; set; }

    /// <summary>
    ///     スタート
    /// </summary>
    public ReactiveCommand StartTimeCode { get; }

    /// <summary>
    ///     ストップ
    /// </summary>
    public ReactiveCommand StopTimeCode { get; }
    
    /// <summary>
    ///     リセット
    /// </summary>
    public ReactiveCommand ResetTimeCode { get; }

    /// <summary>
    ///     総合タイム
    /// </summary>
    public ReactivePropertySlim<string> DisplayTimeCode { get; set; }

    /// <summary>
    ///     加算：時
    /// </summary>
    public ReactiveCommand IncrementHour { get; }

    /// <summary>
    ///     減算：時
    /// </summary>
    public ReactiveCommand DecrementHour { get; }

    /// <summary>
    ///     チャプターリスト
    /// </summary>
    public ReadOnlyReactiveCollection<Chapter> ChapterList { get; }
}