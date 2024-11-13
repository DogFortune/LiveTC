namespace LiveTC.Maui.Models.Chapters;

public class Chapter
{
    public Chapter(int number)
    {
        Number = number;
        TimeCode = new TimeSpan(0);
    }
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
    public TimeSpan TimeCode { get; set; }
}