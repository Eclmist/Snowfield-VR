using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveStats
{
    StatsContainer StatContainer
    {
        get;
    }
}
