using RootMotion.FinalIK;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    [SerializeField] private bool clone;
    private VRIK vrik;

    public bool Clone { get => clone; }

    void Awake()
    {
        vrik = GetComponent<VRIK>();
    }

    private void Start()
    {
        if (GlobalSettings.IsHost && clone)
            Destroy(gameObject);
        
    }
}
