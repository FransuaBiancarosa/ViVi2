using RootMotion.FinalIK;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    [SerializeField] private Transform Head;
    [SerializeField] private bool clone;
    //[SerializeField] private GameObject CameraRig;
    private VRIK vrik;

    public bool Clone { get => clone; }

    void Awake()
    {
        vrik = GetComponent<VRIK>();
        //CameraRig = GameObject.Find("OVR Character");
    }

    private void Start()
    {
        if (!clone)
            HideHead();
        if (GlobalSettings.IsHost && clone)
            Destroy(gameObject);
        
    }

    public void HideHead() {
        //Head.localScale = new Vector3(0,0,0);
    }

    /*
//used by the remote host avatar when the server gets started
    public void SetVRIK(Transform head, Transform lHand, Transform rHand) {
        vrik.solver.spine.headTarget = head;
        vrik.solver.leftArm.target = lHand;
        vrik.solver.rightArm.target = rHand;
        CameraRig.GetComponent<LocalPlayerController>().SwitchRig(isOne: true);
    }

//used by the local avatar when the clients (not host) start (no connection is required)
    public void SetVRIK() {
        HideHead();
        VRIKAnchor[] anchors = CameraRig.GetComponentsInChildren<VRIKAnchor>();
        vrik.solver.spine.headTarget = anchors[0].transform;
        vrik.solver.leftArm.target = anchors[1].transform;
        vrik.solver.rightArm.target = anchors[2].transform;
        if(!GlobalSettings.IsHost)
            CameraRig.GetComponent<LocalPlayerController>().SwitchRig(isOne: false);
    }
    */
}
