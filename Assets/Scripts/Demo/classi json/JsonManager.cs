using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    [SerializeField] private string jsonName;
    private const string prefabFolder = "Prefabs";

    private void Awake()
    {
        GlobalSettings.RootFolder = "scene_config";
    }

    IEnumerator Start()
    {
        //deserializza json "root"
        string jsonPath = Path.Combine(GlobalSettings.RootFolder, jsonName);
        //string json = JsonReader.ReadJson(jsonPath);
        string json = Resources.Load<TextAsset>(jsonPath).text;

        GlobalSettings.ElencoScene = ListaScene.CreateFromJSON(json);

        //setta i parametri di connessione e si connette nella modalità opportuna
        var networkManager = GameObject.FindObjectOfType<MyNetworkManager>();
        networkManager.networkAddress = GlobalSettings.ElencoScene.ip;
        networkManager.SetPort(Convert.ToUInt16(GlobalSettings.ElencoScene.port));
        networkManager.gameObject.GetComponent<StartConnectionScript>().Connect();
        yield return new WaitUntil(() => Syncer.instance != null);

        //deserializza la scena di partenza
        /*
        json = JsonReader.ReadJson(Path.Combine(
            GlobalSettings.RootFolder,
            "scene",
            GlobalSettings.ElencoScene.scene[0].tipo,
            GlobalSettings.ElencoScene.scene[0].json + ".json")
        );*/
        jsonPath = Path.Combine(GlobalSettings.RootFolder, "scene", GlobalSettings.ElencoScene.scene[0].tipo, GlobalSettings.ElencoScene.scene[0].json);
        json = Resources.Load<TextAsset>(jsonPath).text;
        object scenaDaCostruire = null;
        SceneManagerScript sceneManager = GetComponentInParent<SceneManagerScript>();

        switch (GlobalSettings.ElencoScene.scene[0].tipo)
        {
            default: throw new Exception("The type provided for the scene is unsupported");
            case "foto360":
                scenaDaCostruire = ScenaFoto360.CreateFromJSON(json);
                sceneManager.LoadScene((scenaDaCostruire as ScenaFoto360).id);
                break;
            case "video360":
                scenaDaCostruire = ScenaVideo360.CreateFromJSON(json);
                sceneManager.LoadScene((scenaDaCostruire as ScenaVideo360).id);
                break;
            case "3D":
                scenaDaCostruire = Scena3D.CreateFromJSON(json);
                sceneManager.LoadScene((scenaDaCostruire as Scena3D).id);
                break;
        }


    }


}
