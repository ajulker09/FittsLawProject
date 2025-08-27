using UnityEngine;
using Oculus.Interaction; // for InteractorState (optional)

[RequireComponent(typeof(LineRenderer))]
public class RayInteractorLine : MonoBehaviour
{
    [Header("References")]
    public Transform rayOrigin;          // assign PointerPose here
    public float fallbackMaxLength = 1f; // used if no RayInteractor found

    [Header("Visuals")]
    public float rayWidth = 0.007f;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.white;
    public Color selectColor = Color.white;

    private LineRenderer line;
    private RayInteractor rayInteractor; // optional (only for state/color)

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        rayInteractor = GetComponent<RayInteractor>(); // ok if null

        // Line setup
        line.positionCount = 2;
        line.startWidth = rayWidth;
        line.endWidth = rayWidth;

        // Simple unlit material so it looks like the debug gizmo
        var mat = new Material(Shader.Find("Unlit/Color"));
        line.material = mat;
        line.enabled = true;
    }

    void Update()
    {
        if (rayOrigin == null)
        {
            // Fallback to this transform if PointerPose not assigned
            rayOrigin = transform;
        }

        Vector3 start = rayOrigin.position;
        Vector3 dir = rayOrigin.forward;

        float maxLen = fallbackMaxLength;
        if (rayInteractor != null)
        {
            // If available, use the interactor's max length
            maxLen = rayInteractor.MaxRayLength;
        }

        // Raycast to find the end point
        Vector3 end = start + dir * maxLen;
        if (Physics.Raycast(start, dir, out RaycastHit hit, maxLen))
        {
            end = hit.point;

            // Color by state (if we have the interactor), otherwise treat as hover
            if (rayInteractor != null && rayInteractor.State == InteractorState.Select)
                line.material.color = selectColor;
            else
                line.material.color = hoverColor;
        }
        else
        {
            // No hit: normal color to full length
            line.material.color = normalColor;
        }

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
