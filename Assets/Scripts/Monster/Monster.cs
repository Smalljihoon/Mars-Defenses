using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("몬스터 정보")]
    public string Enemyname = string.Empty; // UI 체력표시때 표시할 이름
    public float EnemyCurHp = 0;     // 몬스터 HP
    public float EnemyMaxHp = 0;
    public float EnemyMoveSpeed = 0;  // 몬스터 이동속도
    public float IncreaseHp = 2f;    // 라운드 증가시 체력증가 배율

    [Header("몬스터 센서")]
    public GameObject PlayerBase = null; // 목표 지점 (목표물1)
    public GameObject Character = null;    // 플레이어 (목표물2)
    public float Moveradius = 10;    // 탐지 반경
    public float Attackradius = 5; // 공격 반경 
    public LayerMask Attacklayer;   // 몬스터 공격시 감지 레이어   = 플레이어 , 기지

    [Header("오디오")]
    public AudioSource Walk_source = null;
    public AudioSource Die_source = null;
    public AudioSource ATK_source = null;

    [Header("Dummy")]
    protected bool isSensor = false;    // 감지 여부 bool변수
    protected bool isDie = false;       // 죽음 여부 bool
    protected bool isMove = true;   //  이동 여부 bool
    protected bool isStun = false;
    protected bool isAttackReady = true;
    protected bool isAttack = false;    // 플레이어를 공격하는지 bool
    protected bool isAttackSense = false;   // 공격여부 bool
    protected float Diedelay = 0f;      // 몬스터가 죽으면 파괴되기까지 딜레이 시간
    protected float OriginSpeed;    // 이동 스피드 원본
    protected float StunTime = 0f;  // 몬스터 피격시 스턴시간
    protected float AttackSoundDelay = 0;
    protected float AttackDelay = 2;
    protected float AttackTime = 0;
    protected float MoveSoundDelay = 0;
    protected float MoveDelay = 0;
    protected Vector3 Movedir = Vector3.zero;   // 이동 방향 벡터 변수
    protected Vector3 MovePos = Vector3.zero;      // 이동을 담을 벡터 변수
    protected Vector3 TargetPoint = Vector3.zero;   // 타겟지점 벡터 변수
    protected Rigidbody Enemyrigid; // 몬스터 리지드바디

    public virtual void Start()
    {
        EnemyRoundHp(); // 몬스터 생성시 라운드 별 체력 조정
        EnemyCurHp = EnemyMaxHp;
        OriginSpeed = EnemyMoveSpeed;
        Enemyrigid = GetComponent<Rigidbody>();
    }

    public virtual void Update()
    {
        if (this.transform.position.y < 4.8f)   // 만약 몬스터가 맵 밑에 있다면 파괴
        {
            Destroy(gameObject);
        }

        isAttack = false;

        MoveAndAttack_Sense();

        if (isStun) // 피격시 스턴
        {
            StunTime += Time.deltaTime;
        }

        if (isMove) // 이동
        {
            EnemyMove();
        }

        // 스턴 종료
        if (StunTime > 2.0f)
        {
            isMove = true;
            EnemyMoveSpeed = OriginSpeed;
            StunTime = 0f;
            isStun = false;
        }

        if (this is not Dragon)
        {
            AttackDelay -= Time.deltaTime;

            isAttackReady = AttackDelay <= 0;

            if (isAttackReady && isAttackSense)
            {
                Attack();
                AttackDelay = 2;
            }
        }
    }

    public void MoveAndAttack_Sense()       // 이동 & 공격 영역 감지 함수
    {
        Collider[] region = Physics.OverlapSphere(transform.position, Moveradius, Attacklayer);  // layermask = player, base

        isSensor = false;
        isAttackSense = false;
        isAttack = false;

        //레이어 번호를 정의
        int playerLayer = LayerMask.NameToLayer("Player");

        // region 배열을 순회하여 레이어 확인
        foreach (var collider in region)
        {
            isAttackSense = Vector3.Distance(collider.transform.position, transform.position) < Attackradius;
            if (collider.gameObject.layer == playerLayer)
            {
                isSensor = true;
                if (isAttackSense)
                {
                    isAttack = true;
                }
            }
        }

        // 공격을 위한 조건문 (이동감지반경 > 공격감지반경) 
        if (isAttackSense) // 공격 범위 내 감지 ok
        {
            if (isSensor)   // 이동감지에 플레이어 감지가 되면
            {
                if (isAttack)   // 공격대상이 플레이어라면
                {
                    LookTarget();
                    isMove = false;
                }
                else // 플레이어한테 이동
                {
                    isMove = true; // 이동 ok
                }
            }
            else // 플레이어 감지가 아닌 기지라면
            {
                LookTarget();
                isMove = false;
            }
        }
        else
        {
            isMove = true;
        }
    }

    //몬스터 죽음
    public void Die()       // 애니메이션 클립용 함수
    {
        GameManager.Instance.GetMoney();
        GameManager.Instance.CountKill += 1;
        GameManager.Instance.Kill += 1;

        Destroy(this.gameObject);
    }

    // 라운드 상승시 몬스터 체력 증가
    public virtual void EnemyRoundHp()
    {
        EnemyMaxHp = Mathf.RoundToInt(EnemyMaxHp * Mathf.Pow(IncreaseHp, GameManager.Instance.Round - 1));
    }

    public virtual void Attack()
    {

    }

    //몬스터 회전
    public virtual void LookTarget()
    {
        if (isAttack)   // 공격대상 플레이어
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Player.Instance.transform.GetChild(0).position - transform.position), 0.5f);
        }
        else        // 공격대상 기지
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Base.Instance.transform.GetChild(0).position - transform.position), 0.5f);
        }
    }

    public void AttackSoundPlay()
    {
        Walk_source.Stop();
        ATK_source.Play();
    }

    public virtual void MoveSoundPlay()
    {
        Walk_source.Play();
    }

    public void DieSoundPlay()
    {
        Walk_source.Stop();
        Die_source.Play();
    }

    // 몬스터 이동
    public virtual void EnemyMove()
    {
        Movedir = (TargetPoint - transform.position).normalized;  // 이동 방향 노멀라이즈

        if (isSensor)
        {
            TargetPoint = Player.Instance.transform.GetChild(0).position;       // 타겟 지점 = player
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Movedir), 0.1f);   // 이동방향으로 회전
        }
        else
        {
            TargetPoint = PlayerBase.transform.position;                    // 타겟 지점 = base
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(PlayerBase.transform.position - this.transform.position), 0.1f);  // 이동방향으로 회전
        }

        MovePos = Enemyrigid.position + Movedir * EnemyMoveSpeed * Time.deltaTime;  // 이동
        Enemyrigid.MovePosition(MovePos);   // Enemyrigid.MovePosition()을 통해 이동
    }

    // 데미지 피격       (라이플(히트스캔))
    public virtual void EnemyDamage(int bulletATK)   //  플레이어에서 호출
    {
        if (EnemyCurHp > 0)
        {
            isMove = false; // 움직이지 마라!
            isStun = true;
            EnemyMoveSpeed = 0; // 스턴 => 이속 = 0
            EnemyCurHp = EnemyCurHp - bulletATK;   // 호출 될 때마다 총의 공격력만큼 hp에서 깎는다
        }
    }

    //기즈모 Scene 체크용
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Moveradius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Attackradius);
    }
}
