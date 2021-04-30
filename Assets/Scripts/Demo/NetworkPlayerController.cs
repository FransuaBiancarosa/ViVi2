using UnityEngine;
using Mirror;


public class NetworkPlayerController : NetworkBehaviour
{
    [SyncVar]
    public int prova = 0;
    [SerializeField] private Mode host;
    public Mode Host { get => host; }

    public override void OnStartLocalPlayer()
    {
        //FindObjectOfType<LocalPlayerController>().OnRemotePlayerActivated();
        Debug.Log("Local Player Started");
        
    }

    void Update()
    {
        //regular clients cannot do anything related to the network behaviour
        if(!GlobalSettings.IsHost)
            return;
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch) || OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            prova++;
    }
}
