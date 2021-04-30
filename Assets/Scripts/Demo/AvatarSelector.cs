using UnityEngine;

public class AvatarSelector : MonoBehaviour
{
    [SerializeField] private GameObject avatarHost;
    [SerializeField] private GameObject avatarClient;
    void Awake()
    {
        if(GlobalSettings.IsHost) {
            avatarClient.SetActive(false);
        } else {
            avatarHost.SetActive(false);
        }
    }
}
