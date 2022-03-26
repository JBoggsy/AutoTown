using UnityEngine;

public static class Util
{
    /**
     * Get the object at the current cursor position in the main camera. Only objects with colliders are considered.
     */
    public static GameObject GetObjectUnderCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }
}