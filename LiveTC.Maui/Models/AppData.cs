using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveTC.Maui.Models.Chapters;

namespace LiveTC.Maui.Models;

public class AppData : ObservableObject
{
    public AppData()
    {
        ChapterList = new ObservableCollection<Chapter>();
        AddChapter();

        SelectedChapter = ChapterList.FirstOrDefault() ?? throw new InvalidOperationException();

        DisplayTimeCode = "00:00:00:00";
        DisplaySelectedChapterTimeCode = "00:00:00:00";
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
    ///     チャプターTCのインクリメント
    /// </summary>
    /// <param name="time"></param>
    public void IncrementChapterTime(TimeSpan time)
    {
        var timeSpan = SelectedChapter.TimeCode.Add(time);
        UpdateSelectedChapterTimeCode(timeSpan);
    }

    /// <summary>
    ///     チャプターTCのデクリメント
    /// </summary>
    /// <param name="time">デクリメント値。マイナスになる場合は更新されない。</param>
    public void DecrementChapterTime(TimeSpan time)
    {
        var timeSpan = SelectedChapter.TimeCode.Add(time);
        if (timeSpan.TotalMilliseconds < 0) return;
        UpdateSelectedChapterTimeCode(timeSpan);
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

    /// <summary>
    ///     ディスプレイタイマー更新
    /// </summary>
    /// <param name="chapterTime">更新された値のTimeSpan。ここで指定した値になります。</param>
    private void UpdateSelectedChapterTimeCode(TimeSpan chapterTime)
    {
        SelectedChapter.TimeCode = chapterTime;
        DisplaySelectedChapterTimeCode =
            $"{SelectedChapter.TimeCode.Hours:00}:{SelectedChapter.TimeCode.Minutes:00}:{SelectedChapter.TimeCode.Seconds:00}:{SelectedChapter.TimeCode.Milliseconds / 10:00}";
    }

    /// <summary>
    ///     チャプターの追加
    /// </summary>
    public void AddChapter()
    {
        ChapterList.Add(new Chapter(ChapterList.Count + 1, TimeSpan.Zero));
        SelectedChapter = ChapterList.Last();
    }

    /// <summary>
    ///     チャプター削除
    /// </summary>
    public void RemoveChapter()
    {
        if (ChapterList.Count == 1) return;
        ChapterList.Remove(ChapterList[ChapterList.IndexOf(SelectedChapter)]);
        SelectedChapter = ChapterList.Last();
    }

    private string _displayTimeCode;

    /// <summary>
    ///     本編TCの表示用
    /// </summary>
    public string DisplayTimeCode
    {
        get => _displayTimeCode;
        set => SetProperty(ref _displayTimeCode, value);
    }

    private string _displaySelectedChapterTimeCode;

    /// <summary>
    ///     選択中のチャプターTC
    /// </summary>
    public string DisplaySelectedChapterTimeCode
    {
        get => _displaySelectedChapterTimeCode;
        set => SetProperty(ref _displaySelectedChapterTimeCode, value);
    }

    private TimeSpan _elapsedTime;

    /// <summary>
    ///     本編TC
    /// </summary>
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
        set
        {
            SetProperty(ref _selectedChapter, value);
            UpdateSelectedChapterTimeCode(SelectedChapter.TimeCode);
        }
    }

    /// <summary>
    ///     チャプターリスト
    /// </summary>
    public ObservableCollection<Chapter> ChapterList { get; set; }
}