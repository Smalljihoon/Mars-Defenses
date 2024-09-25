using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject LookTurret;         // �ͷ� ���
    [SerializeField] GameObject Tbullet = null;     // �ͷ� �Ѿ�
    [SerializeField] Transform bulletPosLeft = null;    // �Ѿ� �߻� ��ġ ��
    [SerializeField] Transform bulletPosRight = null;   // �Ѿ� �߻� ��ġ ��
    [SerializeField] float SenseRadius = 0f;    // ���� ���� �ݰ�
    [SerializeField] LayerMask AttackLayer;   // ���� ��� ���̾�
    [SerializeField] int BulletSpeed = 0;       // �Ѿ� �ӵ�

    private Transform pre_unit_trans = null;    //LookTurret���� �� ����
    private GameObject HitSensor; // Physics.OverlapSphere���� ������ �ݶ��̴��� ���� ������ ���ӿ�����Ʈ ����
    private Monster Mon;    
    private float ShotTimer = 0;    // �߻� �ֱ�
    private bool isSensor = false;  // ���� ����
    private bool is_looking_monster = true; // �ͷ��� ���͸� �ٶ󺸴��� ����

    private void Start()
    {
        pre_unit_trans = LookTurret.transform;
    }

    void Update()
    {
        Collider[] attackquest = Physics.OverlapSphere(transform.position, SenseRadius, AttackLayer);   // ���� �ݶ��̴�

        isSensor = attackquest.Length > 0;      // �����Ǹ� isSensor true

        foreach (var col in attackquest)    // ������ ������Ʈ�� �ݶ��̴��� ���� �͸� �̾Ƴ��� 
        {
            HitSensor = col.transform.gameObject;
            Mon = HitSensor.GetComponent<Monster>();
            break;
        }

        // �ͷ��� ȸ���� ���͸� �ٶ󺸴� ���� �´���
        if (LookTurret.transform.rotation.Equals(pre_unit_trans.rotation))
        {
            is_looking_monster = true;
        }

        if (isSensor && is_looking_monster)
        {
            Quaternion trans = Quaternion.LookRotation(Mon.transform.position - transform.position);
            ShotTimer += Time.deltaTime;
            LookTurret.transform.rotation = Quaternion.Lerp(LookTurret.transform.rotation, trans, 0.5f);
            pre_unit_trans.rotation = trans;
            is_looking_monster = false;
        }
 
        if (ShotTimer > 2f)
        {
            AttackTurret();
            ShotTimer = 0;
        }
    }

    public void AttackTurret()
    {
        GameObject L_Turretbullet = Instantiate(Tbullet, bulletPosLeft.position, Quaternion.identity);
        GameObject R_Turretbullet = Instantiate(Tbullet, bulletPosRight.position, Quaternion.identity);
        Rigidbody L_rigid = L_Turretbullet.GetComponent<Rigidbody>();
        Rigidbody R_rigid = R_Turretbullet.GetComponent<Rigidbody>();
        Vector3 Rdir = (Mon.transform.position - bulletPosLeft.position).normalized;
        Vector3 Ldir = (Mon.transform.position - bulletPosRight.position).normalized;

        L_rigid.velocity = Rdir * BulletSpeed * Time.deltaTime;
        R_rigid.velocity = Ldir * BulletSpeed * Time.deltaTime;

        Destroy(L_rigid, 2f);
        Destroy(R_rigid, 2f);
    }
}
