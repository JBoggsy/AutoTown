using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMonobehaviour : MonoBehaviour
{
    [Header("Config")]
    public float PanSpeed;
    public float ZoomSpeed;

    public void Update()
    {
        float distance = GetComponent<Camera>().orthographicSize * PanSpeed * Time.deltaTime;
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement += distance * Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement += distance * Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement += distance * Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += distance * Vector2.right;
        }
        gameObject.transform.position += (Vector3)movement;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f )
        {
            GetComponent<Camera>().orthographicSize /= ZoomSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            GetComponent<Camera>().orthographicSize *= ZoomSpeed;
        }
    }
}
