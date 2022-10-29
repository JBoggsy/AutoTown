using System.Collections;
using System.Collections.Generic;
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

    public static float PerlinNoiseMultisample(float x, float y, List<(float freq, float mag)> frequencies, float offset)
    {
        float sample_acc = 0f;
        float weight_acc = 0f;

        foreach ((float freq, float mag) in frequencies)
        {
            sample_acc += mag * Mathf.PerlinNoise(x * freq + offset, y * freq + offset);
            weight_acc += mag;
        }

        return sample_acc / weight_acc;
    }
}