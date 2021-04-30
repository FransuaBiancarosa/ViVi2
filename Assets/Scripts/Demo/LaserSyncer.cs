using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSyncer : NetworkBehaviour
{
    private LineRenderer originalLine;
    private LineRenderer clonedLine;
    private GameObject originalCursor;
    private GameObject sphere;
    [SyncVar(hook = nameof(OnLaserChanged))]
    private Vector3 origin;
    [SyncVar(hook = nameof(OnLaserChanged))]
    private Vector3 end;
    [SyncVar(hook = nameof(OnCursorChanged))]
    private bool hitting;
    [SyncVar(hook = nameof(OnCursorChanged))]
    private bool laserActive;

    private void Awake()
    {
        if (GlobalSettings.IsHost)
        {
            //LaserPointer linea = FindObjectOfType<LaserPointer>();
            LaserPointer linea = FindObjectOfType<PointerScript>().transform.FindChildRecursive("LaserPointer").GetComponent<LaserPointer>();
            if(linea == null)
                Debug.Break();
            originalLine = linea.GetComponent<LineRenderer>();
            if (originalLine == null)
                Debug.LogError("original line null");
            originalCursor = linea.cursorVisual;
        }
        else
        {
            GameObject linea = GameObject.FindGameObjectWithTag("ClonedLaser");
            clonedLine = linea.GetComponent<LineRenderer>();
            clonedLine.enabled = false;
            sphere = linea.transform.GetChild(0).gameObject;
            sphere.SetActive(false);
        }
    }

    private void Update()
    {
        if(GlobalSettings.IsHost)
        {
            origin = originalLine.GetPosition(0);
            end = originalLine.GetPosition(1);
            hitting = originalCursor.activeInHierarchy;
            laserActive = originalLine.enabled;
        }
    }

    private void OnLaserChanged(Vector3 oldLaser, Vector3 newLaser)
    {
        if (!GlobalSettings.IsHost && clonedLine != null && sphere != null)
        {
            if(clonedLine == null)
                Debug.LogError("cloned line is null");
            clonedLine.SetPosition(0, origin);
            clonedLine.SetPosition(1, end);
            sphere.transform.position = end;
        }
    }

    private void OnCursorChanged(bool oldCursor, bool newCursor)
    {
        if (!GlobalSettings.IsHost && clonedLine != null && sphere != null)
        {
            sphere.SetActive(hitting);
            clonedLine.gameObject.SetActive(laserActive);
        }
    }
}
