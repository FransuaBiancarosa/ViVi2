using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugVR : MonoBehaviour
{
    // Start is called before the first frame update
    private static DebugVR instance;
    [SerializeField] private TextMeshPro debugText; 
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this);
        if(debugText != null)
            debugText.text = "";
    }

    public static void SetText(string debug) {
        //instance.debugText.text += $"\n {debug}";
    }
}
