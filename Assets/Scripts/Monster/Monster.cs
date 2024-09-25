using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("���� ����")]
    public string Enemyname = string.Empty; // UI ü��ǥ�ö� ǥ���� �̸�
    public float EnemyCurHp = 0;     // ���� HP
    public float EnemyMaxHp = 0;
    public float EnemyMoveSpeed = 0;  // ���� �̵��ӵ�
    public float IncreaseHp = 2f;    // ���� ������ ü������ ����

    [Header("���� ����")]
    public GameObject PlayerBase = null; // ��ǥ ���� (��ǥ��1)
    public GameObject Character = null;    // �÷��̾� (��ǥ��2)
    public float Moveradius = 10;    // Ž�� �ݰ�
    public float Attackradius = 5; // ���� �ݰ� 
    public LayerMask Attacklayer;   // ���� ���ݽ� ���� ���̾�   = �÷��̾� , ����

    [Header("�����")]
    public AudioSource Walk_source = null;
    public AudioSource Die_source = null;
    public AudioSource ATK_source = null;

    [Header("Dummy")]
    protected bool isSensor = false;    // ���� ���� bool����
    protected bool isDie = false;       // ���� ���� bool
    protected bool isMove = true;   //  �̵� ���� bool
    protected bool isStun = false;
    protected bool isAttackReady = true;
    protected bool isAttack = false;    // �÷��̾ �����ϴ��� bool
    protected bool isAttackSense = false;   // ���ݿ��� bool
    protected float Diedelay = 0f;      // ���Ͱ� ������ �ı��Ǳ���� ������ �ð�
    protected float OriginSpeed;    // �̵� ���ǵ� ����
    protected float StunTime = 0f;  // ���� �ǰݽ� ���Ͻð�
    protected float AttackSoundDelay = 0;
    protected float AttackDelay = 2;
    protected float AttackTime = 0;
    protected float MoveSoundDelay = 0;
    protected float MoveDelay = 0;
    protected Vector3 Movedir = Vector3.zero;   // �̵� ���� ���� ����
    protected Vector3 MovePos = Vector3.zero;      // �̵��� ���� ���� ����
    protected Vector3 TargetPoint = Vector3.zero;   // Ÿ������ ���� ����
    protected Rigidbody Enemyrigid; // ���� ������ٵ�

    public virtual void Start()
    {
        EnemyRoundHp(); // ���� ������ ���� �� ü�� ����
        EnemyCurHp = EnemyMaxHp;
        OriginSpeed = EnemyMoveSpeed;
        Enemyrigid = GetComponent<Rigidbody>();
    }

    public virtual void Update()
    {
        if (this.transform.position.y < 4.8f)   // ���� ���Ͱ� �� �ؿ� �ִٸ� �ı�
        {
            Destroy(gameObject);
        }

        isAttack = false;

        MoveAndAttack_Sense();

        if (isStun) // �ǰݽ� ����
        {
            StunTime += Time.deltaTime;
        }

        if (isMove) // �̵�
        {
            EnemyMove();
        }

        // ���� ����
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

    public void MoveAndAttack_Sense()       // �̵� & ���� ���� ���� �Լ�
    {
        Collider[] region = Physics.OverlapSphere(transform.position, Moveradius, Attacklayer);  // layermask = player, base

        isSensor = false;
        isAttackSense = false;
        isAttack = false;

        //���̾� ��ȣ�� ����
        int playerLayer = LayerMask.NameToLayer("Player");

        // region �迭�� ��ȸ�Ͽ� ���̾� Ȯ��
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

        // ������ ���� ���ǹ� (�̵������ݰ� > ���ݰ����ݰ�) 
        if (isAttackSense) // ���� ���� �� ���� ok
        {
            if (isSensor)   // �̵������� �÷��̾� ������ �Ǹ�
            {
                if (isAttack)   // ���ݴ���� �÷��̾���
                {
                    LookTarget();
                    isMove = false;
                }
                else // �÷��̾����� �̵�
                {
                    isMove = true; // �̵� ok
                }
            }
            else // �÷��̾� ������ �ƴ� �������
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

    //���� ����
    public void Die()       // �ִϸ��̼� Ŭ���� �Լ�
    {
        GameManager.Instance.GetMoney();
        GameManager.Instance.CountKill += 1;
        GameManager.Instance.Kill += 1;

        Destroy(this.gameObject);
    }

    // ���� ��½� ���� ü�� ����
    public virtual void EnemyRoundHp()
    {
        EnemyMaxHp = Mathf.RoundToInt(EnemyMaxHp * Mathf.Pow(IncreaseHp, GameManager.Instance.Round - 1));
    }

    public virtual void Attack()
    {

    }

    //���� ȸ��
    public virtual void LookTarget()
    {
        if (isAttack)   // ���ݴ�� �÷��̾�
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Player.Instance.transform.GetChild(0).position - transform.position), 0.5f);
        }
        else        // ���ݴ�� ����
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

    // ���� �̵�
    public virtual void EnemyMove()
    {
        Movedir = (TargetPoint - transform.position).normalized;  // �̵� ���� ��ֶ�����

        if (isSensor)
        {
            TargetPoint = Player.Instance.transform.GetChild(0).position;       // Ÿ�� ���� = player
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Movedir), 0.1f);   // �̵��������� ȸ��
        }
        else
        {
            TargetPoint = PlayerBase.transform.position;                    // Ÿ�� ���� = base
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(PlayerBase.transform.position - this.transform.position), 0.1f);  // �̵��������� ȸ��
        }

        MovePos = Enemyrigid.position + Movedir * EnemyMoveSpeed * Time.deltaTime;  // �̵�
        Enemyrigid.MovePosition(MovePos);   // Enemyrigid.MovePosition()�� ���� �̵�
    }

    // ������ �ǰ�       (������(��Ʈ��ĵ))
    public virtual void EnemyDamage(int bulletATK)   //  �÷��̾�� ȣ��
    {
        if (EnemyCurHp > 0)
        {
            isMove = false; // �������� ����!
            isStun = true;
            EnemyMoveSpeed = 0; // ���� => �̼� = 0
            EnemyCurHp = EnemyCurHp - bulletATK;   // ȣ�� �� ������ ���� ���ݷ¸�ŭ hp���� ��´�
        }
    }

    //����� Scene üũ��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Moveradius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Attackradius);
    }
}
