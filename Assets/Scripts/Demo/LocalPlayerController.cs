using System.IO;
using UnityEngine;
using UnityEngine.UI;

//Added to the OVR Character
public class LocalPlayerController : MonoBehaviour
{
    #region Serialized Field
    [SerializeField]
    private GameObject PlayerOneView;
    [SerializeField]
    private GameObject PlayerOnePlaceholder;

    [SerializeField]
    private GameObject PlayerTwoView;
    [SerializeField]
    private GameObject PlayerTwoPlaceholder;

    [SerializeField]
    private GameObject CenterCameraAnchor;

    [SerializeField]
    private GameObject CameraPivot;
    [SerializeField]
    private GameObject Avatar;
    #endregion

    #region Private Fields
    private bool isOne = true;
    private int player1Mask, player2Mask;
    private Vector3 PlayerOnePosition;
    private Vector3 PlayerTwoPosition;
    private Camera OculusCamera;
    #endregion

    private void Awake()
    {
        /*PlayerTwoView.SetActive(false);
        PlayerOnePosition = PlayerOnePlaceholder.transform.position + transform.position;
        PlayerTwoPosition = PlayerTwoPlaceholder.transform.position + transform.position;*/
        GlobalSettings.playerController = this;

        OculusCamera = CenterCameraAnchor.GetComponent<Camera>();
        player1Mask = OculusCamera.cullingMask | (1 << LayerMask.NameToLayer("Camera1Only"));
        player2Mask = OculusCamera.cullingMask | (1 << LayerMask.NameToLayer("Camera2Only"));
        OculusCamera.cullingMask = player1Mask;
    }

    private void Start()
    {

        //SetAvatarPosition();
        //Avatar.GetComponent<AvatarController>().SetVRIK();
    }

    public void SetAvatarPosition()
    {
        CameraPivot.transform.position = GlobalSettings.IsHost ? GlobalSettings.PositionHost : GlobalSettings.PositionClient;
        //SwitchRig(GlobalSettings.IsHost);
    }

    void Update()
    {
        /*
        if (OVRInput.GetUp(OVRInput.Button.One,OVRInput.Controller.RTouch) || OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch) || Input.GetKeyUp(KeyCode.S))
        {
            SwitchRig();
            Debug.Log("Rig Switched");
        }*/
    }

/*
    private void SwitchRig()
    {
        isOne = !isOne;
        if (isOne)
        {
            //PlayerTwoView.SetActive(false);
            //PlayerOneView.SetActive(true);
            CameraPivot.transform.position = PlayerOnePosition;
            //OculusCamera.cullingMask = player1Mask;
            RenderSettings.skybox = Resources.Load<Material>(Path.Combine("360View material", "xenon_pdv1_material"));
            DynamicGI.UpdateEnvironment();
        }
        else
        {
            PlayerOneView.SetActive(false);
            PlayerTwoView.SetActive(true);
            CameraPivot.transform.position = PlayerTwoPosition;
            OculusCamera.cullingMask = player2Mask;
        }
    }
*/

//sets the camera mask according to the selected player (host or client)
//TODO rewrite the method so that it takes in input the new position for the camera
    public void SwitchRig(bool isOne)
    {
        if (isOne)
        {
            //PlayerTwoView.SetActive(false);
            //PlayerOneView.SetActive(true);
            //CameraPivot.transform.position = PlayerOnePosition;
            //OculusCamera.cullingMask = player1Mask;
           // RenderSettings.skybox = Resources.Load<Material>(Path.Combine("360View material", "xenon_pdv1_material"));
           // DynamicGI.UpdateEnvironment();
        }
        else
        {
            //PlayerOneView.SetActive(false);
            //PlayerTwoView.SetActive(true);
            //CameraPivot.transform.position = PlayerTwoPosition;
            //OculusCamera.cullingMask = player2Mask;
            //RenderSettings.skybox = Resources.Load<Material>(Path.Combine("360View material", "xenon_pdv2_material"));
            //DynamicGI.UpdateEnvironment();
        }
    }

    public void OnRemotePlayerActivated() {
        if(Avatar != null) {
            if(GlobalSettings.IsHost) {
                Destroy(Avatar);
                /*foreach(var identity in GetComponentsInChildren<VRIKAnchor>()) {
                    identity.gameObject.SetActive(false);
                }*/
            } else {
                //Avatar.transform.parent = transform.parent;
                //transform.position = PlayerTwoPlaceholder.transform.position;
            }
        }
    }
}
