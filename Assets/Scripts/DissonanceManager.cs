using System.Collections;
using System.Collections.Generic;
using Dissonance;
using UnityEngine;

public class DissonanceManager : MonoBehaviour
{
    [SerializeField] private VoiceBroadcastTrigger broadcastTrigger;
    // Start is called before the first frame update
    void Start()
    {
        if(broadcastTrigger == null)
            broadcastTrigger = GameObject.FindObjectOfType<VoiceBroadcastTrigger>();
        if(!GlobalSettings.IsHost) {
            //broadcastTrigger.IsMuted = true;
            broadcastTrigger.enabled = false;
        }
    }
}
