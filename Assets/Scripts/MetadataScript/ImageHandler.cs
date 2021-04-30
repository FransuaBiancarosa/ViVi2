﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageHandler : MonoBehaviour
{
    public static ImageHandler singleton;
    public GameObject imageCanvas;

    public RawImage imagePlayer;
    public TextMeshProUGUI audioTitle;
    public GameObject nextButton;
    public GameObject prevButton;

    public List<ActionInput> imagePaths;
    int currentImageId = 0;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
    }
    public void SetUpImageHandler(List<ActionInput> imagePaths, Vector3 spawnPosition)
    {
        if (imagePaths == null || imagePaths.Count == 0)
            return;

        imageCanvas.SetActive(true);
        imageCanvas.transform.position = spawnPosition;

        this.imagePaths = imagePaths;
        if (imagePaths.Count <= 1)
        {
            nextButton.SetActive(false);
            prevButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
            prevButton.SetActive(true);
        }

        currentImageId = 0;

        imagePlayer.texture = Resources.Load<Texture>(Path.Combine("FileMultimediali","Immagini", imagePaths[0].path));
       
        audioTitle.text = imagePaths[0].path;

    }


    public void NextImage()
    {
        currentImageId = ((currentImageId + 1) % (imagePaths.Count));
        imagePlayer.texture = Resources.Load<Texture>(Path.Combine("FileMultimediali", "Immagini", imagePaths[currentImageId].path));
        audioTitle.text = imagePaths[currentImageId].path;
    }

    public void PrevImage()
    {
        currentImageId--;
        if (currentImageId < 0)
            currentImageId = imagePaths.Count - 1;

        imagePlayer.texture = Resources.Load<Texture>(Path.Combine("FileMultimediali", "Immagini", imagePaths[currentImageId].path));
        audioTitle.text = imagePaths[currentImageId].path;
    }

    public void Close()
    {     
        imageCanvas.SetActive(false);
    }
}