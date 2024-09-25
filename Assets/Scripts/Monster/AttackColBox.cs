using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColBox : MonoBehaviour
{
    public GameObject EnemyAttack = null; // ���� col�� ��� ���� �ڽ� ������Ʈ
    public float EnemyATK = 0;    // ���� ���ݷ�
    private float IncreaseATK = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Player.Instance.TakeDamage(Mathf.RoundToInt(EnemyATK * Mathf.Pow(IncreaseATK, GameManager.Instance.Round - 1)));
            }
            else if(other.gameObject.CompareTag("Base"))
            {
                Base.Instance.HitBase(EnemyATK);
            }
        }
        else
            return;
    }
}


