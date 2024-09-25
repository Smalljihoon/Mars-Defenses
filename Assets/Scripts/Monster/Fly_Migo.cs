using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly_Migo : Monster
{
    public Animator Fly_Migo_anim = null;
    bool isFly = false;

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

        if (transform.position.y < 7)
        {
            isFly = true;
        }
        else if (transform.position.y > 10)
        {
            isFly = false;
        }

        if (isFly && !isDie)
        {
            UpFly();
        }

        if (this.EnemyCurHp <= 0)
        {
            isMove = false;
            isDie = true;
            Fly_Migo_anim.SetTrigger("FlyDie");
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
            Fly_Migo_anim.SetTrigger("FlyDamaged");
            base.EnemyDamage(bulletATK);
        }
    }

    public void UpFly()
    {
        Enemyrigid.AddForce(Vector3.up * 2, ForceMode.Impulse);
    }

    public override void LookTarget()
    {
        if (Fly_Migo_anim.GetCurrentAnimatorStateInfo(0).IsName("fly_attack"))
        {
            Fly_Migo_anim.SetTrigger("FlyAttack");
        }
        else
        {
            Fly_Migo_anim.CrossFade("fly_attack", 0);
        }
        base.LookTarget();
    }

    public override void EnemyMove()
    {
        if (this.EnemyCurHp > 0)
        {
            Fly_Migo_anim.SetTrigger("FlyMove");
            base.EnemyMove();
        }
    }
}
