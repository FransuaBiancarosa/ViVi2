using UnityEngine;
using Mirror;
using RootMotion.FinalIK;
using UnityEngine.EventSystems;

public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] private Mode host;
    [SerializeField] private Transform OculusQuestController;
    public Mode Host { get => host; }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("Local Player Started");
        //Set the VRIK anchors for the avatar of the host
        VRIK ik = GetComponent<VRIK>();
        VRIKAnchor[] anchors = FindObjectsOfType<VRIKAnchor>();
        foreach(VRIKAnchor anchor in anchors) {
            switch (anchor.Type)
            {
                case AnchorType.Head:
                    ik.solver.spine.headTarget = anchor.transform;
                    break;
                case AnchorType.Left:
                    ik.solver.leftArm.target = anchor.transform;
                    break;
                case AnchorType.Right:
                    ik.solver.rightArm.target = anchor.transform;
                    break;
                default: return;
            }
        }

        //Set the anchor for the OVR Input module
        GameObject.FindObjectOfType<OVRInputModule>().rayTransform = OculusQuestController;
    }
}
