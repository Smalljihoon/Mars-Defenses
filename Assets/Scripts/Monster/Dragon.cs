using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Monster
{
    public Animator Dragonanim = null;
    public ParticleSystem Flames = null;
    private bool isFly = false;
    private float breathDelay = 7;

    public override void Start()
    {
        AttackSoundDelay = 7;
        MoveSoundDelay = 6f;
        Flames.Stop();
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (isMove)
        {
            MoveDelay += Time.deltaTime;
        }

        breathDelay -= Time.deltaTime;

        if (breathDelay < 0)
        {
            AttackDelay -= Time.deltaTime;

            if (AttackDelay < 0)
            {
                isAttackReady = true;
                breathDelay = 7;
                AttackDelay = 2;
            }
        }

        if (isAttackSense && !isDie && breathDelay >= 0 && isAttackReady)
        {
            isAttackReady = false;
            Flames.Play();
            BreathSoundPlay(true);
        }
        else if (breathDelay <= 0 || (!isAttackSense && isDie))
        {
            Flames.Stop();
            BreathSoundPlay(false);
        }

        if (transform.position.y < 20)
        {
            isFly = true;
        }
        else if (transform.position.y > 20)
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
            Dragonanim.SetBool("isDie", true);
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

    protected void BreathSoundPlay(bool isPlay)
    {
        if (isPlay)
        {
            ATK_source.Play();
        }
        else
        {
            ATK_source.Stop();
        }
    }

    private void UpFly()
    {
        Enemyrigid.AddForce(Vector3.up * 3, ForceMode.Impulse);
    }

    public override void EnemyMove()
    {
        if (this.EnemyCurHp > 0)
        {
            Dragonanim.SetTrigger("DragonMove");
            Movedir = (TargetPoint - transform.position).normalized;  // �̵� ���� ��ֶ�����

            if (isSensor)
            {
                TargetPoint = Player.Instance.transform.GetChild(0).position;       // Ÿ�� ���� = player
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Movedir), 0.1f);   // �̵��������� ȸ��
            }
            else
            {
                TargetPoint = PlayerBase.transform.position;                    // Ÿ�� ���� = base
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(PlayerBase.transform.position - this.transform.position), 0.1f);  // �̵��������� ȸ��
            }

            MovePos = Enemyrigid.position + Movedir * EnemyMoveSpeed * Time.deltaTime;  // �̵�
            Enemyrigid.MovePosition(MovePos);   // Enemyrigid.MovePosition()�� ���� �̵�
            //E_source.clip = E_walkSound;
            //E_source.Play();
        }
    }
}
