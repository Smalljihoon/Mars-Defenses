using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    private TrailRenderer trailRenderer;

    private void Start()
    {
            trailRenderer= GetComponent<TrailRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            // ÃÑ¾Ë »èÁ¦
            effect.transform.parent = null;
            trailRenderer.enabled = false;
            effect.SetActive(true);
            Destroy(Player.Instance.instantLauncher,0.5f);

            Monster mon = other.gameObject.GetComponent<Monster>();
            mon.EnemyDamage(Player.Instance.LauncherATK);
        }
        else if(other.gameObject.tag == "Shop")
        {
            Destroy(Player.Instance.instantLauncher);
            UIManager.Instance.ShopOnActive();
        }
        else if (other.gameObject.tag != "Enemy")
        {
            trailRenderer.enabled = false;
            effect.transform.parent = null;
            effect.SetActive(true);
            Destroy(Player.Instance.instantLauncher,0.5f);
        }
    }
}
