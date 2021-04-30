using System.Collections;
using UnityEngine;
using Mirror;

public class StartConnectionScript : MonoBehaviour
{

    void Start()
    {   
        //Connect();
    }

    public void Connect() {
        if(GlobalSettings.IsHost){
            GetComponent<NetworkManager>().StartHost();
        } else {
            StartCoroutine(ClientConnection(7f));
        } 
    }

    private IEnumerator ClientConnection(float pollingWaitingTime) {
        while(!NetworkClient.active) {
            GetComponent<NetworkManager>().StartClient();
            yield return new WaitForSeconds(pollingWaitingTime);
        }
        yield return null;
    }
}
