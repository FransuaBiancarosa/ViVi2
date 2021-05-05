using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToSceneScript : MonoBehaviour
{
    //settare nell'event trigger l'evento di pulizia della scena
    public int teleportDestination;

    public void TeleportToScene()
    {
        //pulusci o resetta ciò che è necessario
        CloseCurrentActiveActionPanel();
        WarningManager.singleton.SetUpWarning(gameObject.transform.position, "Desideri spostarti ?", "Se primi si andrai nella scena "+ GlobalSettings.ElencoScene.scene[teleportDestination].json,
           () => { SceneManagerScript.singleton.LoadScene(teleportDestination); },
           () => { }
           );

        
    }
    public void SetOn()
    {
        SetHighlight(true);
    }

    public void SetOff()
    {
        SetHighlight(false);
    }

    public void SetHighlight(bool on)
    {
        if (on)
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f, 0f, 0f));
        }
        else
        {
            GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f, 1f, 1f));
        }
    }

    void CloseCurrentActiveActionPanel()
    {
        if (GlobalSettings.currentActiveMetadataHandler != null)
        {
            GlobalSettings.currentActiveMetadataHandler.SendMessage("Close");
        }
        GlobalSettings.currentActiveMetadataHandler = null;
    }
}
