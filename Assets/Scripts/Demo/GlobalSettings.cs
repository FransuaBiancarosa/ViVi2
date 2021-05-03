using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public static class GlobalSettings 
{
    public static bool IsHost;
    public static NetworkManager NetworkManager;
    public static ListaScene ElencoScene;
    public static string RootFolder;
    public const string PrefabRoot = "Prefabs";

    public static LocalPlayerController playerController;
    public static object scenaDaCostruire;

    public static Vector3 PositionHost;
    public static Vector3 PositionClient;
    public static List<GameObject> Interactables;

    public static GameObject currentActiveMetadataHandler;

    public static Vector3 FloatListToVector3(List<float> list)
    {
        Vector3 toReturn=Vector3.zero;
        if (list != null && list.Count == 3)
        {
            toReturn= new Vector3(list[0],
                                  list[1],
                                  list[2]);
        }
        return toReturn;
    }
}
