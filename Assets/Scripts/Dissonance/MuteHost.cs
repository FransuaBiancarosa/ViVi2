using Dissonance;
using UnityEngine;

public class MuteHost : MonoBehaviour
{
    [SerializeField] private DissonanceComms dissonanceComms;
    
    void Start()
    {
        if(dissonanceComms == null)
            dissonanceComms = GameObject.FindObjectOfType<DissonanceComms>();
    }

    public void SetMutedState(bool isMuted = false) {
        dissonanceComms.IsMuted = isMuted;
    }

    
}
