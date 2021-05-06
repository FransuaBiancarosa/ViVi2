using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ArrowDirection
{
   up=0,
   down,
   right,
   left
}
public class TooltipHandler : MonoBehaviour
{
    public GameObject tooltipObject;
    public TextMeshProUGUI tooltipText;
    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject rightArrow;
    public GameObject leftArrow;
    public static TooltipHandler singleton;

    private void Awake()
    {
        if(singleton==null)
        {
            singleton = this;
        }
    }

    public void SetUpTooltip(string tooltipTextMessage,Vector3 tooltipPosition, ArrowDirection arrowDirection)
    {
        tooltipText.text = tooltipTextMessage;
        SetArrow(arrowDirection);

        tooltipObject.transform.position = tooltipPosition;
        tooltipObject.transform.LookAt(Camera.main.transform);
        Vector3 eulerRotation = tooltipObject.transform.rotation.eulerAngles;
        tooltipObject.transform.rotation = Quaternion.Euler(0, eulerRotation.y, 0);
        tooltipObject.SetActive(true);
    }

    void SetArrow(ArrowDirection direction)
    {
        upArrow.SetActive(false);
        downArrow.SetActive(false);
        rightArrow.SetActive(false);
        leftArrow.SetActive(false);

        switch (direction)
        {
            case ArrowDirection.up: upArrow.SetActive(true); break;
            case ArrowDirection.down: downArrow.SetActive(true); break;
            case ArrowDirection.right: rightArrow.SetActive(true); break;
            case ArrowDirection.left: leftArrow.SetActive(true); break;
        }
    }

    public void HideTooltip()
    {
       tooltipObject.SetActive(false);
    }
}
