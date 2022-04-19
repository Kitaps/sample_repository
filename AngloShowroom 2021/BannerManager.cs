using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BannerManager : MonoBehaviour
{
    public int waitTime = 30;
    public List<MoveBanner> banners;

    public GameObject[] bannerImages;
    Dictionary<string, (ExternalMedia, FitContainer)>[] bannerImagesTuples;

    // Sets
    public HashSet<string> allBannerPaths;   // Stores paths
    public HashSet<string> currentBannerPaths;  // Has the info of the currently displayed images
    public HashSet<string> possibleBannerPaths;   // Images that can be chosen to replace already used ones

    // Aux Array
    // public string[] nextImages = new string[4];
    public List<string> possibleBannerList = new List<string>();
    public List<string> possibleBannerListAux = new List<string>();

    public UnityEvent bannerSwitchStarted = new UnityEvent();
    public TCPServer Server;

    private static System.Random rng = new System.Random(); 
    


    // AUX for debbuging

    void Awake()
    {
        // b1UEM = banner1Upper.GetComponent<ExternalMedia>();
        // b1UFC = banner1Upper.GetComponent<FitContainer>();
        
        // Setup Sets
        // There are 4 images on display, never more, never less
        currentBannerPaths = new HashSet<string>();
        GetBannerPaths();
        possibleBannerPaths = new HashSet<string>(allBannerPaths);

        BuildBannerStructure();


        // possibleBannerList.Add("1");
        // possibleBannerList.Add("2");
        // possibleBannerListAux = new List<string>(possibleBannerList);
        // possibleBannerList.Add("3");
        // possibleBannerList.Add("4");

        
    }
    void Start()
    {
        storeCurrents();
        StartCoroutine(Cycle());
    }

    IEnumerator Cycle()
    {
        for (; ; )
        {
            if(bannerImages[0].active && Server.canExecuteCommand)
            {
                bannerSwitchStarted.Invoke();
                foreach(MoveBanner banner in banners)
                {
                    banner.update_position();
                }
                // Update the current 
            /// storeCurrents();

            UpdatePossibles();

            SetNextImages();

            // Currently the image is changed after the wait.
            // That's why the currents have to be stored after the UpdatePossibles.
            // Once The SetNextImages occurs on time, say before the wait, store currents should be before updatePossibles
            storeCurrents();

            // Debug.Log("HACHAMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA 10");
            // change_image("/banners/bg.png");
            // Shuffle<string>(possibleBannerList);
            // Pop(possibleBannerList);
            // allBannerPaths.CopyTo(nextImages, 0, 4);
            }
            
            yield return new WaitForSeconds(waitTime);
        }
    }

    void change_image(string imagePath, (ExternalMedia, FitContainer) imagePropertyTuple)
    {
        if(imagePropertyTuple.Item1.relativeToStreamingAssets)
        {
            imagePropertyTuple.Item1.relativeToStreamingAssets = false;
        }
        imagePropertyTuple.Item1.filePath = imagePath;
        /// float refreshEvery_aux = imagePropertyTuple.Item1.refreshEvery;
        /// imagePropertyTuple.Item1.refreshEvery = 0;
        /// imagePropertyTuple.Item1.refreshEvery = refreshEvery_aux;
        imagePropertyTuple.Item1.Refresh();
        // b1UEM.filePath = imagePath;
        // // b1UEM.Refresh(); --> Error
        // float refreshEvery_aux = b1UEM.refreshEvery;
        // b1UEM.refreshEvery = 0;
        // b1UEM.refreshEvery = refreshEvery_aux;
        // // b1UFC.FitNewImage(); // <-- This would be the best option, but then there would be an issue with the starting image
    }

    void GetBannerPaths()
    {
        string directoryPath = $"{Application.streamingAssetsPath}/banners/";
        IEnumerable<string> filesList= Directory.GetFiles(directoryPath).Where(name => !name.EndsWith(".meta"));
        // filesList = filesList_aux;
        allBannerPaths = new HashSet<string>(filesList);
    }

    void BuildBannerStructure()
    {
        // This method builds a representation to the 4 banners's images
        // One upper and one lower for each Banner
        // Each image has an ExternalMedia(Item1) component and FitContainer(Item2) component
        bannerImagesTuples = new Dictionary<string, (ExternalMedia, FitContainer)>[4];
        for(int i=0; i<8; i+=2)
        {
            // The Upper and Lower in the dictionary will be references to the upper and lower image properties respictevily
            Dictionary<string, (ExternalMedia, FitContainer)> aux_dic = new Dictionary<string, (ExternalMedia, FitContainer)>();
            // Debug.Log($"{i} and {i+1} and {i/2}");
            (ExternalMedia, FitContainer) upper = GetMediaAndFit(bannerImages[i]);
            (ExternalMedia, FitContainer) lower = GetMediaAndFit(bannerImages[i+1]);

            aux_dic.Add("upper", upper);
            aux_dic.Add("lower", lower);

            // i will always be a multiple of 2, and i/2 will be the index in the list
            bannerImagesTuples[i / 2] = aux_dic; 
        }
    }

    (ExternalMedia, FitContainer) GetMediaAndFit(GameObject imageObject)
    {
        // Atomic function to get the components of an image object 
        ExternalMedia aux_externalMedia = imageObject.GetComponent<ExternalMedia>();
        FitContainer aux_fixContainer = imageObject.GetComponent<FitContainer>();
        return (aux_externalMedia, aux_fixContainer);
    }

    void storeCurrents()
    {
        // First we clean the currentSet, to save the new 4 current images
        currentBannerPaths.Clear();
        // We have 4 banners with 2 images each
        // We have to select the currently showing, 
        // therefore we need to choose the lower if banner is up, or upper if banner is not up
        string choose;
        for(int i=0; i<4; i++)
        {
            // We check if the banner is up or not
            if(banners[i].up)
            {
                choose = "lower";
            }
            else
            {
                choose = "upper";
            }
            // Debug.Log(bannerImagesTuples[i][choose].Item1.filePath);
            if(bannerImagesTuples[i][choose].Item1.relativeToStreamingAssets)
            {
                Debug.Log($"-------------------------------> {bannerImagesTuples[i][choose].Item1.filePath}");
                Debug.Log($"-----------------> {Application.streamingAssetsPath}{bannerImagesTuples[i][choose].Item1.filePath}");
                currentBannerPaths.Add($"{Application.streamingAssetsPath}{bannerImagesTuples[i][choose].Item1.filePath}");
            }

            // Add the image path from the ExternalMedia component from the upper or lower image from the banner i
            currentBannerPaths.Add(bannerImagesTuples[i][choose].Item1.filePath);
        }
    }

    void UpdatePossibles()
    {
        if(possibleBannerList.Count < 4)
        {
            ////Debug.Log("---------------------------------------------------------------------------------------------");
            // Refill P
            // Make a P set with all possible values except those already displayed
            HashSet<string> auxPSet = new HashSet<string>(allBannerPaths);
            ////Debug.Log($"allBannersPaths: {allBannerPaths.Count}");
            
            // Remove the currently displayed images
            ////Debug.Log($"auxPSet: {auxPSet.Count}");
            ////Debug.Log($"currentBannerPaths: {currentBannerPaths.Count}");
            auxPSet.ExceptWith(currentBannerPaths);
            ////Debug.Log($"auxPSet - CurrentBannerPaths: {auxPSet.Count}");
            
            // Remove the images still left in possibleBannerList
            // These will be added to the start
            ////Debug.Log($"possibleBannerList rest: {possibleBannerList.Count}");
            auxPSet.ExceptWith(possibleBannerList);
            ////Debug.Log($"auxPSet - Rest: {auxPSet.Count}");

            possibleBannerListAux = auxPSet.ToList();
            Shuffle<string>(possibleBannerListAux);
            ////Debug.Log($"mixed possibleBannerListAux: {possibleBannerListAux.Count}");
           
            // Add the rest of possibleBannerList
            possibleBannerListAux.AddRange(possibleBannerList);
            ////Debug.Log($"possibleBannerListAux + Rest: {possibleBannerListAux.Count}");
            
            // Replace
            possibleBannerList = new List<string>(possibleBannerListAux);
            ////Debug.Log($"possibleBannerList replaced: {possibleBannerList.Count}");
            ////Debug.Log("---------------------------------------------------------------------------------------------");
        }
    }

    public static void Shuffle<T>(IList<T> list)  
    {  
        // from https://stackoverflow.com/questions/273313/randomize-a-listt
        int n = list.Count;  
        while (n > 1) 
        {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }   

    string Pop(List<string> list)
    {   // Only usable with non empty Lists

        // Removes the last element from a list and returns it's value
        // The oposite to Add which adds an elemet to the tail
        string value = list.Last(); 
        list.RemoveAt(list.Count - 1);
        return value;
    }

    void SetNextImages()
    {
        // Like in Store Currents
        // We have 4 banners with 2 images each
        // We have to select the currently inactive (not showing)
        // therefore we need to choose the lower if banner is not up, or upper if banner is up
        string choose;
        for(int i=0; i<4; i++)
        {
            // We check if the banner is up or not
            if(banners[i].up)
            {
                choose = "lower";
            }
            else
            {
                choose = "upper";
            }
            // Once we have the banner and the position, we set an image path
            string selectedPath = Pop(possibleBannerList);
            change_image(selectedPath, bannerImagesTuples[i][choose]);
        }
    }

}
