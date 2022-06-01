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

    private int score = 0;
    private static int total_score = 0; // 게임 점수 총합
    private static int Life = 3;    // 라이프 

    public int BossLife = 3;
    public bool isBossDead = false;

    public Text totalScore_Text;

    public void LoadNextScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        int curScene = scene.buildIndex;
        //Debug.Log("curScene : " + curScene);

        int nextScene = curScene + 1;
        if (nextScene <= 10)
        {
            total_score += score;
            SceneManager.LoadScene(nextScene);
        }

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
            //SceneManager.LoadScene("Level-Boss");
        }
        if (isBossDead == true)
        {
            AddScore(3); // 점수 주고
            SubLife(-2); // 체력 주고
            LoadNextScene();
        }
        scoreText.text = "Score : " + score;
        LifeText.text = "Life : " + Life;

        // 마지막 씬
        totalScore_Text.text = "Total Score : " + total_score;
    }

    // 점수를 증가시키는 메서드
    public void AddScore(int newScore)
    {
        if (!isGameover)
        {
            score += newScore;
            //scoreText.text = "Score : " + score;
            Debug.Log(score);
        }
    }

    public void SubLife(int discount)
    {
        if (!isGameover)
        {
            Life = Life - discount;
            //LifeText.text = "Life : " + Life;
        }
        if (Life <= 0)
        {
            OnPlayerDead();
            // 라이프가 다 떨어지면 GameOver 화면으로.....
            SceneManager.LoadScene("GameOver");
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
        return Life;
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead()
    {
        isGameover = true;
        gameoverUI.SetActive(true);
    }

    //GameOverScene에서 사용.
    public void LoadGameOver()
    {
        Life = 3;   // 다시 라이프를 3으로...
        total_score = 0; // 총합 점수를 0으로...
        // 버튼을 누르면 타이틀화면으로....
        SceneManager.LoadScene(0);
    }
}