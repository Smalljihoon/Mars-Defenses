using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private static Player PlayerInstance; // 플레이어 싱글톤
    public static Player Instance
    {
        get { return PlayerInstance; }
    }

    [Header("Transform")]
    public Transform Charater = null;        //케릭터 Transform
    [SerializeField] Transform cameraPos = null;    //cameraPos Object Transform

    [Header("캐릭터 스텟")]
    public float curHP = 1;
    public float maxHP = 1;
    public float MoveSpeed = 5f;              // 캐릭터 움직임 속도
    [SerializeField] float RunSpeed = 0f;               // 캐릭터 달리기 +속도

    [Header("Rifle 총기")]
    [SerializeField] GameObject RifleBullet = null;             // 라이플 총알 GO
    [SerializeField] Transform RiflePos = null;                      // Rifle 총구 
    [SerializeField] int RifleSpeed = 0;                               // 총알 속도
    public int max_magazine = 0;                             //  최대탄창
    public int cur_magazine = 0;                              // 현재 탄창
    public int RifleATK = 0;                                                // 라이플 공격력

    [Header("Launcher 총기")]
    [SerializeField] GameObject LauncherBullet = null;      // 유탄 총알 GO
    [SerializeField] int LauncherSpeed = 0;                         // 유탄 총알 스피드
    [SerializeField] Transform LauncherPos = null;             // Launcher 총구
    public int LauncherATK = 0;                                         // 유탄 공격력

    [Header("총기 공통")]
    [SerializeField] Image CrossHair = null;                           // 십자가 크로스헤어
    [SerializeField] GameObject ShotParticle = null;             // 총구에서의 파티클이벤트
    [SerializeField] float FireRate = 9f;                       // 초당 몇발을 쏠지 몇발에 대한 변수
    public Transform aim = null;                                // 크로스헤어 전용 UI 캔버스 (캔버스의 포지션이 바뀌면 화면상 크로스헤어 위치도 바뀜)

    [Header("디폴트")]
    [SerializeField] LayerMask layerMask;  // Physics.Raycast에 쓰일 레이어마스크
    [SerializeField] Camera cam = null;                            // 메인카메라 (시점)

    [Header("오디오")]
    public AudioClip RifleSound = null;
    public AudioClip LauncherSound = null;
    public AudioClip ReloadSound = null;
    public AudioClip HurtSound = null;
    public AudioClip StepSound = null;
    public AudioSource p_source = null;

    private Animator anim = null;                                           // 케릭터 애니메이터
    private CharacterController characterController = null;    // 캐릭터 컨트롤러
    private float timeBetweenShots = 0;                  // 총 슈팅 주기 변수
    private float shotTimer = 0;                                      // --
    private float Reloadsec = 0f;                   // 재장전 주기
    private bool canPlayAnimation = true;       // animation 재생 bool
    private Ray ray;                                            // 에임으로의 ray
    private Vector3 ScreenCenter = Vector3.zero;                   // 화면 중앙 벡터
    private RaycastHit hit;                         // 크로스헤어의 ray 광선에 맞은 객체를 담는 hit 변수
    private bool isRun = false;     // 달리고있냐
    private bool isReload = false;  // 장전중이냐
    private Vector2 moveInput = Vector2.zero;          // 이동 값을 받는 변수
    public bool isAttack = true;    // 공격 여부
    public int healAmount = 5;  // 힐량
    public float regenInterval = 2f; // 재생 간격시간
    public float healCount = 5f; // 힐이 되기전 데미지 받지 않아야 할 시간
    private float lastDamageTime = 0;
    public GameObject instantLauncher; // 유탄 총알
    public string hitname = string.Empty;   // 조준점에 맞은 객체의 이름
    public string hithp = string.Empty;     // 조준점에 맞은 객체의 hp
    public float hit_curhp = 0;     // 조준점에 맞은 객체의 현재 hp
    public float hit_maxhp = 0;     // 조준점에 맞은 객체의 최대hp
    public Monster EnemyGo = null;  // 조준점에 맞은 몬스터를 담는 변수
    public bool isNull = false; // 조준점에 객체가 있냐 없냐를 판단하는 bool값

    protected IEnumerator healCourtine = null;

    private void Awake()
    {
        PlayerInstance = this;
    }

    void Start()
    {
        curHP = maxHP;
        lastDamageTime = Time.time;
        cur_magazine = max_magazine;
        UIManager.Instance.Player_HpbarUpdate();
        timeBetweenShots = 1f / FireRate;
        anim = Charater.GetComponentInChildren<Animator>();
        characterController = Charater.GetComponentInChildren<CharacterController>();
        ray = new Ray();
    }

    // 에임으로 쏘는 ray 디버깅 체크용
    //private void OnDrawGizmos()
    //{
    //    Debug.DrawRay(ScreenCenter, cam.transform.forward * 1000, Color.red);
    //}

    void Update()
    {
        if (isReload)
        {
            Reloadsec += Time.deltaTime;    // 재장전 타임
        }
        shotTimer += Time.deltaTime;    // 총 발사 주기 타임

        if (Time.time - lastDamageTime >= healCount && curHP < maxHP)
        {
            if (healCourtine == null)
            {
                healCourtine = Heal();
                StartCoroutine(healCourtine);
            }
        }

        AimingPoint();
        LookAround();   // 마우스 회전
        Move();              // 캐릭터 무빙
        Roll();                 // 구르기
        Reload();           // 장전
        RifleShot();        // 라이플 샷
        LauncherShot(); // 유탄 샷
        UIManager.Instance.Status_UI();
    }

    private void LateUpdate()
    {
        // 캐릭터의 위치에 따라 카메라를 이동
        cameraPos.position = Charater.position + new Vector3(0, 2.0f, 0);
    }

    private void AimingPoint()
    {
        ScreenCenter = cam.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));   // 화면 중앙
        ray = new Ray(ScreenCenter, cam.transform.forward * 1000);      // 조준점 레이
        // 나중에 커서매니저로 이동?
        if (Physics.Raycast(ray, out hit, 10000))
        {
            if (hit.collider.CompareTag("Enemy"))   // 적이면 크로스헤어 레드색상
            {
                CrossHair.color = Color.red;
                Hitobject(hit);
                isNull = false;
            }
            else // 적이 아니면 흰색
            {
                CrossHair.color = Color.white;
                isNull = true;
            }
        }
        else
        {
            CrossHair.color = Color.white;
        }
    }

    public void Hitobject(RaycastHit hitGO)
    {
        EnemyGo = hitGO.transform.gameObject.GetComponent<Monster>();

        hitname = EnemyGo.Enemyname;
        hithp = EnemyGo.EnemyCurHp.ToString();

        hit_curhp = EnemyGo.EnemyCurHp;
        hit_maxhp = EnemyGo.EnemyMaxHp;
    }

    public void TakeDamage(float damage)
    {
        curHP -= damage;
        curHP = Mathf.Clamp(curHP, 0, maxHP);
        lastDamageTime = Time.time;
        StopCoroutine(Heal());

        p_source.clip = HurtSound;
        p_source.Play();
        UIManager.Instance.Player_HpbarUpdate();
    }

    // 체력회복 코루틴
    IEnumerator Heal()
    {
        while (curHP < maxHP  && curHP != 0)
        {
            curHP += healAmount;
            curHP = Mathf.Clamp(curHP, 0, maxHP);
            UIManager.Instance.Player_HpbarUpdate();

            yield return new WaitForSeconds(regenInterval);
        }
        healCourtine = null;
    }

    // 장전
    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) || cur_magazine == 0 && !isReload)
        {
            isReload = true;
            anim.SetBool("isReload", true);
            p_source.clip = ReloadSound;
            p_source.Play();
        }
        if (Reloadsec > 3f)
        {
            cur_magazine = max_magazine;
            UIManager.Instance.Bullet_UI();
            anim.SetBool("isReload", false);
            Reloadsec = 0f;
            isReload = false;
        }
    }

    // 라이플 총 발사
    void RifleShot()
    {
        if (Input.GetMouseButton(0) && shotTimer > timeBetweenShots && !isRun && !isReload && cur_magazine > 0 && isAttack)
        {
            anim.SetBool("isShoot", true);
            GameObject instantRifle = Instantiate(RifleBullet, RiflePos.position, Quaternion.identity);     // 총알 생성
            Rigidbody RifleRigid = instantRifle.GetComponent<Rigidbody>();      // 총알의 리지드바디 가져오기
            Vector3 Rdir = (aim.position - RiflePos.position).normalized;   // 총알이 날아갈 방향 노멀라이즈
            RifleRigid.velocity = Rdir * RifleSpeed * Time.deltaTime;       // 총알 발사
            p_source.clip = RifleSound;
            p_source.Play();
            Particle(); // 총구 파티클 재생
            Destroy(instantRifle, 1f);  // 생성된 총알 파괴
            shotTimer = 0;

            if (Physics.Raycast(ray, out hit, 1000, layerMask))     // 레이어 마스크 잘 쓰기!!
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    Monster enemy = hit.transform.gameObject.GetComponent<Monster>();
                    // 적 데미지
                    enemy.EnemyDamage(RifleATK);
                }
                else if (hit.transform.gameObject.tag == "Shop")
                {
                    UIManager.Instance.ShopOnActive();
                }
            }

            cur_magazine = cur_magazine - 1;
            UIManager.Instance.Bullet_UI();
        }
        else if (Input.GetMouseButtonUp(0) || cur_magazine <= 0)  // 탄창에 총알이 없으면 발사하는 애니메이션을 false로
        {
            anim.SetBool("isShoot", false);
        }
    }

    // 유탄 총 발사
    void LauncherShot()
    {
        if (Input.GetMouseButtonDown(1) && !isReload)
        {
            anim.SetBool("isShoot", true);
            instantLauncher = Instantiate(LauncherBullet, LauncherPos.position, Quaternion.identity);     // 유탄 총알 생성
            Rigidbody LauncherRigid = instantLauncher.GetComponent<Rigidbody>();      // 총알의 리지드바디 가져오기
            Vector3 Ldir = (Player.Instance.aim.position - Player.Instance.LauncherPos.position).normalized;   // 총알이 날아갈 방향 노멀라이즈
            Vector3 Updir = transform.up * 5;
            LauncherRigid.velocity = Updir + Ldir * LauncherSpeed * Time.deltaTime;       // 총알 발사
            p_source.clip = LauncherSound;
            p_source.Play();
            Particle(); // 총구 파티클 재생
        }
        else if (Input.GetMouseButtonUp(1))
        {
            anim.SetBool("isShoot", false);
        }
    }

    // 마우스 회전
    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraPos.rotation.eulerAngles;
        float camRotationZ = camAngle.z + mouseDelta.y;
        float camRotationY = camAngle.y + mouseDelta.x;

        if (camRotationZ < 180f)       // 위로 회전하는 경우
        {
            camRotationZ = Mathf.Clamp(camRotationZ, -1f, 70f);   // -1f을 0으로 하지않는 이유 = 카메라가 수평면 밑으로 회전하지 않는 문제 때문
        }
        else        // 아래로 회전하는 경우
        {
            camRotationZ = Mathf.Clamp(camRotationZ, 335f, 361f);
        }

        cameraPos.rotation = Quaternion.Euler(0, camRotationY, camRotationZ);
    }

    // 캐릭터 무빙
    private void Move()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        anim.SetFloat("Walk_Horizontal", moveInput.x);
        anim.SetFloat("Walk_Vertical", moveInput.y);

        Vector3 lookForward = new Vector3(cameraPos.forward.x, 0f, cameraPos.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraPos.right.x, 0f, cameraPos.right.z).normalized;
        Vector3 moveDir = lookForward * -moveInput.x + lookRight * moveInput.y;

        Charater.forward = lookRight;
        // CharacterController를 통해 캐릭터를 이동
        if (Input.GetKey(KeyCode.LeftShift) && moveInput.y > 0)    // 왼쪽 쉬프트 누르면 뛰기
        {
            isRun = true;
            anim.SetBool("isRun", true);
            characterController.Move(moveDir * MoveSpeed * RunSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftShift) && moveInput.y < 0)
        {
            isRun = false;
            anim.SetBool("isRun", false);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            anim.SetBool("isRun", false);
        }
        //p_source.clip = StepSound;
        //p_source.Play();
        characterController.Move(moveDir * MoveSpeed * Time.deltaTime);
    }

    // 구르기
    void Roll()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime >= 0.99f && !anim.IsInTransition(0) && moveInput.y >= 0)
        {
            canPlayAnimation = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canPlayAnimation)
        {
            anim.SetTrigger("Roll");
            canPlayAnimation = false;
        }
    }

    // 총알 발사시 총구 이펙트
    public GameObject Particle()
    {
        GameObject InstantParticle = Instantiate(ShotParticle, RiflePos);
        return InstantParticle;
    }
}
