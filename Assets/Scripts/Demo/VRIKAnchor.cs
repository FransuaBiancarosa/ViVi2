using UnityEngine;

public enum AnchorType
{
    Head, Left, Right
}

public class VRIKAnchor : MonoBehaviour
{
    public Transform Anchor;
    public AnchorType Type;

    private void Update()
    {
        if(GlobalSettings.IsHost && Anchor != null)
        {
            Anchor.position = transform.position;
            Anchor.rotation = transform.rotation;
        }
    }
}
