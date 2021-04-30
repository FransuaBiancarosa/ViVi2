using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField] private OVRScreenFade fade;
    
    public void FadeIn() {
        if(fade != null)
            fade.FadeIn();
    }
    
    public void FadeOut() {
        if(fade != null)
            fade.FadeOut();
    }
    
    
}
