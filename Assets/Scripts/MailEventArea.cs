using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailEventArea : MonoBehaviour {

    [SerializeField]
    private float eventRadius;
    [SerializeField]
    private LayerMask layerM;
    [SerializeField]
    private MessageManager.Mail mail;
    private bool isTriggered = false;



    // Update is called once per frame
    void Update()
    {

        if (Physics.OverlapSphere(transform.position, eventRadius, layerM).Length > 0)
        {
            MessageManager.Instance.SendMail(mail.Title, mail.Message, mail.Clip);
            isTriggered = true;
        }
           



        if (isTriggered)
            Destroy(gameObject);

	}

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,eventRadius);
    }
}
