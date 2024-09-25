using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;     // 게임매니저 싱글톤
    public static GameManager Instance
    {
        get { return instance; }
    }

    [Header("라플 가격")]
    public TMP_Text RifleATKprice = null;
    [Header("이속 가격")]
    public TMP_Text MoveSpeedprice = null;
    [Header("유탄 가격")]
    public TMP_Text LauncherATKprice = null;
    [Header("최대체력 가격")]
    public TMP_Text MaxHPprice = null;
    [Header("체력재생 가격")]
    public TMP_Text Healprice = null;
    [Header("탄창 가격")]
    public TMP_Text Magazineprice = null;
    [Header("재화")]
    public int Property = 0;        // 게임 재화
    [Header("라운드 & 킬")]
    public int Round = 0;       // 라운드
    public int ConditionRound = 0;  // 라운드 상승조건 (킬)
    public int Kill = 0;    // UI표기되는 kill
    public int CountKill = 0;  // 카운팅을 위한 kill 변수

    public bool isGameover = false; // 게임오버 여부 bool
    private string Rifle_price = string.Empty;      // 라이플 업그레이드 가격
    private string Launcher_price = string.Empty; // 유탄 업그레이드 가격
    private string Move_price = string.Empty;   // 이동 업그레이드 가격
    private string MaxHp_price = string.Empty; // 최대체력 업그레이드 가격
    private string Magazine_price = string.Empty;   // 탄창 업그레이드 가격
    private string Heal_price = string.Empty;       // 체력회복 재생량 업그레이드 가격
    private float Endcount = 0;         // time.deltatime 값을 더해갈 변수
    private int EndTextcount = 3;     // 남은 종료시간  

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        isGameover = false;
        Cursor.lockState = CursorLockMode.Locked; // 커서를 화면 중앙에 고정
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

    // 게임오버
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

    // 라운드
    public void Roundcount()
    {
        if (CountKill > ConditionRound)
        {
            Round += 1;
            CountKill = 0;
        }
    }

    // 가격
    private void StringInit()
    {
        Rifle_price = RifleATKprice.text;
        Move_price = MoveSpeedprice.text;
        Launcher_price = LauncherATKprice.text;
        MaxHp_price = MaxHPprice.text;
        Magazine_price = Magazineprice.text;
        Heal_price = Healprice.text;
    }

    // 커서 화면중앙 고정
    private void AimStatus()
    {
        // ESC를 누르면 커서가 화면 중앙에 고정되는 것을 해제
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // 클릭 시 다시 커서를 화면 중앙에 고정
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // 재화 획득
    public void GetMoney()
    {
        int addmoney = Random.Range(1, 5);
        Property = Property + addmoney;
        UIManager.Instance.Property_UI();
    }

    // 플레이어 이동속도 증가
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

    // 플레이어 라이플 공격력 증가
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

    // 유탄 공격력 증가
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

    // 최대 체력 증가
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

    // 체력 증가량 증가
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

    // 탄창 증가
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
