using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackColBox : MonoBehaviour
{
    public float FlameATK;
    //public ParticleSystem Flames;

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.Instance.TakeDamage(FlameATK);
        }
        else if(other.gameObject.CompareTag("Base"))
        {
            Base.Instance.HitBase(FlameATK);
        }
    }
}
