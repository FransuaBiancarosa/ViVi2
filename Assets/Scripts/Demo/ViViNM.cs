using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using RootMotion.FinalIK;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public struct ViViNetworkMessage : NetworkMessage
{
    public bool isHost;
    public Transform headAnchor;
    public Transform rHandAnchor;
    public Transform lHandAnchor;
}

public struct SceneChangedMessage : NetworkMessage {
    public string sceneName;
}

public enum Mode
{
    Host = 0, 
    Client = 1,
    Empty = 2
}

public class ViViNM : NetworkManager
{
    [SerializeField] private bool isHost;
    [SerializeField] private Transform playerHeadAnchor;
    [SerializeField] private Transform playerLHandAnchor;
    [SerializeField] private Transform playerRHandAnchor;
    [SerializeField] 
    private GameObject syncer;
    private int spawned = 0;
    public int counter = 0;
    public TMP_Text debug;

    private string sceneToLoad = "";
    
    public void SetPort(ushort port) {
        try {
            ((TelepathyTransport)transport).port = port;
        } catch(Exception e) {
            Debug.LogError(e.StackTrace);
        }
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
        ClientScene.PrepareToSpawnSceneObjects();
        NetworkClient.Send(new SceneChangedMessage() {
            sceneName = sceneToLoad
        });
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        try
        {
            NetworkServer.RegisterHandler(new Action<NetworkConnection, ViViNetworkMessage>((connection, message) => OnCreateCharacter(connection, message)));
            NetworkServer.RegisterHandler<SceneChangedMessage>(OnClientLoadedSceneServer);
            //Makes the Network Manager globally visible
            GlobalSettings.NetworkManager = this;
        } catch(Exception e)
        {
            Debug.LogError(e.StackTrace);
        } 
    }

    private void OnClientLoadedSceneServer(NetworkConnection connection, SceneChangedMessage message) {
        NetworkServer.SpawnObjects();
    }

    public override void Awake()
    {
        base.Awake();
        GlobalSettings.IsHost = this.isHost;
        GlobalSettings.NetworkManager = this;
    }

    private void OnCreateCharacter(NetworkConnection connection, ViViNetworkMessage message)
    {
        try
        {
            GameObject character;
            //Spawna un client o un host
            character = message.isHost ? Instantiate(spawnPrefabs[(int)Mode.Host]) : Instantiate(spawnPrefabs[(int)Mode.Client]);
            //setta il nome del client/host
            var controller = character.GetComponent<NetworkPlayerController>();
            character.transform.name = controller.Host == Mode.Host ? "Host" : "Client";
            //a seconda della tipologia
            int spawnIndex = (int)(controller.Host == Mode.Host ? Mode.Host : Mode.Client);
            Transform startPosition = startPositions[spawnIndex];
            character.transform.position = startPosition.position;
            character.transform.rotation = startPosition.rotation;

            NetworkServer.AddPlayerForConnection(connection, character);
        }
        catch (Exception e)
        {
            debug.text = e.StackTrace;
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    { 
        base.OnClientConnect(conn);

        try
        {
            counter++;
            if (GlobalSettings.IsHost)
            {
                NetworkServer.Spawn(Instantiate(syncer));
                Syncer.instance.OnHostConnected(playerHeadAnchor, playerLHandAnchor, playerRHandAnchor);
            }
            if(debug != null)
                debug.text = $"Connected {counter}";
            ViViNetworkMessage message;
            if (spawned == 0 && GlobalSettings.IsHost)
            {
                message = new ViViNetworkMessage
                {
                    isHost = true
                };
            }
            else
            {
                message = new ViViNetworkMessage
                {
                    isHost = false
                };
            }

            spawned++;
            conn.Send(message);
        }
        catch (Exception e)
        {
            if(debug != null) {
                if (Syncer.instance == null)
                    debug.text = "null";
                debug.text = e.StackTrace;
            } else Debug.LogError(e.StackTrace);
        }
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        counter--;
        if(debug != null)
            debug.text = $"Connected {counter}";
        //Retry to establish a new connection
        GetComponent<StartConnectionScript>().Connect();
    }

    public IEnumerator NetworkLoadSceneAsync(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //yield return new WaitUntil( () => pippo.isDone);

        sceneToLoad = sceneName;
        //TODO clients pending
        NetworkServer.SendToAll(new SceneMessage() {sceneName = sceneName, sceneOperation = SceneOperation.LoadAdditive});
        yield return null;
    }
}
