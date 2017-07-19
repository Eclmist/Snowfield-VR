using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorable
{
   int ItemID { get; }
   Sprite Icon { get; }
   int MaxStackSize { get; }
   GameObject ObjectReference { get; }

    
}
