using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class ItemsFactory : MonoBehaviour {

    public static void SaveBlacksmithItem(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<BlacksmithItem>));
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, ItemsManager.BlacksmithItemList);
            Debug.Log("Saved Blacksmith Item(s)");
        }
    }

    public static List<BlacksmithItem> LoadBlacksmithItem(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<BlacksmithItem>));
        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            Debug.Log("Loaded Blacksmith Item(s)");
            return serializer.Deserialize(stream) as List<BlacksmithItem>;
        }
    }

    public static void SaveAlchemyItem(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<AlchemyItem>));
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, ItemsManager.AlchemyItemList);
            Debug.Log("Saved AlchemyItem(s)");
        }
    }

    public static List<AlchemyItem> LoadAlchemyItem(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<AlchemyItem>));
        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            Debug.Log("Loaded AlchemyItem(s)");
            return serializer.Deserialize(stream) as List<AlchemyItem>;
        }
    }
}
