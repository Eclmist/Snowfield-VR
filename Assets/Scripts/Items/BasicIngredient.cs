using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicIngredient : AlchemyItem {

    public List<Attribute> attributes;
    public Dictionary<Attribute, float> attribDistribution;

    void Start()
    {
        attributes = new List<Attribute>();
        attribDistribution = new Dictionary<Attribute, float>();
    }

}
