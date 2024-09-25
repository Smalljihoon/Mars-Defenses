using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;     // ���ӸŴ��� �̱���
    public static GameManager Instance
    {
        get { return instance; }
    }

    [Header("���� ����")]
    public TMP_Text RifleATKprice = null;
    [Header("�̼� ����")]
    public TMP_Text MoveSpeedprice = null;
    [Header("��ź ����")]
    public TMP_Text LauncherATKprice = null;
    [Header("�ִ�ü�� ����")]
    public TMP_Text MaxHPprice = null;
    [Header("ü����� ����")]
    public TMP_Text Healprice = null;
    [Header("źâ ����")]
    public TMP_Text Magazineprice = null;
    [Header("��ȭ")]
    public int Property = 0;        // ���� ��ȭ
    [Header("���� & ų")]
    public int Round = 0;       // ����
    public int ConditionRound = 0;  // ���� ������� (ų)
    public int Kill = 0;    // UIǥ��Ǵ� kill
    public int CountKill = 0;  // ī������ ���� kill ����

    public bool isGameover = false; // ���ӿ��� ���� bool
    private string Rifle_price = string.Empty;      // ������ ���׷��̵� ����
    private string Launcher_price = string.Empty; // ��ź ���׷��̵� ����
    private string Move_price = string.Empty;   // �̵� ���׷��̵� ����
    private string MaxHp_price = string.Empty; // �ִ�ü�� ���׷��̵� ����
    private string Magazine_price = string.Empty;   // źâ ���׷��̵� ����
    private string Heal_price = string.Empty;       // ü��ȸ�� ����� ���׷��̵� ����
    private float Endcount = 0;         // time.deltatime ���� ���ذ� ����
    private int EndTextcount = 3;     // ���� ����ð�  

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        isGameover = false;
        Cursor.lockState = CursorLockMode.Locked; // Ŀ���� ȭ�� �߾ӿ� ����
        Cursor.visible = false;
    }

    void Update()
    {
        UIManager.Instance.Property_UI();
        UIManager.Instance.Stat_Info();
        UIManager.Instance.OptionOnActice();
        UIManager.Instance.GameCount();
        StringInit();
        AimStatus();
        Roundcount();


        if (isGameover)
        {
            Endcount += Time.deltaTime;
            if (Endcount > 1f)
            {
                Endcount = 0;
                EndTextcount -= 1;
            }
        }

        if (UIManager.Instance.isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Player.Instance.curHP <= 0 || Base.Instance.curBaseHp <= 0)
        {
            UIManager.Instance.GameOverPanel.SetActive(true);
            isGameover = true;
            Gameover();
        }
    }

    // ���ӿ���
    public void Gameover()
    {
        UIManager.Instance.EndTime.text = EndTextcount.ToString();

        if(EndTextcount < 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("Start");
        }
    }

    // ����
    public void Roundcount()
    {
        if (CountKill > ConditionRound)
        {
            Round += 1;
            CountKill = 0;
        }
    }

    // ����
    private void StringInit()
    {
        Rifle_price = RifleATKprice.text;
        Move_price = MoveSpeedprice.text;
        Launcher_price = LauncherATKprice.text;
        MaxHp_price = MaxHPprice.text;
        Magazine_price = Magazineprice.text;
        Heal_price = Healprice.text;
    }

    // Ŀ�� ȭ���߾� ����
    private void AimStatus()
    {
        // ESC�� ������ Ŀ���� ȭ�� �߾ӿ� �����Ǵ� ���� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Ŭ�� �� �ٽ� Ŀ���� ȭ�� �߾ӿ� ����
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // ��ȭ ȹ��
    public void GetMoney()
    {
        int addmoney = Random.Range(1, 5);
        Property = Property + addmoney;
        UIManager.Instance.Property_UI();
    }

    // �÷��̾� �̵��ӵ� ����
    public void MoveSpeedIncrease()
    {
        int price = int.Parse(Move_price);
        if (Property >= price)
        {
            Player.Instance.MoveSpeed = Player.Instance.MoveSpeed + 2;
            Property = Property - price;
            price += 2;
            string sendprice = price.ToString();
            MoveSpeedprice.text = sendprice;
        }
    }

    // �÷��̾� ������ ���ݷ� ����
    public void RifleATKIncrease()
    {
        int price = int.Parse(Rifle_price);
        if (Property >= price)
        {
            Player.Instance.RifleATK = Player.Instance.RifleATK + 5;
            Property = Property - price;
            price += 5;
            string sendprice = price.ToString();
            RifleATKprice.text = sendprice;
        }
    }

    // ��ź ���ݷ� ����
    public void LauncherATKIncrease()
    {
        int price = int.Parse(Launcher_price);
        if (Property >= price)
        {
            Player.Instance.LauncherATK = Player.Instance.LauncherATK + 5;
            Property = Property - price;
            price += 5;
            var sendprice = price.ToString();
            LauncherATKprice.text = sendprice;
        }
    }

    // �ִ� ü�� ����
    public void MaxHpIncrease()
    {
        int price = int.Parse(MaxHp_price);
        if (Property >= price)
        {
            Player.Instance.maxHP = Player.Instance.maxHP + 50;
            Property = Property - price;
            price += 5;
            string sendprice = price.ToString();
            MaxHPprice.text = sendprice;
        }
    }

    // ü�� ������ ����
    public void HpHealIncrease()
    {
        int price = int.Parse(Heal_price);
        if (Property >= price)
        {
            Player.Instance.healAmount = Player.Instance.healAmount + 5;
            Property = Property - price;
            price += 5;
            string sendprice = price.ToString();
            Healprice.text = sendprice;
        }
    }

    // źâ ����
    public void MagazineIncrease()
    {
        int price = int.Parse(Magazine_price);
        if (Property >= price)
        {
            Player.Instance.max_magazine = Player.Instance.max_magazine + 10;
            Property = Property - price;
            price += 5;
            string sendprice = price.ToString();
            Magazineprice.text = sendprice;
        }
    }
}
