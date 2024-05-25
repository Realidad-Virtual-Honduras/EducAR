using UnityEngine;
using UnityEngine.UI;

public class TestTouch : MonoBehaviour
{
    private TouchMananger touchMananger;
    [SerializeField] private Camera cam;
    [SerializeField] private Image dotImage;

    void Awake()
    {
        touchMananger = TouchMananger.Instance;
        cam = Camera.main;
    }

    private void OnEnable()
    {
        touchMananger.OnStartTouch += Move;
    }

    private void OnDisable()
    {
        touchMananger.OnEndTouch -= Move;
    }

    public void Move(Vector2 screenPos, float time)
    {
        Vector3 screenCoordinates = new Vector3 (screenPos.x, screenPos.y, cam.nearClipPlane);
        Vector3 worldCoordinates = cam.ScreenToWorldPoint(screenCoordinates);

        worldCoordinates.z = 0;
        dotImage.rectTransform.localPosition = worldCoordinates;
    }
}
