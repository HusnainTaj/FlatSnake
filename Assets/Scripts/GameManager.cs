using UnityEngine;
using UnityEngine.UI;
using SimpleHTTP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
	private bool LeaderboardEnabled = true;

    private API NGAPI;
    private bool NGAPIEnabled = false;

    public GameObject snake;
    public GameObject snakeHead;
    public GameObject snakeTails;
    public GameObject snakeTailPrefab;
    public GameObject foodPrefab;


    private int DifficultyLevel;
    private int score = 0;
    private SnakeSprites snakeSprites;
    private bool gameStarted = false;
    private bool gameEnded = false;

    public GameObject gameNotStartedPanel;
    public Text scoreText;
    public GameObject deadPanel;

    public string submitScoreUrl;

    private void Awake()
    {
        if(NGAPIEnabled) NGAPI = GameObject.FindGameObjectWithTag("NewgroundsAPI").GetComponent<API>();
        DifficultyLevel = PlayerPrefs.GetInt(PlayerPrefsVariables.Difficulty, Difficulty.medium);
    }

    private void Start()
    {
        GenerateFood();
        snakeSprites = FindObjectOfType<SnakeSprites>();
        scoreText.text = score.ToString();

        if(NGAPIEnabled) 
        {
            StartCoroutine(NGAPI.Connect());
            StartCoroutine(NGAPI.GetMedals());
            StartCoroutine(NGAPI.PreloadSettings());
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown && !gameStarted)
        {
            gameNotStartedPanel.SetActive(false);
            startSnake();
            gameStarted = true;
        }

        if (Input.GetKeyDown(KeyCode.R) && gameEnded)
        {
            FindObjectOfType<ButtonManager>().RestartBtn();
        }
    }

    public void CollectFood(GameObject CollectedFood)
    {
        Destroy(CollectedFood);

        AddTail();

        IncrementScore(DifficultyLevel);

        GenerateFood();
    }

    private void AddTail()
    {
        GameObject tail = Instantiate(snakeTailPrefab, new Vector2(-1000, 0), Quaternion.identity);
        tail.transform.SetParent(snakeTails.transform);
    }

    public void Die()
    {
        stopSnake();
        changeSnakeSprite(snakeSprites.snakeHeadDead, snakeSprites.snakeTailDead);
        deadPanel.transform.Find("ScoreText").GetComponent<Text>().text = getScore();
        deadPanel.SetActive(true);
        gameEnded = true;

        if(NGAPIEnabled) UnlockMedal(score);

        if (score > PlayerPrefs.GetInt(PlayerPrefsVariables.HighScore, 0))
        {
            PlayerPrefs.SetInt(PlayerPrefsVariables.HighScore, score);
            if(NGAPIEnabled) SubmitScoreNG(score);
            deadPanel.transform.Find("NewHighScore").gameObject.SetActive(true);
        }

		if(LeaderboardEnabled) StartCoroutine(SubmitScore(score));
    }

    private void UnlockMedal(int score)
    {
        if (score >= 4)
            StartCoroutine(NGAPI.UnlockMedal("Getting Started"));
        if (score >= 20)
            StartCoroutine(NGAPI.UnlockMedal("Getting Serious"));
        if (score >= 30)
            StartCoroutine(NGAPI.UnlockMedal("Having Fun"));
        if (score >= 40)
            StartCoroutine(NGAPI.UnlockMedal("Getting Better"));
        if (score >= 50)
            StartCoroutine(NGAPI.UnlockMedal("Taking snake to next level"));
        if (score >= 60)
            StartCoroutine(NGAPI.UnlockMedal("Snake Level 1"));
        if (score >= 75)
            StartCoroutine(NGAPI.UnlockMedal("Snake Level 2"));
        if (score >= 90)
            StartCoroutine(NGAPI.UnlockMedal("Snake Level 3"));
        if (score >= 100)
            StartCoroutine(NGAPI.UnlockMedal("Snake Level 4"));
        if (score >= 120)
            StartCoroutine(NGAPI.UnlockMedal("Snake Level 5"));
        if (score >= 200)
            StartCoroutine(NGAPI.UnlockMedal("Snake Expert"));
        if (score >= 250)
            StartCoroutine(NGAPI.UnlockMedal("Snake Master"));
    }

	private IEnumerator SubmitScore(int score)
    {

		Debug.Log ("SubmitScoreasd");

		string dificulty;
		switch (PlayerPrefs.GetInt (PlayerPrefsVariables.Difficulty, Difficulty.medium)) {
		case Difficulty.easy:
			dificulty = PlayerPrefsVariables.LBEasyDifficulty;
			break;

		case Difficulty.medium:
			dificulty = PlayerPrefsVariables.LBMediumDifficulty;
			break;

		case Difficulty.hard:
			dificulty = PlayerPrefsVariables.LBHardDifficulty;
			break;
		default:
			dificulty = PlayerPrefsVariables.LBMediumDifficulty;
			break;
		}

		LBScore lbscore = new LBScore (PlayerPrefs.GetString(PlayerPrefsVariables.Username, ""), dificulty, score);

		using (UnityWebRequest www = UnityWebRequest.Put ("http://node.perspective-v.com/score", JsonUtility.ToJson (lbscore))) {
			www.SetRequestHeader("Content-Type", "application/json");
			yield return www.Send ();
			Debug.Log (www.responseCode);
			if (www.isError) {
				Debug.Log (www.error);
			} else {
				Debug.Log ("Form upload complete!");
			}
		}
    }

	private void SubmitScoreNG(int score)
	{
		switch (PlayerPrefs.GetInt(PlayerPrefsVariables.Difficulty, Difficulty.medium))
		{
		case Difficulty.easy:
			StartCoroutine(NGAPI.PostScore("Easy", score));
			break;

		case Difficulty.medium:
			StartCoroutine(NGAPI.PostScore("Medium", score));
			break;

		case Difficulty.hard:
			StartCoroutine(NGAPI.PostScore("Hard", score));
			break;
		}
	}

    public void GenerateFood()
    {
        float x = Mathf.Round(UnityEngine.Random.Range(-6f, 6f));
        float y = Mathf.Round(UnityEngine.Random.Range(-4.9f, 3.9f));

        if (x % 2 != 0)
        {
            if (x < 0)
                x -= .4f;
            else if (x >= 0)
                x += .4f;
        }
        else
        {
            if (x < 0)
                x -= .2f;
            else if (x >= 0)
                x += .2f;
        }

        if (y % 2 != 0)
        {
            if (y < 0)
                y -= .5f;
            else if (y >= 0)
                y += .3f;
        }
        else
        {
            if (y < 0)
                y -= .3f;
            else if (y >= 0)
                y += .5f;
        }

        x = Mathf.Clamp(x, -5.8f, 5.8f);
        y = Mathf.Clamp(y, -4.7f, 3.7f);

        Instantiate(foodPrefab, new Vector2(x, y), Quaternion.identity);
    }

    public void startSnake()
    {
        snakeHead.GetComponent<SnakeHead>().enabled = true;
    }

    public void stopSnake()
    {
        snakeHead.GetComponent<SnakeHead>().enabled = false;
    }

    void changeSnakeSprite(Sprite head, Sprite tail)
    {
        snakeHead.GetComponent<SpriteRenderer>().sprite = head;

        SpriteRenderer[] tailsSprite = snakeTails.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer tailSprite in tailsSprite)
        {
            tailSprite.sprite = tail;
        }
    }

    void IncrementScore(int difficultyLevel)
    {
        switch (difficultyLevel)
        {
            case Difficulty.easy:
                score += 1;
                break;

            case Difficulty.medium:
                score += 2;
                break;

            case Difficulty.hard:
                score += 4;
                break;
        }

        scoreText.text = getScore();
    }

    public string getScore()
    {
        return score.ToString();
    }
}
