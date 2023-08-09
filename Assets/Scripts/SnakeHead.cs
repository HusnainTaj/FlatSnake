using System.Collections;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public float step;
    public float stepDelay;

    private GameManager GM;
    private SnakeTail snakeTail;
    private byte SnakeDir = SnakeDirection.Up;

    // Used for fixing movement bug!
    private byte nextSnakeDir = SnakeDirection.Up;

    void Awake()
    {
        snakeTail = FindObjectOfType<SnakeTail>();
        GM = FindObjectOfType<GameManager>();

        SetDifficulty();
    }

    private void SetDifficulty()
    {
        switch (PlayerPrefs.GetInt(PlayerPrefsVariables.Difficulty, Difficulty.medium))
        {
            case Difficulty.easy:
                stepDelay = .1f; // .2f;
                break;

            case Difficulty.medium:
                stepDelay = .05f; // .1f;
                break;

            case Difficulty.hard:
                stepDelay = .025f; // .05f;
                break;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Move());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && SnakeDir != SnakeDirection.Down)
            nextSnakeDir = SnakeDirection.Up;
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && SnakeDir != SnakeDirection.Up)
            nextSnakeDir = SnakeDirection.Down;
        else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && SnakeDir != SnakeDirection.Right)
            nextSnakeDir = SnakeDirection.Left;
        else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && SnakeDir != SnakeDirection.Left)
            nextSnakeDir = SnakeDirection.Right;
    }

    IEnumerator Move()
    {
        while (true)
        {
            snakeTail.follow(transform.position);
            SnakeDir = nextSnakeDir;

            switch (SnakeDir)
            {
                case SnakeDirection.Up:
                    transform.position = new Vector2(transform.position.x, transform.position.y + step);
                    break;
                case SnakeDirection.Down:
                    transform.position = new Vector2(transform.position.x, transform.position.y - step);
                    break;
                case SnakeDirection.Left:
                    transform.position = new Vector2(transform.position.x - step, transform.position.y);
                    break;
                case SnakeDirection.Right:
                    transform.position = new Vector2(transform.position.x + step, transform.position.y);
                    break;
            }
            yield return new WaitForSeconds (stepDelay);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            GM.CollectFood(collision.gameObject);
        }
        else if (collision.tag == "SnakeTail")
        {
            GM.Die();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Grid")
        {
            GM.Die();
        }
    }
}
