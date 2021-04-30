using UnityEngine;
using Oculus.Avatar;
using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;

public class AvatarManager : MonoBehaviour
{
        [SerializeField] private Transform CameraAnchor;
        [SerializeField] private Transform LHandAnchor;
        [SerializeField] private Transform RHandAnchor;
    void Awake()
    {
        Core.Initialize();
        //Users.GetLoggedInUser().OnComplete(GetLoggedInUserCallback);
        //Request.RunCallbacks();  //avoids race condition with OvrAvatar.cs Start().
        //TODO Explore this method
        //OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger);
    }

    private void GetLoggedInUserCallback(Message<User> message)
    {
        
    }

    /**
    private void GetLoggedInUserCallback(Message<User> message) {
        if (!message.IsError) {

            OvrAvatar[] avatars = FindObjectsOfType(typeof(OvrAvatar)) as OvrAvatar[];
            foreach (OvrAvatar avatar in avatars)
            {
                avatar.oculusUserID = message.Data.ID.ToString();
            }
        }
    }*/

    //public virtual void Update()
    //{
    //    // Look for updates from remote users
    //    p2pManager.GetRemotePackets();

    //    // update avatar mouths to match voip volume
    //    foreach (KeyValuePair<ulong, RemotePlayer> kvp in remoteUsers)
    //    {
    //        float remoteVoiceCurrent = Mathf.Clamp(kvp.Value.voipSource.peakAmplitude * VOIP_SCALE, 0f, 1f);
    //        kvp.Value.RemoteAvatar.VoiceAmplitude = remoteVoiceCurrent;
    //    }

    //    if (localAvatar != null)
    //    {
    //        localAvatar.VoiceAmplitude = Mathf.Clamp(voiceCurrent * VOIP_SCALE, 0f, 1f);
    //    }
    //}
}
