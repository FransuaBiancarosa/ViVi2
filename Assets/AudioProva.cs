using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioProva : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.P))
            GetComponent<AudioSource>().Play();
    }
}
