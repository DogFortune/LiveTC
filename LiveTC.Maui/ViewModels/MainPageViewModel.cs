using System.Diagnostics;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.TinyLinq;

namespace LiveTC.Maui.ViewModels;

using AppContext = LiveTC.Maui.Models.AppContext;

public class MainPageViewModel : ViewModelBase
{
    public MainPageViewModel(AppContext model)
    {
        Model = model;
        StopWatch = new Stopwatch();
        ElapsedTime = new TimeSpan();
        DisplayTimeCode = new ReactivePropertySlim<string>("00:00:00:00");

        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(10);
        timer.Tick += (s, e) =>
        {
            ElapsedTime = StopWatch.Elapsed;
            DisplayTimeCode.Value =
                $"{ElapsedTime.Hours:00}:{ElapsedTime.Minutes:00}:{ElapsedTime.Seconds:00}:{ElapsedTime.Milliseconds / 10:00}";
        };

        StartTimeCode = new ReactiveCommand();
        StartTimeCode.Subscribe(_ =>
        {
            if (IsRunning) return;
            StopWatch.Start();
            timer.Start();
            IsRunning = true;
        });

        StopTimeCode = new ReactiveCommand();
        StopTimeCode.Subscribe(_ =>
        {
            if (!IsRunning) return;
            StopWatch.Stop();
            timer.Stop();
            IsRunning = false;
        });
    }

    /// <summary>
    ///     モデル
    /// </summary>
    private AppContext Model { get; }

    private bool IsRunning { get; set; }

    /// <summary>
    ///     ストップウォッチ本体
    /// </summary>
    private Stopwatch StopWatch { get; }

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
    ///     総合タイム
    /// </summary>
    public ReactivePropertySlim<string> DisplayTimeCode { get; set; }
}