using CommunityToolkit.Mvvm.ComponentModel;

namespace LiveTC.Maui.Models.Chapters;

public class Chapter : ObservableObject
{
    public Chapter(int number, TimeSpan timeCode)
    {
        Number = number;
        TimeCode = timeCode;
    }

    /// <summary>
    ///     チャプター番号
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    ///     チャプター毎のタイムコード
    /// </summary>
    private TimeSpan _timeCode;

    public TimeSpan TimeCode
    {
        get => _timeCode;
        set => SetProperty(ref _timeCode, value);
    }
}