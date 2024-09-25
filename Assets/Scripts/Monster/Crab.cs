using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Crab : Monster
{
    public Animator Crab_anim;  // 크랩 애니메이터

    public override void Start()
    {
        MoveSoundDelay = 21f;
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
            Crab_anim.SetTrigger("CrabDie");
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

    public override void EnemyDamage(int bulletATK)
    {
        if (this.EnemyCurHp > 0 && !isDie)
        {
            if (Crab_anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
            {
                Crab_anim.SetTrigger("CrabDamage");
            }
            else
            {
                Crab_anim.CrossFade("Damage", 0);
            }

            base.EnemyDamage(bulletATK);
        }
    }

    public override void LookTarget()
    {
        Crab_anim.SetTrigger("CrabAttack");
        base.LookTarget();
    }

    public override void EnemyMove()
    {
        if (this.EnemyCurHp > 0)
        {
            Crab_anim.SetTrigger("CrabWalk");
            base.EnemyMove();
        }
    }

}
