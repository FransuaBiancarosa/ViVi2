using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    public static VideoHandler singleton;
    public GameObject videoPlayerCanvas;

    public VideoPlayer videoPlayer;
    public Slider videoTime;
    public TextMeshProUGUI videoTitle;
    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject nextVideoButton;
    public GameObject prevVideoButton;
    
    public List<ActionInput> videoPaths;
    int currentVideoId=0;
    bool isPaused;
    Coroutine currentWaitToEndCoroutine;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
    }
    public void SetUpVideoHandler(List<ActionInput> videoPaths, Vector3 spawnPosition)
    {
        if (videoPaths == null || videoPaths.Count==0)
            return;

        StopVideo();

        if (GlobalSettings.currentActiveMetadataHandler != null && GlobalSettings.currentActiveMetadataHandler != this.gameObject)
        {
            GlobalSettings.currentActiveMetadataHandler.SendMessage("Close");
        }
        GlobalSettings.currentActiveMetadataHandler = this.gameObject;


        GameObject tempPlayer = new GameObject();
        tempPlayer.transform.position = Camera.main.transform.position;
        Vector3 tempRotation = Camera.main.transform.rotation.eulerAngles;
        tempRotation.x = 0;
        tempRotation.z = 0;
        tempPlayer.transform.rotation =Quaternion.Euler(tempRotation);

      
        Vector3 spawnPos2 = Camera.main.transform.position + (tempPlayer.transform.forward * 1.5f);
        spawnPos2.y = 1.5f;
        spawnPos2.z = 4f;
        videoPlayerCanvas.transform.position = spawnPos2;
        Destroy(tempPlayer);

        videoPlayerCanvas.SetActive(true);
       

        this.videoPaths = videoPaths;
        if(videoPaths.Count<=1)
        {
            nextVideoButton.SetActive(false);
            prevVideoButton.SetActive(false);
        }
        else
        {
            nextVideoButton.SetActive(true);
            prevVideoButton.SetActive(true);
        }

        currentVideoId = 0;
        videoPlayer.clip = Resources.Load<VideoClip>(Path.Combine("FileMultimediali","Video", videoPaths[0].path));
        videoTitle.text = videoPaths[0].path;

    }

    public void PlayVideo()
    {
        videoPlayer.Play();
        
        if(!isPaused)
             currentWaitToEndCoroutine = StartCoroutine(WaitVideoToEnd());

        playButton.SetActive(false);
        pauseButton.SetActive(true);
       
        
        isPaused = false;
    }

    public void PauseVideo()
    {
        videoPlayer.Pause();
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        isPaused = true;
    }

    public void StopVideo()
    {
        if (currentWaitToEndCoroutine != null)
            StopCoroutine(currentWaitToEndCoroutine);

        videoTime.normalizedValue = 0;
        videoPlayer.Stop();

        videoPlayer.targetTexture.Release();

        playButton.SetActive(true);
        pauseButton.SetActive(false);
        isPaused = false;
    }

    public void NextVideo()
    {
        StopVideo();

        currentVideoId = ((currentVideoId + 1) % (videoPaths.Count));
        videoPlayer.clip = Resources.Load<VideoClip>(Path.Combine("FileMultimediali","Video", videoPaths[currentVideoId].path));
        videoTitle.text = videoPaths[currentVideoId].path;
    }

    public void PrevVideo()
    {
        StopVideo();

        currentVideoId--;
        if (currentVideoId < 0)
            currentVideoId = videoPaths.Count - 1;

     
        videoPlayer.clip = Resources.Load<VideoClip>(Path.Combine("FileMultimediali","Video", videoPaths[currentVideoId].path));
        videoTitle.text = videoPaths[currentVideoId].path;
    }

   IEnumerator WaitVideoToEnd()
    {
        while(videoPlayer.isPlaying || isPaused)
        {
            videoTime.normalizedValue = (float)(videoPlayer.time / videoPlayer.clip.length);
            yield return null;
        }
        videoTime.normalizedValue = 1;

        videoPlayer.targetTexture.Release();
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        yield return null;
    }

    public void Close()
    {
        StopVideo();
        videoPlayerCanvas.SetActive(false);
    }
}
