using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public enum State {
    Menu,
    Game,
    GameOver,
    Pause
}

public class StateManagerScript : MonoBehaviour
{
    public State currState;

    public GameObject snakeHead;
    public GameObject food;
    public GameObject gold;
    public GameObject scoreText;
    public GameObject gameOverText;
    public GameObject pauseText;
    public GameObject menuText;
    public GameObject abilities;

    SnakeManager snakeManager;


    // Start is called before the first frame update
    void Start()
    {
        currState = State.Menu;
        snakeManager = snakeHead.GetComponent<SnakeManager>();
    }

    void ResetGame() {
        snakeManager.Reset();
        snakeHead.transform.position = Vector3.zero;
        food.GetComponent<FoodScript>().RandomizeOrient();
    }

    // Update is called once per frame
    void UpdateState()
    {
        if (currState == State.Menu) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                currState = State.Game;
            }
        }
        else if (currState == State.Game) {
            if (!snakeManager.snakeMove.isInvincible && 
                snakeManager.snakeMove.snakeBody.ToArray().Any(
                    x => x.transform.position == snakeHead.transform.position))
            {
                gameOverText.transform.GetChild(0).GetComponent<TMP_Text>().text = snakeManager.snakeMove.currLength.ToString();
                currState = State.GameOver;
                ResetGame();
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                currState = State.Pause;
            }
        }
        else if (currState == State.Pause) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                currState = State.Game;
            }
        }
        else if (currState == State.GameOver) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                currState = State.Menu;
            }
        }
    }

    void UpdateGame()
    {
        if (currState == State.Game)
        {
            snakeManager.snakeMove.Show();
            food.SetActive(true);
            gold.SetActive(true);
            scoreText.SetActive(true);
            scoreText.GetComponent<TMP_Text>().text = snakeManager.snakeMove.currLength.ToString();
            gameOverText.SetActive(false);
            pauseText.SetActive(false);
            menuText.SetActive(false);
            abilities.SetActive(true);
        }
        if (currState == State.GameOver)
        {
            snakeManager.snakeMove.Hide();
            food.SetActive(false);
            gold.SetActive(false);
            scoreText.SetActive(false);
            gameOverText.SetActive(true);
            pauseText.SetActive(false);
            menuText.SetActive(false);
            abilities.SetActive(false);
        }
        if (currState == State.Pause)
        {
            snakeManager.snakeMove.Hide();
            food.SetActive(false);
            gold.SetActive(false);
            scoreText.SetActive(false);
            gameOverText.SetActive(false);
            pauseText.SetActive(true);
            menuText.SetActive(false);
            abilities.SetActive(false);
        }
        if (currState == State.Menu)
        {
            snakeManager.snakeMove.Hide();
            food.SetActive(false);
            gold.SetActive(false);
            scoreText.SetActive(false);
            gameOverText.SetActive(false);
            pauseText.SetActive(false);
            menuText.SetActive(true);
            abilities.SetActive(false);
        }
    }

    void Update()
    {
        if (snakeManager == null) { snakeManager = snakeHead.GetComponent<SnakeManager>(); }
        UpdateState();
        UpdateGame();
    }
}
