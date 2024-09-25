using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Migo : Monster
{
    public Animator Ground_Migo_anim = null;

    public override void Start()
    {
        MoveSoundDelay = 3f;
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (isMove)
        {
            MoveDelay += Time.deltaTime;
        }

        if (this.EnemyCurHp <= 0)
        {
            isMove = false;
            isDie = true;
            Ground_Migo_anim.SetTrigger("GroundDie");
        }

        MoveSoundPlay();
    }

    public override void MoveSoundPlay()
    {
        if (MoveDelay > MoveSoundDelay)
        {
            base.MoveSoundPlay();
            MoveDelay = 0;
        }
    }

    public override void Attack()
    {
        if (Ground_Migo_anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            Ground_Migo_anim.SetTrigger("GroundAttack");
        }
        else
        {
            Ground_Migo_anim.CrossFade("attack", 0);
        }
    }

    public override void LookTarget()
    {
        base.LookTarget();
    }

    public override void EnemyMove()
    {
        if (this.EnemyCurHp > 0)
        {
            Ground_Migo_anim.SetTrigger("GroundMove");
            base.EnemyMove();
        }
    }

    public override void EnemyDamage(int bulletATK)
    {
        if (this.EnemyCurHp > 0)
        {
            Ground_Migo_anim.SetTrigger("GroundDamaged");
            base.EnemyDamage(bulletATK);
        }
    }
}
