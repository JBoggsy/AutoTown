using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{

    /*
        * Find an object at the cursor position in world space which has a component of the specified
        * generic type. Returns that component from that object. Or null if the criteria are not met.
        * 
        * Only considers objects with collider components. Priority between overlapping objects is not
        * defined.
        * 
        * Uses the main camera.
        */
    public static T GetObjectUnderCursor<T>() where T : Component
    {
        Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] all_colliders = Physics2D.OverlapPointAll(mouse_position);

        foreach (Collider2D collider in all_colliders)
        {
            T component = collider.GetComponent<T>();

            if (component != null)
            {
                return component;
            }
        }

        return default;
    }

    /*
        * Sample perlin noise at multiple frequencies specified in <fourier_series>. This is sometimes
        * refered to as "fractal noise".
        * 
        * param <offset> acts as a deterministic seed.
        */
    public static float PerlinNoiseMultisample(float x, float y, List<(float freq, float mag)> fourier_series, float offset)
    {
        float sample_acc = 0f;
        float weight_acc = 0f;

        foreach ((float freq, float mag) in fourier_series)
        {
            sample_acc += mag * Mathf.PerlinNoise(x * freq + offset, y * freq + offset);
            weight_acc += mag;
        }

        return sample_acc / weight_acc;
    }

    public static int RandomInt(int upper_bound)
    {
        /*
            * All properties and methods of UnityEngine.Random have an inclusive upper bound. System.Random does not have
            * this issue but I want to stick to using one.
            */
        float sample = upper_bound * Random.value;
        return sample == upper_bound ? 0 : Mathf.FloorToInt(sample);
    }

    public static T RandomElement<T>(List<T> list)
    {
        if (list.Count == 0) { return default; }

        int index = RandomInt(list.Count);
        return list[index];
    }

    public static T RandomElement<T>(System.Array array)
    {
        return (T)array.GetValue(RandomInt(array.Length));
    }
}
