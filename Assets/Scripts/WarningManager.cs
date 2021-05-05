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

        warningCanvas.transform.position = spawnPosition;
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
}
