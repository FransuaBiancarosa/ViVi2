using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShowTooltip : MonoBehaviour
{
    public string tooltipText;
    public ArrowDirection arrowDirection;
    public Vector3 offset;
    public float secondsToAppear = 2.0f;

    Coroutine showTooltipCoroutine;

    public void StartContdownToShowTooltip()
    {
        showTooltipCoroutine = StartCoroutine(ContdownToShowTooltip());
    }

    public void StopShowTooltip()
    {
        if(showTooltipCoroutine!=null)
        {
            StopCoroutine(showTooltipCoroutine);
        }

        TooltipHandler.singleton.HideTooltip();
    }

    IEnumerator ContdownToShowTooltip()
    {
        yield return new WaitForSeconds(secondsToAppear);

        TooltipHandler.singleton.SetUpTooltip(tooltipText, gameObject.transform.position + offset, arrowDirection);
    }
}
