using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] 
    [Range(-1,1f)]
    private float heightOffset;
    public static MapHandler instance;
    public bool IsActive {get; set;}

    void Awake()
    {
        instance = this;
    }

    public void EnableMap() {
        GameObject tempPlayer = new GameObject();
        tempPlayer.transform.position = Camera.main.transform.position;
        Vector3 tempRotation = Camera.main.transform.rotation.eulerAngles;
        tempRotation.x = 0;
        tempRotation.z = 0;
        tempPlayer.transform.position = new Vector3(tempPlayer.transform.position.x, 0.85f, tempPlayer.transform.position.z);
        tempPlayer.transform.rotation =Quaternion.Euler(tempRotation);
      
        Vector3 spawnPos2 = tempPlayer.transform.position + (tempPlayer.transform.forward * 4.5f * gameObject.transform.localScale.z);
        Debug.LogError(spawnPos2);
        spawnPos2.y = 1f * gameObject.transform.localScale.y;

        map.transform.position = spawnPos2;
        map.transform.LookAt(tempPlayer.transform);
        map.transform.Rotate(new Vector3(0, 180,0));
        map.transform.rotation = Quaternion.Euler(0, map.transform.rotation.eulerAngles.y, map.transform.rotation.eulerAngles.z);
        map.transform.Translate(new Vector3(0f, heightOffset, 0f));
        //Destroy(tempPlayer);

        map.SetActive(true);
        IsActive = true;
    }

    public void DisableMap() {
        map.SetActive(false);
        TooltipHandler.singleton.HideTooltip();
        IsActive = false;
    }
}
