using UnityEngine;
using System.Collections;
using Edwon.VR.Input;

namespace Edwon.VR.Gesture
{
    public class Example2Player : MonoBehaviour
    {
        public GameObject fire;
        public GameObject earth;
        public GameObject ice;
        public GameObject air;

        VRGestureRig rig;
        IInput input;

        Transform playerHead;
        Transform playerHandL;
        Transform playerHandR;

        void Start()
        {
            rig = FindObjectOfType<VRGestureRig>();
            if (rig == null)
            {
                Debug.Log("there is no VRGestureRig in the scene, please add one");
            }

            playerHead = rig.head;
            playerHandR = rig.handRight;
            playerHandL = rig.handLeft;

            input = rig.GetInput(rig.mainHand);
        }

        void Update()
        {

        }

        void OnEnable()
        {
            GestureRecognizer.GestureDetectedEvent += OnGestureDetected;
        }

        void OnDisable()
        {
            GestureRecognizer.GestureDetectedEvent -= OnGestureDetected;
        }

        void OnGestureDetected(string gestureName, double confidence, Handedness hand, bool isDouble)
        {
            string confidenceString = confidence.ToString().Substring(0, 4);
            //Debug.Log("detected gesture: " + gestureName + " with confidence: " + confidenceString);

            switch (gestureName)
            {
                case "Fire":
                    DoFire();
                    break;
                case "Earth":
                    DoEarth();
                    break;
                case "Ice":
                    DoIce();
                    break;
                case "Air":
                    DoAir();
                    break;
                case "Gravity":
                    DoGravity();
                    break;
                case "Pull":
                    DoPull();
                    break;
            }
        }

        void DoFire()
        {
            Quaternion rotation = Quaternion.LookRotation(playerHandR.forward, Vector3.up);
            Vector3 betweenHandsPos = (playerHandL.position + playerHandR.position) / 2;
            GameObject fireInstance = GameObject.Instantiate(fire, betweenHandsPos, rotation) as GameObject;
            StartCoroutine(IEDoFire(fireInstance));
        }

        IEnumerator IEDoFire(GameObject fireInstance)
        {
            yield return new WaitForSeconds(.1f);
            fireInstance.GetComponent<Collider>().enabled = true;
        }

        void DoEarth()
        {

            float explosionForce = 300f;

            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            float floorY = 2.65f;
            Vector3 earthSpawnPosition = new Vector3(playerHandR.position.x, floorY, playerHandR.position.z);
            GameObject.Instantiate(earth, earthSpawnPosition, rotation);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {

                // if it's a ragdoll make non-kinematic
                if (enemy.GetComponent<Ragdoll>() != null)
                {
                    Ragdoll ragdoll = enemy.GetComponent<Ragdoll>();
                    ragdoll.TriggerWarning();
                    foreach (Rigidbody rb in ragdoll.myParts)
                    {
                        rb.AddExplosionForce(explosionForce, earthSpawnPosition, 100000f);
                    }
                }

                else if (enemy.GetComponent<Rigidbody>() != null)
                {
                    Rigidbody rb = enemy.GetComponent<Rigidbody>();
                    rb.AddExplosionForce(explosionForce, earthSpawnPosition, 10f);
                }
            }
        }

        void DoIce()
        {
            GameObject.Instantiate(ice, playerHandR.position, playerHandR.rotation);
        }

        void DoAir()
        {
            float explosionForce = 6f;

            Ray headRay = new Ray(playerHead.position, playerHead.forward);
            float sphereCastRadius = 1f;
            RaycastHit[] hits;
            hits = Physics.SphereCastAll(headRay, sphereCastRadius);
            int hitCounter = 0;
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {

                    Transform enemy = hit.collider.transform;
                    if (hitCounter == 0)
                    {
                        Vector3 airSpawnPosition = enemy.transform.position;
                        GameObject.Instantiate(air, airSpawnPosition, Quaternion.identity);
                    }

                    hitCounter += 1;
                    // spawn the explosion effect

                    // shoot the enemy up into the air

                    // if it's a ragdoll make non-kinematic
                    if (enemy.GetComponent<Ragdoll>() != null)
                    {
                        Ragdoll ragdoll = enemy.GetComponent<Ragdoll>();
                        ragdoll.TriggerWarning();

                        foreach (Rigidbody rb in ragdoll.myParts)
                        {

                            rb.AddForce(new Vector3(.3f, explosionForce * 2, .1f), ForceMode.Impulse);
                            StartCoroutine(IEDoAir(rb));
                        }
                    }
                    else if (enemy.GetComponent<Rigidbody>() != null)
                    {

                        Rigidbody rb = enemy.GetComponent<Rigidbody>();
                        rb.AddForce(new Vector3(.3f, explosionForce, .1f), ForceMode.Impulse);
                        StartCoroutine(IEDoAir(rb));
                    }
                }
            }

        }

        IEnumerator IEDoAir(Rigidbody rb)
        {
            yield return new WaitForSeconds(.15f);
            rb.useGravity = false;
        }

        void DoPull()
        {

            // pull enemies in
            float pullForce = -300f;
            float floorY = 2.65f;
            Vector3 earthSpawnPosition = new Vector3(playerHead.position.x, floorY, playerHead.position.z);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                // if it's a ragdoll make non-kinematic
                if (enemy.GetComponent<Ragdoll>() != null)
                {
                    Ragdoll ragdoll = enemy.GetComponent<Ragdoll>();
                    ragdoll.TriggerWarning();
                    foreach (Rigidbody rb in ragdoll.myParts)
                    {
                        rb.AddExplosionForce(pullForce, earthSpawnPosition, 100000f);
                    }
                }

                else if (enemy.GetComponent<Rigidbody>() != null)
                {
                    Rigidbody rb = enemy.GetComponent<Rigidbody>();
                    rb.AddExplosionForce(pullForce, earthSpawnPosition, 100000f);
                }
            }
        }

        void DoGravity()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<Rigidbody>() != null)
                    enemy.GetComponent<Rigidbody>().useGravity = true;
            }
        }

        IEnumerator AnimateShape(GameObject shape)
        {
            yield return null;
        }
    }
}