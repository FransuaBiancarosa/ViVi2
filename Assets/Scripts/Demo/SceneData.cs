using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneData
{
    public int id;

    public static SceneData CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<SceneData>(jsonString);
    }
}
