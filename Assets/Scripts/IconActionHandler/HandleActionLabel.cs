using AillieoUtils.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PointOfInterestActions
{
    animazione=0,
    audio,
    video,
    imaggine,
    testo,
    teletrasporto

}
public class HandleActionLabel : MonoBehaviour
{
    public static HandleActionLabel currentActivedLabel;

    public GameObject multiChoiseIcon;
    public GameObject animationPlayStopIcon;
    public GameObject textIcon;
    public GameObject audioIcon;
    public GameObject videoIcon;
    public GameObject imageIcon;
    public GameObject teleportIcon;
    public RadialLayoutGroup radialLayoutGroup;
    public GameObject objectToPlayAnimation;

    List<Azione> azioni;
    //key: actions value: path lists
    Dictionary<string, (GameObject actionButton, List<ActionInput> paths)> avaibleActions = new Dictionary<string, (GameObject actionButton, List<ActionInput> paths)>();

    public void SetAction(List<Azione> azioni,GameObject objectToPlayAnimation=null)
    {
        this.azioni = azioni;
        this.objectToPlayAnimation = objectToPlayAnimation;
        if (azioni.Count == 1)
        {
            multiChoiseIcon.SetActive(false);
            switch (azioni[0].tipo)
            {

                case "animazione":
                    animationPlayStopIcon.SetActive(true);
                    //setta azione dove segni che sei anche quello attivo
                    break;
                case "audio":
                    audioIcon.SetActive(true);
                    //setta azione dove segni che sei anche quello attivo
                    break;
                case "video":
                    videoIcon.SetActive(true);
                    //setta azione dove segni che sei anche quello attivo
                    break;
                case "immagine":
                    imageIcon.SetActive(true);
                    //setta azione dove segni che sei anche quello attivo
                    break;
                case "testo":
                    textIcon.SetActive(true);
                    //setta azione dove segni che sei anche quello attivo
                    break;
                case "teletrasporto":
                    teleportIcon.SetActive(true);
                    //setta azione dove segni che sei anche quello attivo
                    break;
            }
        }
        else
        {
            multiChoiseIcon.SetActive(true);
            //setta che il tasto azioni multiple apre e chiude il menu

            for (int i=0;i<azioni.Count;i++)
            {
                Azione azioneCorrente = azioni[i];
                switch (azioneCorrente.tipo)
                {

                    case "animazione":
                        avaibleActions.Add("animazione", (animationPlayStopIcon, azioneCorrente.input));                       
                        //setta azione dove segni che sei anche quello attivo
                        break;
                    case "audio":
                        avaibleActions.Add("audio", (audioIcon, azioneCorrente.input));
                        //setta azione dove segni che sei anche quello attivo
                        break;
                    case "video":
                        avaibleActions.Add("video", (videoIcon, azioneCorrente.input));
                        //setta azione dove segni che sei anche quello attivo
                        break;
                    case "immagine":
                        avaibleActions.Add("immagine", (imageIcon, azioneCorrente.input));
                        //setta azione dove segni che sei anche quello attivo
                        break;
                    case "testo":
                        avaibleActions.Add("testo", (textIcon, azioneCorrente.input));
                        //setta azione dove segni che sei anche quello attivo
                        break;
                    case "teletrasporto":
                        avaibleActions.Add("teletrasporto", (teleportIcon, azioneCorrente.input));
                        //setta azione dove segni che sei anche quello attivo
                        break;
                }
            }

            SetRadialGrid();
        }


        
    }

    bool visible=false;
    public void ShowOrHideIcon()
    {
        if(!multiChoiseIcon.activeInHierarchy)
        {
            return;
        }

        if (visible)
        {

            currentActivedLabel = null;
            foreach (KeyValuePair<string, (GameObject actionButton, List<ActionInput> paths) > keyValuePair in avaibleActions)
            {
                keyValuePair.Value.actionButton.SetActive(false);
            }
            visible = false;

            //setta che non sei icona attiva
            currentActivedLabel = null;
        }
        else
        {
            //spegni l'icona attiva se non sei tu
            if (currentActivedLabel != null && currentActivedLabel!=this)
            {
                currentActivedLabel.HideIcon();
            }

            foreach (KeyValuePair<string, (GameObject actionButton, List<ActionInput> paths)> keyValuePair in avaibleActions)
            {
                keyValuePair.Value.actionButton.SetActive(true);
            }
            visible = true;

            //settati come icona ativa
            currentActivedLabel = this;
        }
    }

    //chiamata per spegnere dall'esterno le icone in modo da settare bene pure highlight
    public void HideIcon()
    {
        if (visible)
        {

            currentActivedLabel = null;
            foreach (KeyValuePair<string, (GameObject actionButton, List<ActionInput> paths)> keyValuePair in avaibleActions)
            {
                keyValuePair.Value.actionButton.SetActive(false);
            }
            visible = false;
            multiChoiseIcon.GetComponent<IconHighlight>().setSelectedColor();
            //setta che non sei icona attiva
            currentActivedLabel = null;
        }
    }

    private void SetRadialGrid()
    {
        radialLayoutGroup.AngleDelta = 360 /(avaibleActions.Count + 1);//+1 is for multichoice icon

        if ((avaibleActions.Count + 1) > 3)
            radialLayoutGroup.RadiusStart = 15;
        else
            radialLayoutGroup.RadiusStart = 10;

    }

    public void PlayStopAnimationAction()
    {
        Debug.Log("Animation Action");
    }

    public void AudioIconAction()
    {
        Debug.Log("Audio Action");
    }

    public void VideoIconAction()
    {
        Debug.Log("Video Action");
        VideoHandler.singleton.SetUpVideoHandler(avaibleActions["video"].paths, gameObject.transform.position);
    }

    public void ImageIconAction()
    {
        Debug.Log("Image Action");
    }
    public void TextIconAction()
    {
        Debug.Log("Text Action");
    }

    public void TeleportIconAction()
    {
        Debug.Log("Teleport Action");
    }
}
