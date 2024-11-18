using System.Collections.ObjectModel;
using System.Diagnostics;
using LiveTC.Maui.Models.Chapters;
using Prism.Mvvm;

namespace LiveTC.Maui.Models;

public class AppData : BindableBase
{
    public AppData()
    {
        ChapterList = new ObservableCollection<Chapter>();
        AddChapter();

        DisplayTimeCode = "00:00:00:00";
        ElapsedTime = new TimeSpan();
    }

    /// <summary>
    ///     本編TCのリセット
    /// </summary>
    public void ResetElapsedTime()
    {
        UpdateDisplayTimeCode(TimeSpan.Zero);
    }

    /// <summary>
    ///     タイムコードのインクリメント
    /// </summary>
    /// <param name="time">インクリメント値</param>
    public void IncrementElapsedTime(TimeSpan time)
    {
        var timeSpan = ElapsedTime.Add(time);
        UpdateDisplayTimeCode(timeSpan);
    }

    /// <summary>
    ///     タイムコードのデクリメント
    /// </summary>
    /// <param name="time">デクリメント値。マイナスになる場合は更新されない。</param>
    public void DecrementElapsedTime(TimeSpan time)
    {
        var timeSpan = ElapsedTime.Subtract(time);
        if (timeSpan.TotalMilliseconds < 0) return;
        UpdateDisplayTimeCode(timeSpan);
    }

    /// <summary>
    ///     ディスプレイタイマー更新
    /// </summary>
    /// <param name="elapsedTime">更新された値のTimeSpan。ここで指定した値になります。</param>
    private void UpdateDisplayTimeCode(TimeSpan elapsedTime)
    {
        ElapsedTime = elapsedTime;
        DisplayTimeCode =
            $"{ElapsedTime.Hours:00}:{ElapsedTime.Minutes:00}:{ElapsedTime.Seconds:00}:{ElapsedTime.Milliseconds / 10:00}";
    }

    public void AddChapter()
    {
        ChapterList.Add(new Chapter(ChapterList.Count + 1, TimeSpan.Zero));
        ChapterList.Add(new Chapter(ChapterList.Count + 1, TimeSpan.Zero));
        ChapterList.Add(new Chapter(ChapterList.Count + 1, TimeSpan.Zero));
        ChapterList.Add(new Chapter(ChapterList.Count + 1, TimeSpan.Zero));
        ChapterList.Add(new Chapter(ChapterList.Count + 1, TimeSpan.Zero));
    }

    public void RemoveChapter()
    {
        ChapterList.Remove(ChapterList[ChapterList.IndexOf(SelectedChapter)]);
    }

    private string _displayTimeCode;

    /// <summary>
    ///     統合タイム
    /// </summary>
    public string DisplayTimeCode
    {
        get => _displayTimeCode;
        set => SetProperty(ref _displayTimeCode, value);
    }

    private TimeSpan _elapsedTime;

    public TimeSpan ElapsedTime
    {
        get => _elapsedTime;
        set => SetProperty(ref _elapsedTime, value);
    }

    private Chapter _selectedChapter;

    /// <summary>
    ///     選択中のチャプター
    /// </summary>
    public Chapter SelectedChapter
    {
        get => _selectedChapter;
        set => SetProperty(ref _selectedChapter, value);
    }

    /// <summary>
    ///     チャプターリスト
    /// </summary>
    public ObservableCollection<Chapter> ChapterList { get; set; }
}