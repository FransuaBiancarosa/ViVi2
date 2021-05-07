using UnityEngine;
using Mirror;
using RootMotion.FinalIK;
using CrazyMinnow.SALSA.DissonanceLink;

public class Syncer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnPositionChanged))]
    [SerializeField]
    private Vector3 headPos;
    [SyncVar(hook = nameof(OnRotationChanged))]
    [SerializeField]
    private Quaternion headRot;
    
    [SyncVar(hook = nameof(OnPositionChanged))]
    [SerializeField]
    private Vector3 lHandPos;
    [SyncVar(hook = nameof(OnRotationChanged))]
    [SerializeField]
    private Quaternion lHandRot;
    
    [SyncVar(hook = nameof(OnPositionChanged))]
    [SerializeField]
    private Vector3 rHandPos;
    [SyncVar(hook = nameof(OnRotationChanged))]
    [SerializeField]
    private Quaternion rHandRot;
    [SyncVar(hook = nameof(OnSceneChanged))]
    public int sceneId;

    [SerializeField] private Transform headAnchor;
    [SerializeField] private Transform lHandAnchor;
    [SerializeField] private Transform rHandAnchor;
    [SerializeField] private Vector3 headOffset;

    public Transform originHead;
    public Transform originLeft;
    public Transform originRight;

    public static Syncer instance;
    private bool hostStarted = false;
    private Vector3 heightOffset = new Vector3(0, 1.7f, 0);

    private void OnSceneChanged(int oldID, int newID) {
        Debug.Log("OnSceneChanged called");
        DebugVR.SetText($"Hook called with {newID}");
        if(newID < 0)
            return;
        SceneManagerScript sceneManager = FindObjectOfType<SceneManagerScript>();
        StartCoroutine(sceneManager.BuildScene(newID));
    }

    private void Awake()
    {
        Debug.Log("Syncer Awake");
        instance = this;
        sceneId = -1;
        VRIKAnchor[] anchors = FindObjectsOfType<VRIKAnchor>();
        foreach (VRIKAnchor anchor in anchors)
        {
            switch (anchor.Type)
            {
                case AnchorType.Head:
                    anchor.Anchor = headAnchor;
                    break;
                case AnchorType.Left:
                    anchor.Anchor = lHandAnchor;
                    break;
                case AnchorType.Right:
                    anchor.Anchor = rHandAnchor;
                    break;
                default: return;
            }
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Syncer OnStartClient");
        if (!GlobalSettings.IsHost) 
            SetCloneAnchors();
    }

    public void OnHostConnected(Transform head, Transform left, Transform right)
    {
        originHead = head;
        originLeft = left;
        originRight = right;
        hostStarted = true; 
    }

    public void SetCloneAnchors()
    {
        GameObject avatarHost = GameObject.FindObjectOfType<SalsaDissonanceLink>().gameObject;
        VRIK vrik = avatarHost.GetComponent<VRIK>();
        vrik.enabled = false;
    }

    private void Update()
    {
        if (hostStarted && GlobalSettings.IsHost)
        {
            headPos = originHead.position;
            headRot = originHead.rotation;
            lHandPos = originLeft.position;
            lHandRot = originLeft.rotation;
            rHandPos = originRight.position;
            rHandRot = originRight.rotation;
        }
    }

    private void OnPositionChanged(Vector3 oldPos, Vector3 newPos)
    {
        if (!GlobalSettings.IsHost)
        {
            headAnchor.position = headPos + headOffset;
            lHandAnchor.position = lHandPos;
            rHandAnchor.position = rHandPos;
        }
    }

    private void OnRotationChanged(Quaternion oldRot, Quaternion newRot)
    {
        if (!GlobalSettings.IsHost)
        {
            headAnchor.rotation = headRot;
            lHandAnchor.rotation = lHandRot;
            rHandAnchor.rotation = rHandRot;
        }
    }
}
