using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailEventArea : MonoBehaviour {

    [SerializeField]
    private float eventRadius;
    [SerializeField]
    private MessageManager.Mail mail;
    private bool isTriggered = false;

	
	// Update is called once per frame
	void Update ()
    {
        foreach(Collider col in Physics.OverlapSphere(transform.position, eventRadius))
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                MessageManager.Instance.SendMail(mail.Title, mail.Message, mail.Clip);
                isTriggered = true;
            }

           
        }



        if (isTriggered)
            Destroy(gameObject);

	}

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,eventRadius);
    }
}
