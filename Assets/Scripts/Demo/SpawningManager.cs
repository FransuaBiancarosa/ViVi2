using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    [SerializeField] private GameObject StartNetworkPositionPrefab;
    [SerializeField] private GameObject CameraPlaceholder;

    //TODO add a rotation for the camera
    public void SpawnStartPositions(Vector3 virgil, Vector3 client)
    {
        Instantiate(StartNetworkPositionPrefab, virgil, Quaternion.identity);
        Instantiate(StartNetworkPositionPrefab, client, Quaternion.identity);
        if(CameraPlaceholder != null && GlobalSettings.IsHost)
        {
            Instantiate(CameraPlaceholder, client, Quaternion.Euler(new Vector3(0f, 90f, 0f)));
        }
    }
    
}
