using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using DG.Tweening;

public class MoveBanner : MonoBehaviour
{
    public int BannerID;
    public bool up; // If true one image is in layout, other is up
    public FitContainer upper;
    public FitContainer lower;
    public Transform T;

    public UnityEvent bannerSwitchFinished = new UnityEvent();

    public float distance = 15;

    public void update_position()
    {
        
        StartCoroutine(_update_position());
    }

    private IEnumerator _update_position()
    {
        yield return __update_position();
    }

    private IEnumerator __update_position()
    {
        // Change direction if necesary
        if((up && distance > 0) || (!up && distance < 0)) { distance = distance * (-1); }
        
        // Move
        T.DOMove(new Vector3(T.position.x, T.position.y + distance, T.position.z), 1).OnComplete(() =>
        {
            bannerSwitchFinished.Invoke();
        });

        // Change boolean True --> False || False --> True
        up = false == up;

        yield break;
    }


    
}
