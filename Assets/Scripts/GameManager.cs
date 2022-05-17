using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤을 할당할 전역 변수

    public bool isGameover = false; // 게임 오버 상태
    public Text scoreText; // 점수를 출력할 UI 텍스트
    public Text LifeText; // 라이프를 출력

    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트
    
    public float LimitTime; // 성공 판별 시간
    public Text text_Timer; // 시간 출력 UI

    private int score = 0; // 게임 점수
    private int Life = 3;

    public int BossLife = 3;
    public bool isBossDead = false;

    private int total_Score = 0;
    public Text totalScore_Text;

    public void LoadNextScene()
    {
        total_Score += this.score;
        Scene scene = SceneManager.GetActiveScene();

        int curScene = scene.buildIndex;
        //Debug.Log("curScene : " + curScene);

        int nextScene = curScene + 1;

        SceneManager.LoadScene(nextScene);
    }


    // 게임 시작과 동시에 싱글톤을 구성
    void Awake()
    {
        // 싱글톤 변수 instance가 비어있는가?
        if (instance == null)
        {
            // instance가 비어있다면(null) 그곳에 자기 자신을 할당
            instance = this;
        }
        else
        {
            // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우

            // 씬에 두개 이상의 GameManager 오브젝트가 존재한다는 의미.
            // 싱글톤 오브젝트는 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (LimitTime > 0)
        {
            LimitTime -= Time.deltaTime;    // 타이머 종료시 성공 처리
            text_Timer.text = "time : " + Mathf.Round(LimitTime);
            LifeText.text = "Life : " + Life;
        }
        // 게임 오버 상태에서 게임을 재시작할 수 있게 하는 처리
        if (isGameover && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // 일정 시간이 지나면 성공 다음 씬으로 넘어가기 위해서..
        if (LimitTime <= 0 && !isGameover)
        {
            Debug.Log("In!!");
            LoadNextScene();
            //SceneManager.LoadScene("Level1-Boss");
        }
        if (isBossDead == true)
        {
            LoadNextScene();
        }
        totalScore_Text.text = "Total Score : " + total_Score;
    }

    // 점수를 증가시키는 메서드
    public void AddScore(int newScore)
    {
        if (!isGameover)
        {
            score += newScore;
            scoreText.text = "Score : " + score;
        }
    }

    public void SubLife(int discount)
    {
        if (!isGameover)
        {
            Life = Life - discount;
            LifeText.text = "Life : " + Life;
        }
        if (Life <= 0)
        {
            OnPlayerDead();
        }
    }

    public void Sub_BLife(int discount)
    {
        if (!isGameover)
        {
            BossLife = BossLife - discount;
            //BossLifeText.text = "Life : " + Life;
            if(BossLife <= 0)
            {
                isBossDead = true;
            }
        }
    }

    public int ReturnLife()
    {
        return this.Life;
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead()
    {
        isGameover = true;
        gameoverUI.SetActive(true);
    }
}