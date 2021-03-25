using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    [Range(15,180)]
    public float rotateSpeed = 45;
    private float distance;
    private float height;
    private float radius;
    private float angle;
    private float xAngle;

    private void Start()
    {
        radius = transform.position.magnitude;
        angle = 270 * Mathf.Deg2Rad;
        xAngle = transform.rotation.eulerAngles.x * Mathf.Deg2Rad;
    }

    void Update()
    {
        int direction = 0;
        if (Input.GetKey(KeyCode.J))
            direction -= 1;
        if (Input.GetKey(KeyCode.L))
            direction += 1;
        int rise = 0;
        if (Input.GetKey(KeyCode.I))
            rise += 1;
        if (Input.GetKey(KeyCode.K))
            rise -= 1;

        xAngle += rotateSpeed * rise * Time.deltaTime * Mathf.Deg2Rad;
        xAngle = Mathf.Clamp(xAngle, 0, 90 * Mathf.Deg2Rad);
        angle += rotateSpeed * direction * Time.deltaTime * Mathf.Deg2Rad;
        distance = radius * Mathf.Cos(xAngle);
        height = radius * Mathf.Sin(xAngle);
        transform.position = new Vector3(distance * Mathf.Cos(angle), height, distance * Mathf.Sin(angle));
        transform.rotation = Quaternion.Euler(xAngle * Mathf.Rad2Deg, -(angle * Mathf.Rad2Deg - 270), 0);
    }
}
