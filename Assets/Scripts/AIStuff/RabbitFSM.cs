using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitFSM : MonsterFSM {

    private bool isJumping = false;

    protected override bool CanMove()
    {
        return base.CanMove() && isJumping;
    }

    protected void SetJump(int i)
    {
        switch (i)
        {
            case 0:
                isJumping = false;
                return;
            case 1:
                isJumping = true;
                return;
        }
    }
}
