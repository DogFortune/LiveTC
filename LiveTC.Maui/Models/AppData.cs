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
    ///     ディスプレイタイマー更新
    /// </summary>
    /// <param name="elapsedTime"></param>
    public void UpdateDisplayTimeCode(TimeSpan elapsedTime)
    {
        ElapsedTime = elapsedTime;
        DisplayTimeCode =
            $"{ElapsedTime.Hours:00}:{ElapsedTime.Minutes:00}:{ElapsedTime.Seconds:00}:{ElapsedTime.Milliseconds / 10:00}";
    }

    public void AddChapter()
    {
        ChapterList.Add(new Chapter(ChapterList.Count + 1, new TimeSpan(0, 1, 10)));
        ChapterList.Add(new Chapter(ChapterList.Count + 1, new TimeSpan(0, 4, 50)));
    }

    public void RemoveChapter(Chapter chapter)
    {
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

    /// <summary>
    ///     チャプターリスト
    /// </summary>
    public ObservableCollection<Chapter> ChapterList { get; set; }
}