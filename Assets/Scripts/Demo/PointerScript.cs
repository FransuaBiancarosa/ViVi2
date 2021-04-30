using UnityEngine;

public class PointerScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LaserPointer.LaserBeamBehavior laserBeamAppearance;
    [SerializeField] private LaserPointer rightPointer;
    void Start()
    {
        rightPointer.laserBeamBehavior = laserBeamAppearance;
    }
}
