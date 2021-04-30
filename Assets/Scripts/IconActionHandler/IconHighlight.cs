using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconHighlight : MonoBehaviour
{
    public Color normalColor;
    public Color highlithColor;
    public Color selectedColor;
    public Image targetImage;

    bool objectSelected;
    bool objectHighlited;

    public void SetNormalColor()
    {
        if (targetImage != null && !objectSelected)
        {
            targetImage.color = normalColor;          
        }
        objectHighlited = false;
    }

    public void SetHighlightColor()
    {
        if (targetImage != null && !objectSelected)
        {
            targetImage.color = highlithColor;          
        }
        objectHighlited = true;
    }

    public void setSelectedColor()
    {
        if(objectSelected)
        {
            if(objectHighlited)
                targetImage.color = highlithColor;
            else
                targetImage.color = normalColor;

            objectSelected = false;
        }
        else
        {
            targetImage.color = selectedColor;
            objectSelected = true;
        }
    }

 
}
