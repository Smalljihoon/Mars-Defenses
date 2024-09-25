using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private static Base instance;  // Base �̱���
    public static Base Instance
    {
        get { return instance; }
    }

    [SerializeField] GameObject BaseAntena = null;

    public float MaxbaseHP = 10000; // �ִ� ü��
    public float curBaseHp = 1;          // ���� ü��

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

    // ���� ������
    public void HitBase(float damage)
    {
        curBaseHp = curBaseHp - damage;

        curBaseHp = Mathf.Clamp(curBaseHp, 0, MaxbaseHP);

        UIManager.Instance.Base_HpbarUpdate();
    }
}
