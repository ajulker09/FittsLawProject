using UnityEngine;

public class CircleRingCalibration : MonoBehaviour
{
    public GameObject previewCylinder;
    public CircleRingSpawner ringSpawner;

    private Vector3 offset = Vector3.zero;

    public void IncreaseDistance()
    {
        offset.z += 0.10f;
        UpdatePreview();
    }

    public void DecreaseDistance()
    {
        offset.z -= 0.10f;
        UpdatePreview();
    }

    public void MoveUp()
    {
        offset.y += 0.10f;
        UpdatePreview();
    }

    public void MoveDown()
    {
        offset.y -= 0.10f;
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        previewCylinder.transform.position += offset;
    }

    public void ConfirmAdjustment()
    {
        ringSpawner.center += offset;
        offset = Vector3.zero;
        UpdatePreview();
        Debug.Log("Confirmed new center: " + ringSpawner.center);
    }
}
