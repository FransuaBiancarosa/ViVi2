using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mirror;
using Oculus.Platform;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static  SceneManagerScript singleton;
    private FadeController fadeController;
    [SerializeField] private GameObject sceneParent;
    [SerializeField] private GameObject teleportPrefab;

    [Scene]
    [SerializeField]
    private string foto360Scene;
    [Scene]
    [SerializeField]
    private string video360Scene;

    [Scene]
    [SerializeField]
    private string tridimensionalScene;

    private string currentScene = "";

    void Start()
    {
        if (singleton == null || singleton != this)
            singleton = this;
        if (UnityEngine.Application.isPlaying)
            DontDestroyOnLoad(gameObject);
        fadeController = GetComponent<FadeController>();
    }

    public void LoadScene(int idScena) {
       
        Debug.Log("Loading a scene");
        Syncer.instance.sceneId = idScena;
    }

    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
            return;
        Debug.Log("Loading a scene by name");
        int idScena=0;
        for(int i=0;i<GlobalSettings.ElencoScene.scene.Count;i++)
        {
            if(GlobalSettings.ElencoScene.scene[i].json==sceneName)
            {
                idScena = i;
                Syncer.instance.sceneId = idScena;
                break;
            }
        }
        
    }

    private void ResetToDefault() {
        RenderSettings.skybox = null;
        DynamicGI.UpdateEnvironment();
    }

    public IEnumerator BuildScene(int id) {
        
        DebugVR.SetText($"Building scene with {id}");
        FadeOut();
        yield return new WaitForSeconds(2f);
        if(!string.IsNullOrEmpty(currentScene)) 
        {     
            yield return StartCoroutine((GlobalSettings.NetworkManager as MyNetworkManager).UnloadAdditiveSceneCoroutine());
            ResetToDefault();
            currentScene = "";
        }
        Scena scena = GlobalSettings.ElencoScene.scene[id];     
        string jsonPath = Path.Combine(GlobalSettings.RootFolder, "scene", scena.tipo, scena.json);
        object scenaDaCostruire;
        DebugVR.SetText("Before switch");
        switch(scena.tipo) {
            default: throw new Exception("The type provided for the scene is unsupported"); 
            case "foto360": 
            scenaDaCostruire = ScenaFoto360.CreateFromJSON(Resources.Load<TextAsset>(jsonPath).text);
            GlobalSettings.scenaDaCostruire = scenaDaCostruire;
            StartCoroutine((GlobalSettings.NetworkManager as MyNetworkManager).LoadSceneCoroutine(foto360Scene));
            currentScene = foto360Scene;
            break;

            case "video360": 
            scenaDaCostruire = ScenaVideo360.CreateFromJSON(Resources.Load<TextAsset>(jsonPath).text);
            StartCoroutine((GlobalSettings.NetworkManager as MyNetworkManager).LoadSceneCoroutine(video360Scene));
            currentScene = video360Scene;
            break;

            case "3D": 
            scenaDaCostruire = Scena3D.CreateFromJSON(Resources.Load<TextAsset>(jsonPath).text);
            string nameOfThe3DScene = (scenaDaCostruire as Scena3D).path;
            DebugVR.SetText(nameOfThe3DScene);
            StartCoroutine((GlobalSettings.NetworkManager as MyNetworkManager).LoadSceneCoroutine(nameOfThe3DScene));
            currentScene = nameOfThe3DScene;
            break;
        }
        //il fade in deve essere richiamato dalla scena corrente quando è pronta
        //altrimente il player si può vedere che viene spostato 
        /*
        yield return new WaitForSeconds(1f);
        FadeIn();
        yield return new WaitForSeconds(2f);
        */
    }

    public void FadeIn() {
        fadeController.FadeIn();
    }

    public void FadeOut() {
        fadeController.FadeOut();
    }

}
