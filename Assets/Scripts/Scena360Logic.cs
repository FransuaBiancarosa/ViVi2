using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scena360Logic : MonoBehaviour
{

    private ScenaFoto360 scenaFoto360DaCostruire;
    private GuestInfo guestInfo;
    private Vector3 clientPosition;
    private Vector3 hostposition;
    [SerializeField] private GameObject telecamera;

    IEnumerator Start()
    {
        scenaFoto360DaCostruire = (GlobalSettings.scenaDaCostruire as ScenaFoto360);
        if (scenaFoto360DaCostruire != null)
        {
            //riposiziona i player
            if (scenaFoto360DaCostruire.hostSpawnPos != null && scenaFoto360DaCostruire.hostSpawnPos.Count == 3)
            {
                hostposition = GlobalSettings.FloatListToVector3(scenaFoto360DaCostruire.hostSpawnPos);
                GlobalSettings.PositionHost = hostposition;
            }

            if (scenaFoto360DaCostruire.guestInfos != null && scenaFoto360DaCostruire.guestInfos.Count >0)
            {
                guestInfo = scenaFoto360DaCostruire.guestInfos[0];
                if (guestInfo != null)
                {
                    if (guestInfo.posizione != null && guestInfo.posizione.Count == 3)
                    {
                        clientPosition = GlobalSettings.FloatListToVector3(guestInfo.posizione);
                        GlobalSettings.PositionClient = clientPosition;
                    }
                }
            }
            GlobalSettings.playerController.SetAvatarPosition(); 

            //setta lo skybox
            if(GlobalSettings.IsHost)
            {
                Material skyboxMaterial =   Resources.Load<Material>(Path.Combine("360View material", "xenon_pdv1_material"));
                skyboxMaterial.mainTexture= Resources.Load<Texture>(Path.Combine("360Texture", scenaFoto360DaCostruire.hostSfondo));

                RenderSettings.skybox = Resources.Load<Material>(Path.Combine("360View material", "xenon_pdv1_material"));
            }
            else
            {
                Material skyboxMaterial = Resources.Load<Material>(Path.Combine("360View material", "xenon_pdv1_material"));
                skyboxMaterial.mainTexture = Resources.Load<Texture>(Path.Combine("360Texture", guestInfo.sfondo));

                RenderSettings.skybox = Resources.Load<Material>(Path.Combine("360View material", "xenon_pdv1_material"));
            }
            DynamicGI.UpdateEnvironment();

            //setup cameraprefab per host
            if (GlobalSettings.IsHost)
            {
                GameObject go = Instantiate(telecamera);
                go.transform.position = clientPosition - new Vector3(0, 1.7f, 0);
                SceneManager.MoveGameObjectToScene(go, this.gameObject.scene);
                go.transform.LookAt(hostposition);
            }
            //setta i teleport
            yield return StartCoroutine(SetUpTeleportPoints());

            //metti gli oggetti aumentati in scena
            yield return StartCoroutine(SetUpAugmentedObject());

        }

        //fade in
        yield return new WaitForSeconds(1f);
        SceneManagerScript.singleton.FadeIn();
        yield return new WaitForSeconds(2f);

        Debug.Log("360 foto scene loaded");
    }

    IEnumerator SetUpTeleportPoints()
    {
        for (int i = 0; i < scenaFoto360DaCostruire.teletrasporti.Count; i++)
        {
            Teletrasporto teletrasporto = scenaFoto360DaCostruire.teletrasporti[i];
            GameObject teletrasportoPrefab = Resources.Load<GameObject>(Path.Combine("TeleportPointPrefabs", teletrasporto.gobject));
            GameObject instanciatedTeleport = Instantiate(teletrasportoPrefab);
            instanciatedTeleport.transform.position = GlobalSettings.FloatListToVector3(teletrasporto.posizione);
            instanciatedTeleport.transform.rotation = Quaternion.Euler(GlobalSettings.FloatListToVector3(teletrasporto.rotazione));
            instanciatedTeleport.transform.localScale = GlobalSettings.FloatListToVector3(teletrasporto.scala);
            TeleportToSceneScript teleportToSceneScript = instanciatedTeleport.GetComponent<TeleportToSceneScript>();
            if (teleportToSceneScript != null)
            {
                teleportToSceneScript.teleportDestination = teletrasporto.destinazione;
            }
            SceneManager.MoveGameObjectToScene(instanciatedTeleport, this.gameObject.scene);
            yield return null;
        }
        yield return null;
    }

    IEnumerator SetUpAugmentedObject()
    {
        for (int i = 0; i < scenaFoto360DaCostruire.oggettiAumentati.Count; i++)
        {
            OggettoAumentato oggettoAumentato = scenaFoto360DaCostruire.oggettiAumentati[i];
            if(!oggettoAumentato.network || (GlobalSettings.IsHost && oggettoAumentato.network)) {
                switch (oggettoAumentato.tipo)
                { 
                    case "oggetto3D": Instanciate3dAugmetedObject(oggettoAumentato); break;
                    case "icona": Instanciate2DAugmetedObject(oggettoAumentato); break;
                            
                }
            }
            yield return null;
        }

        yield return null;
    }

    void Instanciate3dAugmetedObject(OggettoAumentato oggettoAumentato)
    {

        GameObject oggettoAumentatoPrefab = Resources.Load<GameObject>(Path.Combine("AugmentedObject", "3D", oggettoAumentato.gobject));
        GameObject oggettoAumentatoInstanziato = Instantiate(oggettoAumentatoPrefab);
        oggettoAumentatoInstanziato.transform.position = GlobalSettings.FloatListToVector3(oggettoAumentato.posizione);
        oggettoAumentatoInstanziato.transform.rotation = Quaternion.Euler(GlobalSettings.FloatListToVector3(oggettoAumentato.rotazione));
        oggettoAumentatoInstanziato.transform.localScale = GlobalSettings.FloatListToVector3(oggettoAumentato.scala);


        for(int i=0; i<oggettoAumentato.puntiDiInterazione.Count;i++)
        {
            PuntoDiInterazione puntoDiInterazione = oggettoAumentato.puntiDiInterazione[i];
            GameObject actionPointLabelprefab = Resources.Load<GameObject>(Path.Combine("AugmentedObject", "ActionCanvas"));
            GameObject instanciatedActionPoint = Instantiate(actionPointLabelprefab);
            instanciatedActionPoint.transform.parent=oggettoAumentatoInstanziato.transform;
            instanciatedActionPoint.transform.localPosition = GlobalSettings.FloatListToVector3(puntoDiInterazione.posizione);
            instanciatedActionPoint.transform.rotation = Quaternion.Euler(GlobalSettings.FloatListToVector3(puntoDiInterazione.rotazione));

            instanciatedActionPoint.GetComponent<HandleActionLabel>().SetAction(puntoDiInterazione.azioni, oggettoAumentatoInstanziato);
        }


        if (GlobalSettings.IsHost && oggettoAumentato.network)
            NetworkServer.Spawn(oggettoAumentatoInstanziato);

        SceneManager.MoveGameObjectToScene(oggettoAumentatoInstanziato, this.gameObject.scene);
        //oggettoAumentatoInstanziato.transform.parent = transform;
    }

    void Instanciate2DAugmetedObject(OggettoAumentato oggettoAumentato)
    {
        //l'icona non è altro che un punto di interazione senza parent nella logica attuale
        for (int i = 0; i < oggettoAumentato.puntiDiInterazione.Count; i++)
        {
            PuntoDiInterazione puntoDiInterazione = oggettoAumentato.puntiDiInterazione[i];
            GameObject actionPointLabelprefab = Resources.Load<GameObject>(Path.Combine("AugmentedObject", "ActionCanvas"));
            GameObject instanciatedActionPoint = Instantiate(actionPointLabelprefab);
            instanciatedActionPoint.transform.localPosition = GlobalSettings.FloatListToVector3(oggettoAumentato.posizione);
            instanciatedActionPoint.transform.rotation = Quaternion.Euler(GlobalSettings.FloatListToVector3(oggettoAumentato.rotazione));

            instanciatedActionPoint.GetComponent<HandleActionLabel>().SetAction(puntoDiInterazione.azioni);
            //icone si suppone siano locali
            /*
            if (GlobalSettings.IsHost)
                NetworkServer.Spawn(instanciatedActionPoint);*/

            SceneManager.MoveGameObjectToScene(instanciatedActionPoint, this.gameObject.scene);
        }


      
    }
}
