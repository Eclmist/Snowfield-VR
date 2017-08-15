using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
	public GameObject CameraRig;

	
	public GameObject portalLeft;
	public GameObject leftUIPrefab;
	private GameObject leftUIReference;

	public GameObject portalRight;
	public GameObject rightUIPrefab;
	private GameObject rightUIReference;

	public GameObject[] disableWhenEnteringLeft;
	public GameObject[] disableWhenEnteringRight;
    public GameObject buyLibrary;
    public GameObject buyUIReference;
	public float range;
	public float UiZOffset = 0.262F;

	bool leftShown;
	bool rightShown;
    bool buyShown;

	protected void Update()
	{
        if (Player.Instance.Data.GetJob(JobType.MAGIC) != null)
        {
            //Left door
            if (Vector3.Distance(portalLeft.transform.position, Player.Instance.transform.position) < range)
            {
                if (!leftShown)
                {
                    leftShown = true;
                    leftUIReference = Instantiate(leftUIPrefab, portalLeft.transform.position + portalLeft.transform.forward * UiZOffset, portalLeft.transform.rotation);
                    leftUIReference.GetComponent<OptionPane>().SetEvent(OptionPane.ButtonType.Ok, new UnityEngine.Events.UnityAction(WarpToRight));
                }
            }
            else
            {
                if (leftShown)
                {
                    leftShown = false;
                    leftUIReference.GetComponent<OptionPane>().ClosePane();
                    leftUIReference = null;
                }
            }

            //Right door
            if (Vector3.Distance(portalRight.transform.position, Player.Instance.transform.position) < range)
            {
                if (!rightShown)
                {
                    rightShown = true;
                    rightUIReference = Instantiate(rightUIPrefab, portalRight.transform.position + portalRight.transform.forward * UiZOffset, portalRight.transform.rotation);
                    rightUIReference.GetComponent<OptionPane>().SetEvent(OptionPane.ButtonType.Ok, new UnityEngine.Events.UnityAction(WarpToLeft));
                }
            }
            else
            {
                if (rightShown)
                {
                    rightShown = false;
                    rightUIReference.GetComponent<OptionPane>().ClosePane();
                    rightUIReference = null;
                }
            }
        }
        else
        {
            if (Vector3.Distance(portalLeft.transform.position, Player.Instance.transform.position) < range)
            {
                if (!buyShown)
                {
                    buyShown = true;
                    buyUIReference = Instantiate(buyLibrary, portalLeft.transform.position + portalLeft.transform.forward * UiZOffset, portalLeft.transform.rotation);

                    OptionPane op = buyUIReference.GetComponent<OptionPane>();
                    op.SetEvent(OptionPane.ButtonType.Yes, new UnityEngine.Events.UnityAction(GameManager.Instance.GetMagicRole));
                    if (Player.Instance.Gold < 5000f)
                    {
                        op.GetButton(0).GetComponent<Collider>().enabled = false;
                    }
                }
            }
            else
            {
                if (buyShown)
                {
                    buyShown = false;
                    buyUIReference.GetComponent<OptionPane>().ClosePane();
                    buyUIReference = null;
                }
            }
        }
	}

	protected void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(portalLeft.transform.position, range);
		Gizmos.DrawWireSphere(portalRight.transform.position, range);
	}

	public void WarpToRight()
	{
		WarpToIndex(1);
	}

	public void WarpToLeft()
	{
		WarpToIndex(0);
	}

	private void WarpToIndex(int index)
	{
		Transform targetTransform;
		Vector3 targetRotation;

		switch (index)
		{
			case 0:
				targetTransform = portalLeft.transform;
				targetRotation = new Vector3(0, 180, 0);
				break;
			case 1:
				targetTransform = portalRight.transform;
				targetRotation = new Vector3(0, 90, 0);
				break;
			default:
				Debug.Log("Wrong Warp INDEX!!!!");
				return;
		}

		if (!alreadyRunning)
		{
			StartCoroutine(Warp(targetTransform, targetRotation, index));
		}
	}

	bool alreadyRunning;

	IEnumerator Warp(Transform targetTransform, Vector3 targetRotation, int index)
	{
		alreadyRunning = true;
		SteamVR_Fade.Start(Color.clear, 0);
		SteamVR_Fade.Start(Color.white, 0.5F);

		yield return new WaitForSeconds(0.5F);

		Vector3 position = targetTransform.transform.position + targetTransform.forward;
		position.y = 0;
		CameraRig.transform.position = position;
		var rot = CameraRig.transform.rotation;//= Quaternion.LookRotation(targetTransform.forward);
		rot.eulerAngles = targetRotation;
		CameraRig.transform.rotation = rot;

		foreach (GameObject g in disableWhenEnteringLeft)
		{
			g.SetActive(index == 1);
		}

		foreach (GameObject g in disableWhenEnteringRight)
		{
			g.SetActive(index == 0);
		}

		SteamVR_Fade.Start(Color.clear, 0.5F);

		alreadyRunning = false;
	}
}
