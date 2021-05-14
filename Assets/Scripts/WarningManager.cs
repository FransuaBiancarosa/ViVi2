using RootMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarningManager : MonoBehaviour
{
    public static WarningManager singleton;
    public delegate void WarningActionDelegate(); // This defines what type of method you're going to call.

    public WarningActionDelegate yesActionEvent; // This is the variable holding the method you're going to call.
    public WarningActionDelegate noActionEvent;

    public TextMeshProUGUI warningTitle;
    public TextMeshProUGUI warningDescription;

    public GameObject warningCanvas;
   
    private void Awake()
    {
        if(singleton==null)
        {
            singleton = this;
        }
    }

    public void SetUpWarning(Vector3 spawnPosition,string warningTitleText,string warningDescriptionText, WarningActionDelegate yesAction, WarningActionDelegate noAction)
    {
        if(!string.IsNullOrEmpty(warningTitleText))
        {
            warningTitle.text = warningTitleText;
        }
        else
        {
            warningTitle.text = "Stai per eseguire l'azione";
        }

        if (!string.IsNullOrEmpty(warningDescriptionText))
        {
            warningDescription.text = warningDescriptionText;
        }
        else
        {
            warningDescription.text = "";
        }

        yesActionEvent = yesAction;
        noActionEvent = noAction;

        SpawnInFrontOfPlayer(warningCanvas,1.7f,4.5f);
       
        warningCanvas.SetActive(true);

    }

    public void YesAction()
    {
        yesActionEvent.Invoke();
        Close();
    }

    public void NoAction()
    {
        noActionEvent.Invoke();
        Close();
    }

    void Close()
    {
        yesActionEvent = null;
        noActionEvent = null;
        warningCanvas.SetActive(false);
    }

    void SpawnInFrontOfPlayer(GameObject go,float height,float distance)
    {
        GameObject tempPlayer = new GameObject();
        tempPlayer.transform.position = Camera.main.transform.position;
        Vector3 tempRotation = Camera.main.transform.rotation.eulerAngles;
        tempRotation.x = 0;
        tempRotation.z = 0;
        tempPlayer.transform.position = new Vector3(tempPlayer.transform.position.x, height, tempPlayer.transform.position.z);
        tempPlayer.transform.rotation = Quaternion.Euler(tempRotation);

        Vector3 spawnPos2 = tempPlayer.transform.position + (tempPlayer.transform.forward * distance);
        Debug.LogError(spawnPos2);
        spawnPos2.y = 1f * gameObject.transform.localScale.y;

        go.transform.position = spawnPos2;

        go.transform.LookAt(tempPlayer.transform);
        go.transform.Rotate(new Vector3(0, 180, 0));
        go.transform.rotation = Quaternion.Euler(0, go.transform.rotation.eulerAngles.y, go.transform.rotation.eulerAngles.z);
      
        Destroy(tempPlayer);

    }
}
