using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTriggerArea : MonoBehaviour {

    [SerializeField]
    private float eventRadius;
    [SerializeField]
    private LayerMask layerM;
    [SerializeField]
    private UnityEvent functionToCall;
    
    private bool isTriggered = false;



    // Update is called once per frame
    void Update()
    {

        if (Physics.OverlapSphere(transform.position, eventRadius, layerM).Length > 0)
        {
            functionToCall.Invoke();
            isTriggered = true;
        }




        if (isTriggered)
            Destroy(gameObject);

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, eventRadius);
    }
}
