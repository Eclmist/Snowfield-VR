using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour {

    protected float destroyTimer = 0;

    protected GenericItem item;

    protected void Start()
    {
        item = GetComponent<GenericItem>();
        if (!item)
        {
            Debug.Log("No Generic item");
            Destroy(this);
        }

    }

    protected void Update()
    {
        if (item.LinkedController == null)
        {
            destroyTimer += Time.deltaTime;
            if (destroyTimer > GameConstants.Instance.DroppedDespawnTimer)
            {
                gameObject.SetActive(false);
                Destroy(gameObject, 3);
            }
        }
        else
        {
            Destroy(this);
        }
    }
}
