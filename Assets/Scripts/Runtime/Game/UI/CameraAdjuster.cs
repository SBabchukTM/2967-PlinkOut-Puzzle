using System.Collections;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    [SerializeField] private RectTransform _uiArea;
    [SerializeField] private BoxCollider2D _gameFieldBoxCollider;
    [SerializeField] private Camera cam;
    [SerializeField] private float padding = 0.05f;

    public void Initial(BoxCollider2D collider)
    {
        _gameFieldBoxCollider = collider;
        cam = Camera.main;
        StartCoroutine(AdjustCameraCoroutine());
    }

    private IEnumerator AdjustCameraCoroutine()
    {
        Canvas.ForceUpdateCanvases();
        yield return null;
        yield return new WaitForEndOfFrame();

        if (cam == null || _uiArea == null || _gameFieldBoxCollider == null)
            yield break;

        Vector3[] uiCorners = new Vector3[4];
        _uiArea.GetWorldCorners(uiCorners);

        float uiWorldWidth = Vector3.Distance(uiCorners[0], uiCorners[3]);
        float uiWorldHeight = Vector3.Distance(uiCorners[0], uiCorners[1]);

        Bounds bounds = _gameFieldBoxCollider.bounds;
        float targetWidth = bounds.size.x;
        float targetHeight = bounds.size.y;

        float uiAspect = uiWorldWidth / uiWorldHeight;
        float targetAspect = targetWidth / targetHeight;

        float orthographicSize;

        if (targetAspect > uiAspect)
        {
            float worldHeight = targetWidth / uiAspect;
            orthographicSize = worldHeight / 2f;
        }
        else
        {
            orthographicSize = targetHeight / 2f;
        }

        orthographicSize *= (1 + padding);

        cam.orthographic = true;
        cam.orthographicSize = orthographicSize;

        cam.transform.position = new Vector3(bounds.center.x, bounds.center.y, cam.transform.position.z);
    }
}
