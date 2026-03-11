using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    public Transform pupil;
    public float maxDistance = 0.25f;

    // Vision settings
    public float viewAngle = 120f;           // Degrees of field of view
    public float viewDistance = 10f;         // Max distance to see the camera
    public bool useOcclusionCheck = true;    // Raycast check for occluders
    public LayerMask occlusionMask = ~0;     // Layers considered as occluders

    private Transform target;
    private RectTransform pupilRect;

    private void Start()
    {
        // Set the target to the main camera
        target = Camera.main ? Camera.main.transform : null;

        // Cache RectTransform if this pupil is a UI Image (RectTransform)
        if (pupil != null)
            pupilRect = pupil.GetComponentInChildren<RectTransform>();
    }

    private void Update()
    {
        if (target == null || pupil == null)
            return;

        // If the camera is not visible, reset pupil to neutral and skip looking logic
        if (!CanSeeTarget())
        {
            ResetPupil();
            return;
        }

        // World-space vector from eye center to the target (camera)
        Vector3 toTarget = target.position - transform.position;

        // Project that vector onto the eye's local plane so rotation/flip on Y doesn't invert the pupil direction.
        // transform.forward defines the plane normal (eye plane).
        Vector3 projected = Vector3.ProjectOnPlane(toTarget, transform.forward);

        // If projection is degenerate (camera exactly on normal), fallback to raw vector.
        Vector3 useWorldVector = (projected.sqrMagnitude > 1e-6f) ? projected : toTarget;

        // Convert the world-space direction into the eye's local space so we can move the pupil in local x/y.
        Vector3 localVec = transform.InverseTransformDirection(useWorldVector);

        // Build a 2D direction in the local eye plane and clamp magnitude to maxDistance.
        Vector2 localDir = new Vector2(localVec.x, localVec.y);
        if (localDir.sqrMagnitude > 1e-6f)
            localDir = localDir.normalized * Mathf.Min(maxDistance, localDir.magnitude);
        else
            localDir = Vector2.zero;

        // Apply to pupil. If it's a UI element use RectTransform.anchoredPosition; otherwise use localPosition.
        if (pupilRect != null)
        {
            pupilRect.anchoredPosition = localDir;
        }
        else
        {
            pupil.localPosition = new Vector3(localDir.x, localDir.y, pupil.localPosition.z);
        }
    }

    private bool CanSeeTarget()
    {
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;

        // Distance check
        if (distance > viewDistance)
            return false;

        // Angle check (field of view)
        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > viewAngle * 0.5f)
            return false;

        // Occlusion check (optional)
        if (useOcclusionCheck)
        {
            // RaycastAll so we can ignore hits on this character itself
            RaycastHit[] hits = Physics.RaycastAll(transform.position, dir.normalized, distance, occlusionMask, QueryTriggerInteraction.Ignore);
            if (hits.Length == 0)
            {
                // Nothing in between
                return true;
            }

            foreach (var hit in hits)
            {
                // If the hit is part of this character, ignore it
                if (IsPartOfSelf(hit.transform))
                    continue;

                // Any non-self hit before the camera is an occluder
                return false;
            }

            // Only self-hits -> visible
            return true;
        }

        // No occlusion checks and passed distance/angle -> visible
        return true;
    }

    private bool IsPartOfSelf(Transform t)
    {
        if (t == null)
            return false;

        // Consider hits on this transform, its children, or the same root object as part of self
        if (t == transform || t.IsChildOf(transform))
            return true;

        if (transform.root != null && t.root == transform.root)
            return true;

        return false;
    }

    private void ResetPupil()
    {
        if (pupilRect != null)
        {
            pupilRect.anchoredPosition = Vector2.zero;
        }
        else
        {
            pupil.localPosition = new Vector3(0f, 0f, pupil.localPosition.z);
        }
    }
}
