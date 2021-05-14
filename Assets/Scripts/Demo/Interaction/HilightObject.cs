using UnityEngine;

[RequireComponent(typeof(InteractableEventTrigger))]
public class HilightObject : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;
    private MeshRenderer meshRenderer;
    private void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Highlight() {
        meshRenderer.material = highlightMaterial;
    }

    public void Restore() {
        meshRenderer.material = defaultMaterial;
    }
}