using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class SDD_Collider_Deformator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        if (spriteRenderer != null && polygonCollider != null)
        {
            AddCornersToCollider();
        }
        else
        {
            Debug.LogError("SpriteRenderer or PolygonCollider2D is missing on this GameObject.");
        }
    }

    /// <summary>
    /// Adds corner points to the PolygonCollider2D based on the SpriteRenderer's bounds.
    /// </summary>
    private void AddCornersToCollider()
    {
        Bounds bounds = spriteRenderer.bounds;

        // Calculate corners in world space
        Vector2 topLeft = new Vector2(bounds.min.x, bounds.max.y);
        Vector2 topRight = new Vector2(bounds.max.x, bounds.max.y);
        Vector2 bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        Vector2 bottomLeft = new Vector2(bounds.min.x, bounds.min.y);

        // Convert world space to local space (since PolygonCollider2D uses local coordinates)
        Vector2[] corners = new Vector2[]
        {
            transform.InverseTransformPoint(topLeft),
            transform.InverseTransformPoint(topRight),
            transform.InverseTransformPoint(bottomRight),
            transform.InverseTransformPoint(bottomLeft)
        };

        // Apply the corners to the PolygonCollider2D
        polygonCollider.pathCount = 1; // Single path for a closed shape
        polygonCollider.SetPath(0, corners);

        Debug.Log("Corners added to PolygonCollider2D.");
    }
}
