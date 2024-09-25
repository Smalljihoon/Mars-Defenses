using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    private int TurretATK = 0;
    // ÅÍ·¿ ÃÑ¾Ë µ¥¹ÌÁö
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // ÃÑ¾Ë »èÁ¦
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
