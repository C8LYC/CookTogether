using UnityEngine;

public class BlockInfoUI : MonoBehaviour
{
    public Transform target3DObject;     // The 3D object to follow

    private Camera mainCamera;
    private float blockLength = 1.0f;
    private Vector3 offset = new Vector3(0.0f, 0.6f, 0.6f); // on the edge of the block

    private RectTransform rectTransform;

    [SerializeField] private RectTransform progressBarTransform;

    public void SetProgress(float progress)
    {
        progressBarTransform.localScale = new Vector3(progress, progressBarTransform.localScale.y, progressBarTransform.localScale.z);
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (target3DObject != null)
        {
            Vector3 uiLeftPoint  = mainCamera.WorldToScreenPoint(target3DObject.position + new Vector3(-blockLength * 0.5f, blockLength * 0.6f, blockLength * 0.6f));
            Vector3 uiRightPoint = mainCamera.WorldToScreenPoint(target3DObject.position + new Vector3( blockLength * 0.5f, blockLength * 0.6f, blockLength * 0.6f));
            Vector3 screenPos = mainCamera.WorldToScreenPoint(target3DObject.position + new Vector3(0.0f, blockLength * 0.6f, blockLength * 0.6f));
            float uiWidth = Vector3.Distance(uiLeftPoint, uiRightPoint);

            Vector2 size = rectTransform.sizeDelta;
            size.x = uiWidth * 1.5f;
            rectTransform.sizeDelta = size;
            
            rectTransform.position = screenPos;
        }
    }
}
