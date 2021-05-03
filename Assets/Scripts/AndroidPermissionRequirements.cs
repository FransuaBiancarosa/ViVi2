using UnityEngine;
using UnityEngine.Android;

public class AndroidPermissionRequirements : MonoBehaviour
{
    void Awake()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
            //dialog = new GameObject();
        }
#endif
    }
}
