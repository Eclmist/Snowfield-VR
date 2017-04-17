using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotController : MonoBehaviour
{

    [SerializeField]
    private LayerMask interactableLayer;
    private Rigidbody attachedPoint;
    private GameObject interactableObject;
    private GameObject interactedObject;
    [SerializeField]
    bool reset;
    void Awake()
    {
        attachedPoint = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Control();
        ControllerInput();
    }

    private void Control()
    {
        if (reset)
            attachedPoint.velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
            attachedPoint.AddForce(Vector3.left * 10);
        if (Input.GetKey(KeyCode.D))
            attachedPoint.AddForce(Vector3.right * 10);
        if (Input.GetKey(KeyCode.S))
            attachedPoint.AddForce(Vector3.down * 10);
        if (Input.GetKey(KeyCode.W))
            attachedPoint.AddForce(Vector3.up * 10);
        if (Input.GetKey(KeyCode.Q))
            attachedPoint.AddForce(Vector3.forward * 10);
        if (Input.GetKey(KeyCode.E))
            attachedPoint.AddForce(Vector3.back * 10);
        if (Input.GetKeyUp(KeyCode.Space))
        {
            attachedPoint.AddForce(Vector3.up * 200);
        }
    }

    private void ControllerInput()
    {
        if (interactedObject == null && interactableObject != null&&Input.GetKeyDown(KeyCode.X))
        {
            interactedObject = interactableObject;
            IInteractable interactable = interactedObject.GetComponent<IInteractable>();
            interactable.Interact(transform,attachedPoint);
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            IInteractable interactable = interactedObject.GetComponent<IInteractable>();
            Rigidbody rigidBody = interactedObject.GetComponent<Rigidbody>();
            
            if (interactable != null)
            {
                Debug.Log("Exited");
                interactable.StopInteraction(transform);
            }

            rigidBody.velocity = attachedPoint.velocity;
            rigidBody.angularVelocity = attachedPoint.angularVelocity;
            rigidBody.maxAngularVelocity = rigidBody.angularVelocity.magnitude;
            interactedObject = null;
        }
    }
    

    private void OnTriggerEnter(Collider collider)
    {
        if (interactableObject == null && (interactableLayer == (interactableLayer | (1 << collider.gameObject.layer))))
        {
            Debug.Log("EnteredTriggerable");
            interactableObject = collider.gameObject;
        }
        else
        {
            Debug.Log("Entered");
        }
    }

    private void OnTriggerExit(Collider collider)
    {

        if (interactableObject != null && collider.gameObject == interactableObject)
        {
            Debug.Log("ExitTriggerable");
            interactableObject = null;
        }
        else
        {
            Debug.Log("Exited");
        }
    }

}
