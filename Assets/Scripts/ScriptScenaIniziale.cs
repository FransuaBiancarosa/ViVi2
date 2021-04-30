using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptScenaIniziale : MonoBehaviour
{
    public Transform hostPosition;
    public Transform clientPosition;

    public GameObject telecamera;
    [SerializeField] private Material skybox3D;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        DebugVR.SetText("Loading starting scene");
        RenderSettings.skybox = skybox3D;
        DynamicGI.UpdateEnvironment();
        //riposiziona i player
        GlobalSettings.PositionHost = hostPosition.position + new Vector3(0,1.7f,0);
        GlobalSettings.PositionClient = clientPosition.position;
        GlobalSettings.playerController.SetAvatarPosition();

        //disattiva eventuali cose nella scena di root che potrebbero dare fastidio come luci ecc (ricordarsi di riattivarli on scene unload)

        //setta le cose necessarie
        if (GlobalSettings.IsHost)
        {
            GameObject go = Instantiate(telecamera);
            go.transform.position = clientPosition.position;
            go.transform.LookAt(hostPosition);
            SceneManager.MoveGameObjectToScene(go, this.gameObject.scene);
        }

        //fade in
        yield return new WaitForSeconds(1f);
        SceneManagerScript.singleton.FadeIn();
        yield return new WaitForSeconds(2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.T))
        {
            TeleportToScene();
        }
    }

    public void TeleportToScene()
    {
        //pulusci o resetta ciò che è necessario
        SceneManagerScript.singleton.LoadScene("primaScena360");
    }
}
