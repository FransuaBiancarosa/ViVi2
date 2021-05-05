using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler singleton;
    public GameObject audioPlayerCanvas;

    public AudioSource audioPlayer;
    public Slider audioTime;
    public Text audioTitle;
   // public TextMeshProUGUI audioTitle;
    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject nextButton;
    public GameObject prevButton;

    public List<ActionInput> audioPaths;
    int currentAudioId = 0;
    bool isPaused;
    Coroutine currentWaitToEndCoroutine;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
    }
    public void SetUpAudioHandler(List<ActionInput> audioPaths, Vector3 spawnPosition)
    {
        if (audioPaths == null || audioPaths.Count == 0)
            return;

        StopAudio();

        if (GlobalSettings.currentActiveMetadataHandler != null && GlobalSettings.currentActiveMetadataHandler!=this.gameObject)
        {
            GlobalSettings.currentActiveMetadataHandler.SendMessage("Close");
        }
        GlobalSettings.currentActiveMetadataHandler = this.gameObject;

        GameObject tempPlayer = new GameObject();
        tempPlayer.transform.position = Camera.main.transform.position;
        Vector3 tempRotation = Camera.main.transform.rotation.eulerAngles;
        tempRotation.x = 0;
        tempRotation.z = 0;
        tempPlayer.transform.rotation = Quaternion.Euler(tempRotation);


        Vector3 spawnPos2 = Camera.main.transform.position + (tempPlayer.transform.forward * 1.5f);
        spawnPos2.y = 1.7f;
        spawnPos2.z = 4.5f;
        audioPlayerCanvas.transform.position = spawnPos2;
        Destroy(tempPlayer);

        audioPlayerCanvas.SetActive(true);
        

        this.audioPaths = audioPaths;
        if (audioPaths.Count <= 1)
        {
            nextButton.SetActive(false);
            prevButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
            prevButton.SetActive(true);
        }

        currentAudioId = 0;
        audioPlayer.clip = Resources.Load<AudioClip>(Path.Combine("FileMultimediali","Audio", audioPaths[0].path));
        audioTitle.text = audioPaths[0].path;

    }

    public void PlayAudio()
    {
        audioPlayer.Play();

        if (!isPaused)
            currentWaitToEndCoroutine = StartCoroutine(WaitAudioToEnd());

        playButton.SetActive(false);
        pauseButton.SetActive(true);


        isPaused = false;
    }

    public void PauseAudio()
    {
        audioPlayer.Pause();
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        isPaused = true;
    }

    public void StopAudio()
    {
        if (currentWaitToEndCoroutine != null)
            StopCoroutine(currentWaitToEndCoroutine);

        audioTime.normalizedValue = 0;
        audioPlayer.Stop();

        playButton.SetActive(true);
        pauseButton.SetActive(false);
        isPaused = false;
    }

    public void NextAudio()
    {
        StopAudio();

        currentAudioId = ((currentAudioId + 1) % (audioPaths.Count));
        audioPlayer.clip = Resources.Load<AudioClip>(Path.Combine("FileMultimediali","Audio", audioPaths[currentAudioId].path));
        audioTitle.text = audioPaths[currentAudioId].path;
    }

    public void PrevAudio()
    {
        StopAudio();

        currentAudioId--;
        if (currentAudioId < 0)
            currentAudioId = audioPaths.Count - 1;


        audioPlayer.clip = Resources.Load<AudioClip>(Path.Combine("FileMultimediali","Audio", audioPaths[currentAudioId].path));
        audioTitle.text = audioPaths[currentAudioId].path;
    }

    IEnumerator WaitAudioToEnd()
    {
        while (audioPlayer.isPlaying || isPaused)
        {
            audioTime.normalizedValue = (float)(audioPlayer.time / audioPlayer.clip.length);
            yield return null;
        }
        audioTime.normalizedValue = 1;
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        yield return null;
    }

    public void Close()
    {
        StopAudio();
        audioPlayerCanvas.SetActive(false);
    }
}
