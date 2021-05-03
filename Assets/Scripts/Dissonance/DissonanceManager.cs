using Dissonance;
using UnityEngine;

public class DissonanceManager : MonoBehaviour
{
    [SerializeField] private VoiceBroadcastTrigger broadcastTrigger;
    [SerializeField] private MuteHost hostSilencer;
    public static DissonanceManager manager;
    public MuteHost Silencer {get;}
    void Start()
    {
        if(broadcastTrigger == null)
            broadcastTrigger = GameObject.FindObjectOfType<VoiceBroadcastTrigger>();
        if(hostSilencer)
            hostSilencer = GameObject.FindObjectOfType<MuteHost>();

        if(!GlobalSettings.IsHost) {
            broadcastTrigger.enabled = false;
        }

        manager = this;
    }
}
