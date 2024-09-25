using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private static Base instance;  // Base 싱글톤
    public static Base Instance
    {
        get { return instance; }
    }

    [SerializeField] GameObject BaseAntena = null;

    public float MaxbaseHP = 10000; // 최대 체력
    public float curBaseHp = 1;          // 현재 체력

    private void Awake()
    {
        instance= this;
    }

    private void Start()
    {
        curBaseHp = MaxbaseHP;
        UIManager.Instance.Base_HpbarUpdate();
    }

    void Update()
    {
        BaseAntena.transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
    }

    // 기지 데미지
    public void HitBase(float damage)
    {
        curBaseHp = curBaseHp - damage;

        curBaseHp = Mathf.Clamp(curBaseHp, 0, MaxbaseHP);

        UIManager.Instance.Base_HpbarUpdate();
    }
}
