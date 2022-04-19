using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Collections;
using RenderHeads.Media.AVProVideo;
using Sirenix.OdinInspector;
using DG.Tweening;

public class OnOffManager : MonoBehaviour
{
    // A script based on VIdeo Manager, but with less funtionality
    // Will only be used to play or not play the session start and end video
    public MediaPlayer videoPlayer;

    public GameObject renderScreen;

    // public AudioListener audio;

    public float waitTime = 5f;


    

    public void TurnOnOff(string choice)
    {
        switch(choice)
        {
            case "on":
                StartSession();
                break;

            case "off":
                EndSession();
                break;
        }
    }

    
    private void StartSession()
    {
        StartCoroutine(TurnOnCoroutine());
    }

    private void EndSession()
    {
        PlayOnOff();
        // Fade the volume from 1 to 0 in 3 seconds
        DOTween.To(x => AudioListener.volume = x, 1, 0, 3.0f);
    }

    private void PlayOnOff()
    {
        renderScreen.SetActive(true);
        videoPlayer.Play();
    }

    public void FinishPlaying()
    {
        renderScreen.SetActive(false);
        videoPlayer.Rewind(true); // Stops and Rewinds, being ready for next play
    }
    

    IEnumerator TurnOnCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        PlayOnOff();
        DOTween.To(x => AudioListener.volume = x, 0, 1, 3.0f);
    }
}
