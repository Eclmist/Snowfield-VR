using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

public class ItemsManager
{
    [XmlArray("BlacksmithItems")]
    [XmlArrayItem("BlacksmithItem")]
    public static List<BlacksmithItem> BlacksmithItemList;

    [XmlArray("AlchemyItems")]
    [XmlArrayItem("AlchemyItem")]
    public static List<AlchemyItem> AlchemyItemList;
}
