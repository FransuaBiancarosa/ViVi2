using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AnimationHandler : MonoBehaviour
{
    public static AnimationHandler singleton;
    public GameObject objectToPlayAnimation;
    public GameObject animationPlayerCanvas;

    public AudioSource animationAudio;

    public Slider animationTime;
    public TextMeshProUGUI animationTitle;
    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject nextButton;
    public GameObject prevButton;

    public List<ActionInput> animationPaths;
    int currentAnimationId = 0;
    bool isPaused;

    Animation anim;
    Coroutine currentWaitToEndCoroutine;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
    }
    public void SetUpAnimationHandler(List<ActionInput> animationPaths, Vector3 spawnPosition, GameObject animableObject)
    {
        if (animationPaths == null || animationPaths.Count == 0)
            return;

        StopAnimation();

        if (GlobalSettings.currentActiveMetadataHandler != null && GlobalSettings.currentActiveMetadataHandler != this.gameObject)
        {
            GlobalSettings.currentActiveMetadataHandler.SendMessage("Close");
        }
        GlobalSettings.currentActiveMetadataHandler = this.gameObject;

        objectToPlayAnimation = animableObject;
        anim = objectToPlayAnimation.GetComponentInChildren<Animation>();

        animationPlayerCanvas.SetActive(true);
        animationPlayerCanvas.transform.position = spawnPosition;

        this.animationPaths = animationPaths;
        if (animationPaths.Count <= 1)
        {
            nextButton.SetActive(false);
            prevButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
            prevButton.SetActive(true);
        }

        currentAnimationId = 0;
        animationTitle.text = animationPaths[0].path;

        AudioClip animationAudioClip= Resources.Load<AudioClip>(Path.Combine("FileMultimediali", "AnimationAudio", objectToPlayAnimation.name.Replace("(Clone)", ""), animationPaths[0].path));
        if (animationAudio != null)
            animationAudio.clip = animationAudioClip;

    }

    public void PlayAnimation()
    {
        if (anim == null)
            return;

        if (!isPaused)
        {
            if (anim[animationPaths[currentAnimationId].path].speed < 0)
                anim[animationPaths[currentAnimationId].path].speed = 1;

            anim.Play(animationPaths[currentAnimationId].path);
            if (animationAudio.clip != null)
                animationAudio.Play();
            currentWaitToEndCoroutine = StartCoroutine(WaitAnimationToEnd());           
        }
        else
        {
            anim[animationPaths[currentAnimationId].path].speed=1;
            if (animationAudio.clip != null)
                animationAudio.UnPause();
        }

        

        playButton.SetActive(false);
        pauseButton.SetActive(true);

        isPaused = false;
    }

    public void PauseAnimation()
    {
        if (anim == null)
            return;

        anim[animationPaths[currentAnimationId].path].speed = 0;
        if (animationAudio.clip != null)
            animationAudio.Pause();

        playButton.SetActive(true);
        pauseButton.SetActive(false);
        isPaused = true;
    }

    public void StopAnimation()
    {
        if (anim == null)
            return;

        if (currentWaitToEndCoroutine != null)
            StopCoroutine(currentWaitToEndCoroutine);

        if (animationAudio.clip != null)
            animationAudio.Stop();

        anim.Stop();
        ResetModelAfterANimation();

        playButton.SetActive(true);
        pauseButton.SetActive(false);
        isPaused = false;
    }
    
    void ResetModelAfterANimation()
    {
        anim[animationPaths[currentAnimationId].path].speed = -1;
        anim[animationPaths[currentAnimationId].path].time = 0.00001f;
        anim.Play(animationPaths[currentAnimationId].path);
        animationTime.normalizedValue = 0;
    }

    public void NextAnimation()
    {
        StopAnimation();
        currentAnimationId = ((currentAnimationId + 1) % (animationPaths.Count));
        animationTitle.text = animationPaths[currentAnimationId].path;

        AudioClip animationAudioClip = Resources.Load<AudioClip>(Path.Combine("FileMultimediali", "AnimationAudio", objectToPlayAnimation.name.Replace("(Clone)", ""), animationPaths[currentAnimationId].path));
        if (animationAudio != null)
            animationAudio.clip = animationAudioClip;
    }

    public void PrevAnimation()
    {
        StopAnimation();

        currentAnimationId--;
        if (currentAnimationId < 0)
            currentAnimationId = animationPaths.Count - 1;

        animationTitle.text = animationPaths[currentAnimationId].path;
        AudioClip animationAudioClip = Resources.Load<AudioClip>(Path.Combine("FileMultimediali","AnimationAudio", objectToPlayAnimation.name.Replace("(Clone)", ""), animationPaths[currentAnimationId].path));
        if (animationAudio != null)
            animationAudio.clip = animationAudioClip;
    }

    IEnumerator WaitAnimationToEnd()
    {
        while (anim.isPlaying || isPaused)
        {
            animationTime.normalizedValue = anim[animationPaths[currentAnimationId].path].normalizedTime;
            yield return null;
        }

        if (animationAudio.clip != null)
            animationAudio.Stop();

        animationTime.normalizedValue = 1;
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        yield return null;
    }

    public void Close()
    {
        StopAnimation();
        animationPlayerCanvas.SetActive(false);
    }
}
