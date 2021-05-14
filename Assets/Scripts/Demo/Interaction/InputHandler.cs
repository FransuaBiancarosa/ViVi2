using UnityEngine;

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        if(OVRInput.GetUp(OVRInput.Button.One,OVRInput.Controller.RTouch) || OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch) || Input.GetKeyUp(KeyCode.S))
        {
            if(GlobalSettings.IsHost) {
                if(MapHandler.instance.IsActive) {
                    MapHandler.instance.DisableMap();
                } else {
                    MapHandler.instance.EnableMap();
                }
            }
        }
    }
}
