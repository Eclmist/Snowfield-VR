using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public float moveSpeed, zoomSpeed;

    private Rigidbody rb;

	void Start () 
	{
        rb = gameObject.GetComponent<Rigidbody>();	
	}

    void FixedUpdate()
    {
        Move();
        Zoom();
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * moveSpeed;
    }

    private void Zoom()
    {
        float zoomIn = Input.GetAxis("Fire1");
        float zoomOut = Input.GetAxis("Fire2");

        Vector3 zoom = transform.forward;
             
        if (zoomIn != 0)
        {
            rb.velocity = zoom * zoomSpeed;
        }
        else if (zoomOut != 0)
        {
            rb.velocity = -1 * zoom * zoomSpeed;
        }
    }
}
