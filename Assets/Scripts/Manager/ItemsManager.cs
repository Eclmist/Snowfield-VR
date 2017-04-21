using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

using System.Reflection;

//public enum PotionType
//{
//Red,
//Blue
//}

public class ItemsManager
{
    [XmlArray("BlacksmithItems")]
    [XmlArrayItem("BlacksmithItem")]
    public static List<BlacksmithItem> BlacksmithItemList;

    [XmlArray("AlchemyItems")]
    [XmlArrayItem("AlchemyItem")]
    public static List<AlchemyItem> AlchemyItemList;

    [XmlArray("CraftedItems")]
    [XmlArrayItem("CraftedItem")]
    public static List<CraftedItem> CraftedItemList;

    //public Dictionary<, System.Type> 

    //public static void asdsa()
    //{
    //    var assembly = Assembly.GetExecutingAssembly();
    //    var types = assembly.GetTypes();

    //    foreach (var t in types)
    //    {
    //        if (t.GetType() == typeof(Potion))
    //        {
    //            var fields = t.GetFields();

    //            foreach (var f in fields)
    //            {
    //                if (f.ReflectedType == typeof(PotionType))
    //                {
    //                    System.Type 
    //                }
    //            }
    //        }
    //    }

    //}
}
