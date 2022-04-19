using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class WidgetManager : MonoBehaviour
{
    DateTime localDate;
    public int stepTime = 60;
    public float waitTime = 7.0f;
    public int alertTime = 60 * 60 * 24;
    public int daysUntilOld = 3;
    public GameObject Widget;
    private Transform T;
    public TextMeshPro displayTime;
    public TextMeshPro displayAMPM;
    public GameObject Moon;
    public TextMeshPro displayWeekday;
    public TextMeshPro displayDate;
    public TextMeshPro News;
    public TextMeshPro News_folder;

    private string news_S;
    private string news_folder_S;

    public TimeSpan dusk = new TimeSpan(19, 0, 0);
    public TimeSpan dawn = new TimeSpan(6, 0, 0);
    private TimeSpan hour;
    
    

    public void Start()
    {
        T = Widget.transform;
        StartCoroutine(TikTok());
        StartCoroutine(DailyCheckForNewFiles());
        
    }


    // Widget General Funtions
    public void HideWidget()
    {
        Widget.transform.DOMove(
            new Vector3(T.position.x, T.position.y + 9, T.position.z), 1).SetEase(
                Ease.InOutQuint);
    }

    public void ShowWidget()
    {
        Widget.transform.DOMove(
            new Vector3(T.position.x, T.position.y - 9, T.position.z), 1).SetEase(
                Ease.InOutQuint);
    }

    public void retardedShowWidget()
    {
        StartCoroutine(AuxRetardedShowWidget());
    }

    IEnumerator AuxRetardedShowWidget()
    {
        // Wait X seconds
        yield return new WaitForSeconds(waitTime);
        ShowWidget();   
    }

    // Clock display Funtion
    IEnumerator TikTok()
    {
        while(true)
        {
            // Update displayed time
            localDate = DateTime.Now;
            displayWeekday.DOText(localDate.ToString("dddd"), 0.3f);
            displayTime.DOText(localDate.ToString("HH:mm"), 0.3f);
            displayDate.DOText(localDate.ToString("dd"), 0.3f);
            displayAMPM.DOText(localDate.ToString("tt"), 0.3f);
            // Turn the moon on or off
            hour = localDate.TimeOfDay;
            if((dusk < hour) && (hour < dawn))
            {
                Moon.SetActive(true);
            }
            else
            {
                Moon.SetActive(false);
            }
            // Let the time pass
            yield return new WaitForSeconds(stepTime);
        }
    }

    // Alert
    IEnumerator DailyCheckForNewFiles()
    {
        while(true)
        {
            // Checks for the nevest file Folder
            // If older than 3 days displays one message
            // If newer then 3 days displays another message
            // Both reference the new file Folder
            checkFileDates();
            News.DOText(news_S, 1.0f);
            News_folder.DOText(news_folder_S, 1.0f);
            yield return new WaitForSeconds(alertTime);
        }
    }

    // Read files funtion
    public void checkFileDates()
    {
        // print("DEBUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUG");
        string StreamingAssetsRoot = Application.streamingAssetsPath;
        string multimedia = StreamingAssetsRoot + "/multimedia";
        //string multimedia = StreamingAssetsRoot + "/banners"; Banners should be moved into multimedia

        // Make a Directory Info instance to look for newest file inside
        System.IO.DirectoryInfo rootDir = new System.IO.DirectoryInfo(multimedia);
        System.IO.FileInfo newestFile = WalkDirectoryTree(rootDir);
        // print(WalkDirectoryTree(rootDir));
        // print(newestFile);
        // print(newestFile.Name);
        // print(newestFile.Directory);
        // print(newestFile.Directory.Name);
        // Check if the file is older than X days
        print(DateTime.Now.AddDays(- daysUntilOld));
        
        // See if the newest file is older than 3 days
        int result = DateTime.Compare(DateTime.Now.AddDays(- daysUntilOld), newestFile.LastWriteTime);
        if(result <= 0)
        {
            news_S = "Alerta de nuevo contenido en";
        }
        else
        {
            news_S = "Revisa lo último en";
        }

        // Returns the name of the Directory of the newest added file in multimedia
        news_folder_S = newestFile.Directory.Name;


    }

    // Docs example
    // Walks down the tree from the root directory given to the function
    // Returns newest fileInfo
    static System.IO.FileInfo WalkDirectoryTree(System.IO.DirectoryInfo root)
    {
        // Get the files inside the root directory
        System.IO.FileInfo[] files = null;
        files = root.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
        // Order and return a generator with the ordered files by creation date
        IOrderedEnumerable<System.IO.FileInfo> orderedFiles = files.OrderByDescending(file => file.CreationTime);
        // Return just the newest file
        // Without return it will iterate over all files, from newest to latest
        return orderedFiles.First();
        // foreach (var file in orderedFiles)
        // {
        //     print(file);
        //     return file;
        // }
    }
}
