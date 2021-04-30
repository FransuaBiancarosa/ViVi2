using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CilindroNB : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnChangeColor))]
    public int colorTrigger;
    private int old = 0;

    public void OnChangeColor(int old, int newVal) {
        if(newVal == 0) {
            GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0,0,1f));
        } else {
            GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1,0,0f));
        }
        this.old = newVal;
    }

    public void ChangeColor() {
        if(old == 0) {
            colorTrigger = 1;
            transform.position += new Vector3(5, 0, 0);
        } else {
           colorTrigger = 0; 
           transform.position -= new Vector3(5, 0, 0);
        }
    }

    private void Update() {
        if(Input.GetKeyUp(KeyCode.P)) {
            ChangeColor();
        }
    }
}
