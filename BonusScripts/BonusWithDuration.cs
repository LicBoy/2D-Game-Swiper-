using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusWithDuration : Bonus
{
    public float duration = 30f;

    public override bool HasDuration()
    {
        return true;
    }
}
