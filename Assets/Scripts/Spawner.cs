using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] FourRange;
    [SerializeField] GameObject[] Enemys;
    [SerializeField] Transform Target;
    [SerializeField] float spawnperiod = 10;         // 스폰 주기
    float spawnTime = 0;

    void Update()
    {
        spawnTime += Time.deltaTime;

        if(spawnTime > spawnperiod )
        {
            spawn();
            spawnTime = 0;
        }
    }

    private Vector3 Return_RandomPosition(GameObject Zone)
    {
        Vector3 OriginPos = Zone.transform.position;

        Collider col = Zone.GetComponent<Collider>();

        float range_x = col.bounds.size.x;
        float range_z = col.bounds.size.z;

        range_x = Random.Range((range_x / 2) * -1, (range_x / 2));
        range_z = Random.Range((range_z / 2) * -1, (range_z / 2));

        Vector3 RandomPos = new Vector3(range_x, 15, range_z);
        Vector3 RespawnPos = OriginPos + RandomPos;

        return RespawnPos;
    }

    private GameObject Return_RandomEnemy()
    {
        var RandomEnemy = Random.Range(0, Enemys.Length);

        return Enemys[RandomEnemy];
    }

    GameObject RandomZone()
    {
        var Pickzone = Random.Range(0, FourRange.Length);

        return FourRange[Pickzone];
    }

    private void spawn()
    {
        GameObject newEnemy = Instantiate(Return_RandomEnemy(), Return_RandomPosition(RandomZone()), Quaternion.identity);
        Vector3 dir = Target.transform.position - newEnemy.transform.position;
        var rot = Quaternion.LookRotation(dir, Vector3.up);
        newEnemy.transform.rotation = rot;
    }
}
