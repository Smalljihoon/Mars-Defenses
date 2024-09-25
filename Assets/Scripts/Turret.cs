using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject LookTurret;         // 터렛 헤드
    [SerializeField] GameObject Tbullet = null;     // 터렛 총알
    [SerializeField] Transform bulletPosLeft = null;    // 총알 발사 위치 왼
    [SerializeField] Transform bulletPosRight = null;   // 총알 발사 위치 오
    [SerializeField] float SenseRadius = 0f;    // 몬스터 감지 반경
    [SerializeField] LayerMask AttackLayer;   // 공격 대상 레이어
    [SerializeField] int BulletSpeed = 0;       // 총알 속도

    private Transform pre_unit_trans = null;    //LookTurret과의 비교 변수
    private GameObject HitSensor; // Physics.OverlapSphere에서 감지된 콜라이더를 통해 가져올 게임오브젝트 변수
    private Monster Mon;    
    private float ShotTimer = 0;    // 발사 주기
    private bool isSensor = false;  // 감지 여부
    private bool is_looking_monster = true; // 터렛이 몬스터를 바라보는지 여부

    private void Start()
    {
        pre_unit_trans = LookTurret.transform;
    }

    void Update()
    {
        Collider[] attackquest = Physics.OverlapSphere(transform.position, SenseRadius, AttackLayer);   // 감지 콜라이더

        isSensor = attackquest.Length > 0;      // 감지되면 isSensor true

        foreach (var col in attackquest)    // 감지된 오브젝트중 콜라이더를 가진 것만 뽑아내기 
        {
            HitSensor = col.transform.gameObject;
            Mon = HitSensor.GetComponent<Monster>();
            break;
        }

        // 터렛의 회전이 몬스터를 바라보는 것이 맞는지
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
