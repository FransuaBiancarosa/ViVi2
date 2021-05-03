using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct ClientLoadedScene : NetworkMessage
{
    public string sceneName;
}

public struct PlayerMessage : NetworkMessage {
    public bool isHost;
}

public class MyNetworkManager : NetworkManager
{
    public delegate void PostLoadSceneFunction();

    public bool invokeNetworkManagerBaseAwake;

    [Scene] public string sceneCheckerAdditiveScene;

    [Scene] public string noSceneCheckerAdditiveScene;

    #region vivi
    [SerializeField] private bool isHost;
    [SerializeField] private Transform playerHeadAnchor;
    [SerializeField] private Transform playerLHandAnchor;
    [SerializeField] private Transform playerRHandAnchor;
    private int spawned = 0;
    public int counter = 0;

    #endregion


    private bool loadingSceneWithSceneChecker = false;
    private string additiveSceneLoaded;
    private SceneOperation currentSceneOperation;
    private List<NetworkConnectionToClient> clientsPendingSceneLoading = new List<NetworkConnectionToClient>(); 

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<ClientLoadedScene>(OnClientLoadedSceneServer);
        NetworkServer.RegisterHandler<PlayerMessage>(OnCreateCharacter);
        GlobalSettings.NetworkManager = this;
    }

    private void OnCreateCharacter(NetworkConnection connection, PlayerMessage message) {
        GameObject character;
        
        character = message.isHost ? Instantiate(spawnPrefabs[0]) : Instantiate(spawnPrefabs[1]);
        character.transform.name = message.isHost ? "Host" : "Client";
        NetworkServer.AddPlayerForConnection(connection, character);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        PlayerMessage message;
        if(GlobalSettings.IsHost) {
            NetworkServer.Spawn(Instantiate(spawnPrefabs[2]));
            Syncer.instance.OnHostConnected(playerHeadAnchor, playerLHandAnchor, playerRHandAnchor);
        }
        if(spawned == 0 && GlobalSettings.IsHost) {
            message = new PlayerMessage() {
                isHost = true
            };
        } else {
            message = new PlayerMessage() {
                isHost = false
            };
        }
        spawned++;
        conn.Send(message);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        StartConnectionScript connectionManager = GetComponent<StartConnectionScript>();
        if(connectionManager != null)
            connectionManager.Connect();
    }

    public override void Awake()
    {
        //if(invokeNetworkManagerBaseAwake)
        base.Awake();
        GlobalSettings.IsHost = this.isHost;
        GlobalSettings.NetworkManager = this;
    }


    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);

        //Save scene to load
        additiveSceneLoaded = newSceneName;
        loadingSceneWithSceneChecker = newSceneName.Equals(sceneCheckerAdditiveScene);

        currentSceneOperation = sceneOperation;
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);

        //Check if the scene to load is the one with the SceneChecker logic or if it is an unload operation
        if (loadingSceneWithSceneChecker || currentSceneOperation == SceneOperation.UnloadAdditive)
            return;

        //Loading the scene WITHOUT the SceneChecker logic...custom implementation
        
        ClientScene.PrepareToSpawnSceneObjects();
        //Tell server this client has successfully loaded the issued scene
        NetworkClient.Send(new ClientLoadedScene() { sceneName = additiveSceneLoaded });
    }

    private void OnClientLoadedSceneServer(NetworkConnection arg1, ClientLoadedScene arg2)
    {
        //Invoked on the server when a client has completed loading up its scene
        if (!clientsPendingSceneLoading.Contains(NetworkServer.connections[arg1.connectionId]))
        {
            Debug.LogError("Can't find netwrok connection");
            return;
        }

        clientsPendingSceneLoading.Remove(NetworkServer.connections[arg1.connectionId]);
        if (clientsPendingSceneLoading.Count == 0)
        {
            //All clients have successfully loaded their scenes
            NetworkServer.SpawnObjects();
        }
    }

    public IEnumerator UnloadAdditiveSceneCoroutine()
    {
        if (string.IsNullOrEmpty(additiveSceneLoaded))
            yield break;

        yield return SceneManager.UnloadSceneAsync(additiveSceneLoaded);
        
        NetworkServer.SendToAll(new SceneMessage(){ sceneName = additiveSceneLoaded, sceneOperation = SceneOperation.UnloadAdditive});

        additiveSceneLoaded = null;
    }

    public IEnumerator LoadSceneCoroutine(string sceneName)
    {
        DebugVR.SetText("Before load scene");
        //This is invoked on the SERVER
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        DebugVR.SetText("Load Scene Coroutine");

        additiveSceneLoaded = sceneName;
        //When scene loading has completed prepare the list of connection from which we expect a completed scene load message (OnClientLoadedSceneServer)
        clientsPendingSceneLoading = new List<NetworkConnectionToClient>(NetworkServer.connections.Values);

        NetworkServer.SendToAll(new SceneMessage(){ sceneName = additiveSceneLoaded, sceneOperation = SceneOperation.LoadAdditive});
    }

    public void SetPort(ushort port) {
        ((IgnoranceTransport.Ignorance)transport).port = port;
    }

    public void OnGUI()
    {
        /*
        if (!NetworkServer.active)
            return;

        if(string.IsNullOrEmpty(additiveSceneLoaded))
            if (GUI.Button(new Rect(10, Screen.height - 50, 300, 50), "Additive Load Scene With SceneChecker"))
                StartCoroutine(LoadSceneCoroutine(sceneCheckerAdditiveScene));

        if (string.IsNullOrEmpty(additiveSceneLoaded))
            if (GUI.Button(new Rect(10, Screen.height - 100, 300, 50), "Additive Load Scene With NO SceneChecker"))
                StartCoroutine(LoadSceneCoroutine(noSceneCheckerAdditiveScene));

        if (!string.IsNullOrEmpty(additiveSceneLoaded))
            if (GUI.Button(new Rect(10, Screen.height - 150, 300, 50), "Unload Additive Scene"))
                StartCoroutine(UnloadAdditiveSceneCoroutine());
                */
    }


}
