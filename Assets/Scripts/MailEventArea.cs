using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailEventArea : MonoBehaviour {

    [SerializeField]
    private float eventRadius;
    [SerializeField]
    private MessageManager.Mail mail;

	
	// Update is called once per frame
	void Update ()
    {

       if(Physics.OverlapSphere(transform.position, eventRadius,LayerMask.NameToLayer("Player")).Length > 0)
        {
            MessageManager.Instance.SendMail(mail.Title,mail.Message,mail.Clip);
        }

	}

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,eventRadius);
    }
}
