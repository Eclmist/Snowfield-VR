using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantPanel : MonoBehaviour
{

    [SerializeField]
    private InteractableBuy interactableBuy;
    [SerializeField]
    private GridLayoutGroup layoutGroup;
    


    // Displays the catalog
    public void InitializeAndDisplayCatalog(List<ItemData> itemDataList)
    {
        foreach(ItemData data in itemDataList)
            Instantiate(interactableBuy, layoutGroup.transform, false).Initialize(data);
    }

    

    


}
