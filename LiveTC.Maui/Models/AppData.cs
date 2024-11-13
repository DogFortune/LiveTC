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
    }

    public void AddChapter()
    {
        ChapterList.Add(new Chapter(ChapterList.Count + 1, new TimeSpan(0, 1, 10)));
        ChapterList.Add(new Chapter(ChapterList.Count + 1, new TimeSpan(0, 4, 50)));

    }

    public void RemoveChapter(Chapter chapter)
    {
    }

    public ObservableCollection<Chapter> ChapterList { get; set; }
}