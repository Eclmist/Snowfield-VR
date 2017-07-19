using UnityEngine;
using System.Collections;
using Edwon.VR.Input;

namespace Edwon.VR.Gesture.Examples
{
    public class IcePower : MonoBehaviour
    {
        Rigidbody rb;

        public float pushForceMult;
        public float timeTillDeath;

        Transform playerHead;
        Transform playerHandL;
        Transform playerHandR;

        VRGestureRig myAvatar;

        void Start()
        {
            myAvatar = FindObjectOfType<VRGestureRig>();
            if (myAvatar == null)
            {
                Debug.Log("there is no VRGestureRig in the scene, please add one");
            }
            rb = GetComponent<Rigidbody>();

            playerHead = myAvatar.head;
            playerHandR = myAvatar.handRight;
            playerHandL = myAvatar.handLeft;

            StartCoroutine(DestroySelf());
        }

        void FixedUpdate()
        {
            // blow enemies back

            Ray handRay = new Ray(playerHandR.position, playerHandR.forward);
            float sphereCastRadius = .5f;
            RaycastHit[] hits;
            hits = Physics.SphereCastAll(handRay, sphereCastRadius);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Transform enemy = hit.collider.transform;

                    // push the enemy back
                    Rigidbody[] rbs = enemy.GetComponentsInChildren<Rigidbody>();
                    Vector3 force = (enemy.position - playerHandR.position).normalized * pushForceMult;
                    foreach (Rigidbody rb in rbs)
                    {
                        rb.AddForce(force, ForceMode.Force);
                    }

                }
            }

            // update particles position
            transform.position = playerHandR.position;
            transform.rotation = playerHandR.rotation;
        }


        IEnumerator DestroySelf()
        {
            yield return new WaitForSeconds(timeTillDeath);
            Destroy(gameObject);
        }

    }
}