using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas))]
public class InteractableEventTrigger : EventTrigger
{
    private int idScene = 0;
    void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void SetPointer(int idScene) {
        if(!gameObject.tag.Equals("Teleport")) {
            Debug.LogError("Not a teleport");
            return;
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => {SetScene(idScene);});
        triggers.Add(entry);
    }

    public void SetPointer() {
        if(!gameObject.tag.Equals("Interactable")) {
            Debug.LogError("Not an interactable object (i.e.: experiment)");
            return;
        }
    }

    private void OnPointerEnter() {
       // GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f,0f,0f));
    }

    private void OnPointerExit() {
        //GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1f,1f,1f));
    }

    private void OnPointerClick() {
        //GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(0f,0f,1f));
        //FindObjectOfType<SceneManagerScript>().StartScene(idScene);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        OnPointerEnter();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        OnPointerExit();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        OnPointerClick();
    }

    public void SetScene(int id) {
        idScene = id;
        OnPointerClick();
    }

    private void OnMouseEnter()
    {
        OnPointerEnter(null);
    }

    private void OnMouseExit()
    {
        OnPointerExit(null);
    }

    private void OnMouseUp()
    {
        OnPointerClick(null);
    }

}
