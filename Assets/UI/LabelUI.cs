using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using AillieoUtils.UI;

public class LabelUI : MonoBehaviour
{
    #region Public & Serialized Fields   
    public enum ObjectType { None, Model3D, HotSpot, Animation, Link, Info }
    public bool IsEnabled => m_Canvas.alpha != 0;
    public Transform LabelledObject { get; private set; }

    [Header("GLOBAL SETUP")]
    public float timeToLerp = 1.0f;
    public float speedMultiplier;
    public Vector3 labelOffsetFromCenter;
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private ObjectType objectType;
    [SerializeField]
    private Button iconChildButtonPrefab;

    [Header("ICONS SETUP")]
    [SerializeField]
    private Sprite iconAnimation;
    [SerializeField]
    private Sprite iconLink;
    [SerializeField]
    private Sprite iconInfo;
    [SerializeField]
    private Color colorAnimation, colorLink, colorInfo;


    [Header("OBJECT SETUP")]
    [SerializeField]
    private Sprite iconHotSpot;
    [SerializeField]
    private Sprite icon3DModel;
    [SerializeField]
    private Color colorHotSpot, color3DModel;

    [Header("UI SETUP")]
    [SerializeField]
    private CanvasGroup m_Canvas;
    //icona menu grigio
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Image descriptionImage;
    #endregion

    #region Private Fields
    private Coroutine coroutine = null;
    private GameObject m_CamGO = null;
    private Camera m_camera = null;
    private float m_CamFov = 0.0f;
    private Vector3 labelCenter;
    public RadialLayoutGroup m_RadialLayout;
    #endregion

    #region Unity Lifecycle
    void Awake()
    {
        Initialize();
        SetIcon();
    }

    void Update()
    {
        if (!IsEnabled)
            return;

        MoveLabel(labelOffsetFromCenter);
        SetRadialGrid();
    }
    #endregion

    #region Public Methods
    public void SetLabel(Transform ObjectToLabel, Vector3 offsetFromCenter, ObjectType objectType, string description = "", string title = "")
    {
        m_Canvas.alpha = 0;

        LabelledObject = ObjectToLabel;

        labelCenter = ObjectToLabel.position;

        labelOffsetFromCenter = offsetFromCenter;
        
        if(objectType != ObjectType.None)
            this.objectType = objectType;

        if (titleText != null)
        {
            titleText.text = title;

            if (string.IsNullOrEmpty(title))
                titleText.gameObject.SetActive(false);
            else 
                titleText.gameObject.SetActive(true);
        }

        if (descriptionText != null)
        {
            descriptionText.text = description;

            if (string.IsNullOrEmpty(description))
                descriptionText.gameObject.SetActive(false);
            else
                descriptionText.gameObject.SetActive(true);
        }

        if(!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(description))
            descriptionImage.gameObject.SetActive(true);
        else
            descriptionImage.gameObject.SetActive(false);

      

        SetIcon();

        coroutine = StartCoroutine(FadeCanvas(true));
    }

    public void Disable()
    {
        if(coroutine == null)
        {
            coroutine = StartCoroutine(FadeCanvas(false));
            
        }
        else
        {
            StopCoroutine(coroutine);
            coroutine = null;
            m_Canvas.alpha = 0;
        }

        LabelledObject = null;
    }

    public GameObject SetChilderIcon(ObjectType objectType)
    {
        GameObject instanciatedIcon=GameObject.Instantiate(iconChildButtonPrefab.gameObject, m_RadialLayout.gameObject.transform);
        Image[] instanciatedIconImage = instanciatedIcon.GetComponentsInChildren<Image>();
        instanciatedIcon.AddComponent<CanvasGroup>().alpha=0;

        //altrimenti compaiono i tooltip
        instanciatedIcon.GetComponent<CanvasGroup>().blocksRaycasts=false;
        instanciatedIcon.GetComponent<CanvasGroup>().interactable=false;

        switch (objectType)
        {
            case ObjectType.Model3D:
                instanciatedIconImage[1].sprite = icon3DModel;
                instanciatedIconImage[1].color = color3DModel;
                break;
            case ObjectType.Animation:
                instanciatedIconImage[1].sprite = iconAnimation;
                instanciatedIconImage[1].color = colorAnimation;
                break;
            case ObjectType.Link:
                instanciatedIconImage[1].sprite = iconLink;
                instanciatedIconImage[1].color = colorLink;
                break;
            case ObjectType.Info:
                instanciatedIconImage[1].sprite = iconInfo;
                instanciatedIconImage[1].color = colorInfo;
                break;
        }

        SetRadialGrid();

        return instanciatedIcon;
    }
    #endregion

    #region Private Methods

    private void MoveLabel(Vector3 offset)
    {
        //SetPosition
        Vector3 LabelPos = m_camera.WorldToScreenPoint(labelCenter);

        if (LabelPos.z > 0 && LabelPos.x > 0 && LabelPos.x < Screen.width && LabelPos.y > 0 && LabelPos.y < Screen.height)
        {
            transform.position = LabelPos + offset;
        }
        else
        {
            transform.position = new Vector3 (10000,10000,0);
        }
    }

    private void Initialize()
    {
        m_Canvas = m_Canvas.GetComponent<CanvasGroup>();
        m_Canvas.alpha = 0.0f;       
        m_camera = Camera.main;
        m_CamFov = m_camera.fieldOfView;
        m_RadialLayout = GetComponentInChildren<RadialLayoutGroup>();
    }

    private void SetIcon()
    {
        switch (objectType)
        {
            case ObjectType.Model3D:
                iconImage.sprite = icon3DModel;
                iconImage.color = color3DModel;
                break;
            case ObjectType.HotSpot:
                iconImage.sprite = iconHotSpot;
                iconImage.color = colorHotSpot;
                break;
            case ObjectType.Animation:
                iconImage.sprite = iconAnimation;
                iconImage.color = colorAnimation;
                break;
            case ObjectType.Link:
                iconImage.sprite = iconLink;
                iconImage.color = colorLink;
                break;
            case ObjectType.Info:
                iconImage.sprite = iconInfo;
                iconImage.color = colorInfo;
                break;
        }
    }

    private void SetRadialGrid()
    {
        Button[] m_child = m_RadialLayout.gameObject.GetComponentsInChildren<Button>();

      

        if(m_child.Length == 1)
        {
            m_RadialLayout.AngleStart = 180.0f;
            m_RadialLayout.AngleDelta = 180.0f;
            m_RadialLayout.RadiusStart = 0.0f;
            m_RadialLayout.RadiusDelta = 0.0f;
        }
        if (m_child.Length == 2)
        {
            m_RadialLayout.AngleStart = 180.0f;
            m_RadialLayout.AngleDelta = 180.0f;
            m_RadialLayout.RadiusStart = 30.0f;
            m_RadialLayout.RadiusDelta = 0.0f;
        }

        if (m_child.Length == 3)
        {
            m_RadialLayout.AngleStart = 150.0f;
            m_RadialLayout.AngleDelta = 120.0f;
            m_RadialLayout.RadiusStart = 30.0f;
            m_RadialLayout.RadiusDelta = 0.0f;
        }

        if (m_child.Length == 4)
        {
            m_RadialLayout.AngleStart = 180.0f;
            m_RadialLayout.AngleDelta = 90.0f;
            m_RadialLayout.RadiusStart = 35.0f;
            m_RadialLayout.RadiusDelta = 0.0f;
        }

    }

    private IEnumerator FadeCanvas(bool visible)
    {
        float timeRemaining = timeToLerp;
        float currentAlpha = m_Canvas.alpha;

        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime * speedMultiplier;

            if (visible)
                m_Canvas.alpha = Mathf.Lerp(currentAlpha, 1.0f, Mathf.InverseLerp(timeToLerp, 0, timeRemaining));
            else 
                m_Canvas.alpha = Mathf.Lerp(currentAlpha, 0.0f, Mathf.InverseLerp(timeToLerp, 0, timeRemaining));

            if (timeRemaining > 0)
                yield return null;
        }

        coroutine = null;
    }
    #endregion
}
