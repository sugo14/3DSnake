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


    // Start is called before the first frame update
    void Start()
    {
        currState = State.Menu;
    }

    void ResetGame() {
        snakeHead.GetComponent<SnakeHeadScript>().Reset();
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
            if (snakeHead.GetComponent<SnakeHeadScript>().snakeBody.ToArray().Any(
                    x => x.transform.position == snakeHead.transform.position)) {
                gameOverText.transform.GetChild(0).GetComponent<TMP_Text>().text = snakeHead.GetComponent<SnakeHeadScript>().currLength.ToString();
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

    void UpdateGame() {
        if (currState == State.Game) {
            snakeHead.GetComponent<SnakeHeadScript>().Show();
            food.SetActive(true);
            gold.SetActive(true);
            scoreText.SetActive(true);
            scoreText.GetComponent<TMP_Text>().text = snakeHead.GetComponent<SnakeHeadScript>().currLength.ToString();
            gameOverText.SetActive(false);
            pauseText.SetActive(false);
            menuText.SetActive(false);
        }
        if (currState == State.GameOver) {
            snakeHead.GetComponent<SnakeHeadScript>().Hide();
            food.SetActive(false);
            gold.SetActive(false);
            scoreText.SetActive(false);
            gameOverText.SetActive(true);
            pauseText.SetActive(false);
            menuText.SetActive(false);
        }
        if (currState == State.Pause) {
            snakeHead.GetComponent<SnakeHeadScript>().Hide();
            food.SetActive(false);
            gold.SetActive(false);
            scoreText.SetActive(false);
            gameOverText.SetActive(false);
            pauseText.SetActive(true);
            menuText.SetActive(false);
        }
        if (currState == State.Menu) {
            snakeHead.GetComponent<SnakeHeadScript>().Hide();
            food.SetActive(false);
            gold.SetActive(false);
            scoreText.SetActive(false);
            gameOverText.SetActive(false);
            pauseText.SetActive(false);
            menuText.SetActive(true);
        }
    }

    void Update()
    {
        UpdateState();
        UpdateGame();
    }
}
