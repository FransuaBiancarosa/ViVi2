using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : MonoBehaviour
{
    public static TextHandler singleton;
    public GameObject textCanvas;

    //public TextMeshProUGUI textContainer;
    //public TextMeshProUGUI textTitle;
    public Text textContainer;
    public Text textTitle;
    public GameObject nextButton;
    public GameObject prevButton;

    public List<ActionInput> textPaths;
    int currentTextId = 0;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
    }
    public void SetUpTextHandler(List<ActionInput> textPaths, Vector3 spawnPosition)
    {
        if (textPaths == null || textPaths.Count == 0)
            return;

        if (GlobalSettings.currentActiveMetadataHandler != null && GlobalSettings.currentActiveMetadataHandler != this.gameObject)
        {
            GlobalSettings.currentActiveMetadataHandler.SendMessage("Close");
        }
        GlobalSettings.currentActiveMetadataHandler = this.gameObject;

        GameObject tempPlayer = new GameObject();
        tempPlayer.transform.position = Camera.main.transform.position;
        Vector3 tempRotation = Camera.main.transform.rotation.eulerAngles;
        tempRotation.x = 0;
        tempRotation.z = 0;
        tempPlayer.transform.rotation = Quaternion.Euler(tempRotation);


        Vector3 spawnPos2 = Camera.main.transform.position + (tempPlayer.transform.forward * 1.5f);
        spawnPos2.y = 1.7f;
        spawnPos2.z = 4.5f;
        textCanvas.transform.position = spawnPos2;
        Destroy(tempPlayer);

        textCanvas.SetActive(true);
      

        this.textPaths = textPaths;
        if (textPaths.Count <= 1)
        {
            nextButton.SetActive(false);
            prevButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
            prevButton.SetActive(true);
        }

        currentTextId = 0;
        TextAsset textAsset = Resources.Load<TextAsset>(Path.Combine("FileMultimediali", "testi", textPaths[0].path));

        if (textAsset != null)
        {
            textContainer.text = textAsset.text;
            textTitle.text = textPaths[0].path;
        }



    }


    public void NextImage()
    {
        currentTextId = ((currentTextId + 1) % (textPaths.Count));
        textContainer.text = Resources.Load<TextAsset>(Path.Combine("FileMultimediali", "Testi", textPaths[currentTextId].path)).text;
        textTitle.text = textPaths[currentTextId].path;
    }

    public void PrevImage()
    {
        currentTextId--;
        if (currentTextId < 0)
            currentTextId = textPaths.Count - 1;

        textContainer.text = Resources.Load<TextAsset>(Path.Combine("FileMultimediali", "Testi", textPaths[currentTextId].path)).text;
        textTitle.text = textPaths[currentTextId].path;
    }

    public void Close()
    {
        textCanvas.SetActive(false);
    }
}
