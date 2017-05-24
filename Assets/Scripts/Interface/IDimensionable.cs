using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDimensionable  {

    Sprite Icon { get; set; }
    string Name { get; set; }
    int MaxStackSize { get; set; }
    int CurrentStackSize { get; set; }
    GameObject objReference { get;}

}
