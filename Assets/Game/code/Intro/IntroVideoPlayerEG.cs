using UnityEngine;
using UnityEngine.Video;

public class IntroVideoPlayerEG : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private IntroManagerEG introManager;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        introManager.IntroFinished();
    }

    void Update()
    {
        // Cho phép skip intro
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            introManager.IntroFinished();
        }
    }
}
