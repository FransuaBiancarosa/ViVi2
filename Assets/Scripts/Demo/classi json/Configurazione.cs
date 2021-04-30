using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class JsonReader
{
    public static string ReadJson(string path)
    {
        string output = "";
        using (StreamReader r = new StreamReader(path))
        {
            output = r.ReadToEnd();
        }
        return output;
    }
}

public enum Tipo
{
    other = 0,
    foto360,
    video360,
    tridimensionale
}

#region Json - lista delle scene
[Serializable]
public class ListaScene
{
    public int id; 
    public string ip;
    public string port;
    public List<Scena> scene;
    public static ListaScene CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<ListaScene>(jsonString);
    }
}

[Serializable]
public class Scena
{
    public string tipo;
    public string json;
}
#endregion

[Serializable]
public class ScenaVideo360
{
    public string tipo;
    public int id;
    public string hostVideo;
    public string clientVideo;
    public List<float> hostSpawnPos;
    public List<float> clientSpawnPos;
    public int destinazione;
    public static ScenaVideo360 CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<ScenaVideo360>(jsonString);
    }
}

[Serializable]
public class Scena3D
{
    public string tipo;
    public int id;
    public string path;
    public int destinazione;
    public static Scena3D CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<Scena3D>(jsonString);
    }
}

[Serializable]
public class ScenaFoto360
{
    public string tipo;
    public int id;
    public string hostSfondo;
    public List<float> hostSpawnPos;
    public List<GuestInfo> guestInfos;
    public List<Teletrasporto> teletrasporti;
    public List<OggettoAumentato> oggettiAumentati;
    public static ScenaFoto360 CreateFromJSON(string jsonString) {
        return JsonUtility.FromJson<ScenaFoto360>(jsonString);
    }
}

[Serializable]
public class GuestInfo {
    public List<float> posizione;
    public string sfondo;
}

[Serializable]
public class Teletrasporto
{
    public string tipo;
    public string gobject;
    public List<float> posizione;
    public List<float> rotazione;
    public List<float> scala;
    public int destinazione;
}

[Serializable]
public class OggettoAumentato
{
    public string tipo;
    public bool network;
    public string gobject;
    public List<float> posizione;
    public List<float> rotazione;
    public List<float> scala;
    public List<PuntoDiInterazione> puntiDiInterazione;
}

[Serializable]
public class PuntoDiInterazione
{
    public List<float> posizione;
    public List<float> rotazione;
    public List<float> scala;
    public List<Azione> azioni;
    //public string tipo;
    //public List<Input> input;
    public List<int> owner;
}

[Serializable]
public class Azione
{
    public string tipo;
    public List<ActionInput> input;
    //public List<int> owner;
}

[Serializable]
public class ActionInput
{
    public string path;
}
