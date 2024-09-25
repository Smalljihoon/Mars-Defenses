using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    private int TurretATK = 0;
    // �ͷ� �Ѿ� ������
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // �Ѿ� ����
            Destroy(this.gameObject);

            Monster mon = other.gameObject.GetComponent<Monster>();
            mon.EnemyDamage(TurretATK * (GameManager.Instance.Round + 1));
        }
        else if (other.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
