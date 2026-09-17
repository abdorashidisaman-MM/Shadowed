using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ShadowSurface : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private ShadowSurface leftConnection;
    [SerializeField] private ShadowSurface rightConnection;

    private BoxCollider surfaceCollider;


    public Vector3 Normal
    {
        get
        {
            return transform.forward.normalized;
        }
    }


    public Vector3 Right
    {
        get
        {
            return Vector3.Cross(
                Normal,
                Vector3.up
            ).normalized;
        }
    }


    private void Awake()
    {
        surfaceCollider =
            GetComponent<BoxCollider>();
    }


    public ShadowSurface GetConnection(
        float horizontalInput)
    {
        if (horizontalInput > 0f)
        {
            return rightConnection;
        }

        return leftConnection;
    }


    public bool TrySnapPosition(
        Vector3 desiredWorldPosition,
        float surfaceOffset,
        out Vector3 snappedPosition)
    {
        EnsureCollider();


        Vector3 localPosition =
            transform.InverseTransformPoint(
                desiredWorldPosition
            );


        Vector3 center =
            surfaceCollider.center;

        Vector3 halfSize =
            surfaceCollider.size * 0.5f;


        float minX =
            center.x - halfSize.x;

        float maxX =
            center.x + halfSize.x;

        float minY =
            center.y - halfSize.y;

        float maxY =
            center.y + halfSize.y;


        const float tolerance = 0.01f;


        bool insideHorizontalBounds =
            localPosition.x >= minX - tolerance &&
            localPosition.x <= maxX + tolerance;


        bool insideVerticalBounds =
            localPosition.y >= minY - tolerance &&
            localPosition.y <= maxY + tolerance;


        if (!insideHorizontalBounds ||
            !insideVerticalBounds)
        {
            snappedPosition = default;
            return false;
        }


        localPosition.x =
            Mathf.Clamp(
                localPosition.x,
                minX,
                maxX
            );


        localPosition.y =
            Mathf.Clamp(
                localPosition.y,
                minY,
                maxY
            );


        // +Z is the playable face.
        localPosition.z =
            center.z + halfSize.z;


        Vector3 surfacePoint =
            transform.TransformPoint(
                localPosition
            );


        snappedPosition =
            surfacePoint +
            Normal * surfaceOffset;


        return true;
    }


    public Vector3 GetEntryPosition(
        Vector3 referenceWorldPosition,
        bool enteringFromLeft,
        float surfaceOffset,
        float edgeInset)
    {
        EnsureCollider();


        Vector3 localPosition =
            transform.InverseTransformPoint(
                referenceWorldPosition
            );


        Vector3 center =
            surfaceCollider.center;

        Vector3 halfSize =
            surfaceCollider.size * 0.5f;


        float minX =
            center.x - halfSize.x;

        float maxX =
            center.x + halfSize.x;

        float minY =
            center.y - halfSize.y;

        float maxY =
            center.y + halfSize.y;


        // Convert our world-space inset into
        // approximately local-space units.
        float scaleX =
            Mathf.Abs(transform.lossyScale.x);

        float localInset =
            scaleX > 0.0001f
                ? edgeInset / scaleX
                : 0f;


        // Figure out whether our calculated
        // surfaceRight corresponds to +localX
        // or -localX.
        float localRightAlignment =
            Vector3.Dot(
                Right,
                transform.right.normalized
            );


        bool rightIsPositiveLocalX =
            localRightAlignment > 0f;


        if (enteringFromLeft)
        {
            localPosition.x =
                rightIsPositiveLocalX
                ? minX + localInset
                : maxX - localInset;
        }
        else
        {
            localPosition.x =
                rightIsPositiveLocalX
                ? maxX - localInset
                : minX + localInset;
        }


        localPosition.x =
            Mathf.Clamp(
                localPosition.x,
                minX,
                maxX
            );


        localPosition.y =
            Mathf.Clamp(
                localPosition.y,
                minY,
                maxY
            );


        localPosition.z =
            center.z + halfSize.z;


        Vector3 surfacePoint =
            transform.TransformPoint(
                localPosition
            );


        return
            surfacePoint +
            Normal * surfaceOffset;
    }


    private void EnsureCollider()
    {
        if (surfaceCollider == null)
        {
            surfaceCollider =
                GetComponent<BoxCollider>();
        }
    }
}