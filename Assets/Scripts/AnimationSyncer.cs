using JetBrains.Annotations;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnimationSyncer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnSelectedAnimationChange))]
    public string animationName;

    [SyncVar(hook = nameof(OnPlayPressed))]
    public bool play;

    [SyncVar(hook = nameof(OnPausedPressed))]
    public bool pause;

    [SyncVar(hook = nameof(OnStopPressed))]
    public bool stop;

    Animation anim;
    bool isPaused;
    AudioSource animationAudio;

    int showIconMask;
    int hideIconMask;

    private void Awake()
    {
        anim = GetComponentInChildren<Animation>();
        animationAudio = GetComponentInChildren<AudioSource>();

        hideIconMask = (Camera.main.cullingMask) & ~(1 << LayerMask.NameToLayer("AugmentedIcon"));
        showIconMask = (Camera.main.cullingMask) | (1 << LayerMask.NameToLayer("AugmentedIcon"));
    }

    private void OnSelectedAnimationChange(string oldName, string newName)
    {
        if (GlobalSettings.IsHost)
            return;

        AudioClip animationAudioClip = Resources.Load<AudioClip>(Path.Combine("FileMultimediali", "AnimationAudio", gameObject.name.Replace("(Clone)", ""), animationName));
        if (animationAudio != null)
            animationAudio.clip = animationAudioClip;
    }

    //quando metto in pausa sull'host ricordarsi di settare anche play a false altrimenti quando ripremo play non mi parte nulla
    private void OnPlayPressed(bool oldPlay, bool newPlay)
    {
        if (GlobalSettings.IsHost)
            return;

        if (newPlay)
        {
            if (!isPaused)
            {
                if (anim[animationName].speed < 0)
                    anim[animationName].speed = 1;

                anim.Play(animationName);

                if (animationAudio.clip != null)
                    animationAudio.Play();
               
                Camera.main.cullingMask = hideIconMask;
            }
            else
            {
                anim[animationName].speed = 1;
                if (animationAudio.clip != null)
                    animationAudio.UnPause();

            }
        }
    }
    public void OnPausedPressed(bool oldPlay, bool newPlay)
    {
        if (GlobalSettings.IsHost)
            return;

        if (newPlay)
        {
            anim[animationName].speed = 0;
            if (animationAudio.clip != null)
                animationAudio.Pause();

            isPaused = true;
        }
    }

    public void OnStopPressed(bool oldPlay, bool newPlay)
    {
        if (GlobalSettings.IsHost)
            return;

        if (newPlay)
        {
            if (animationAudio.clip != null)
                animationAudio.Stop();

            anim.Stop();
            ResetModelAfterANimation();
            Camera.main.cullingMask = showIconMask;
            isPaused = false;
        }
    }

    void ResetModelAfterANimation()
    {
        anim[animationName].speed = -1;
        anim[animationName].time = 0.00001f;
        anim.Play(animationName);
        
    }
}
