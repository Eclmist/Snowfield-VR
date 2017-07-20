using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSpell : Spell {

    protected override void Cast()
    {
        Debug.Log("Called HoldSpell");

        if (SpellPrefab != null)
        {
            Debug.Log("Hello");
            Instantiate(SpellPrefab);
        }
        else
        {
            Debug.Log("It is null");
        }
    }
}
