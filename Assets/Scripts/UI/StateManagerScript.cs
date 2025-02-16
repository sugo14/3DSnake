using UnityEngine;
using TMPro;

public enum State {
    Menu,
    Game,
    GameOver,
    Pause
}

public class StateManager : MonoBehaviour
{
    public State currState;

    public GameObject snakeHead;
    public GameObject foodManager;
    public GameObject scoreText;
    public GameObject gameOverText;
    public GameObject pauseText;
    public GameObject menuText;
    public GameObject snakePreview;
    public GameObject abilities;
    public GameObject shop;
    public GameObject effectViews;
    public GameObject gamepad;
    public InputManager inputManager;
    public bool showGamepad = true;

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
        foodManager.GetComponent<FoodManager>().Reset();
        snakeManager.wallManager.GetComponent<WallScript>().ClearWalls();
    }

    public void MenuToGame() {
        SnakePreview snakePreviewScript = snakePreview.GetComponent<SnakePreview>();
        snakeManager.snakeSpecies.snakeSpecies = snakePreviewScript.speciesRegistry.speciesList[snakePreviewScript.currentSpeciesIndex];
        snakeManager.Reset();
        currState = State.Game;
        snakeManager.wallManager.GetComponent<WallScript>().InitializeAllWalls();
    }

    void UpdateState()
    {
        if (currState == State.Menu) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                MenuToGame();
            }
        }
        else if (currState == State.Game) {
            if (snakeManager.IsDead())
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
            if (Input.GetKeyDown(KeyCode.Space) || inputManager.currInput == InputType.Tap) {
                currState = State.Game;
            }
        }
        else if (currState == State.GameOver) {
            if (Input.GetKeyDown(KeyCode.Space) || inputManager.currInput == InputType.Tap) {
                currState = State.Menu;
            }
        }
    }

    void UpdateGame()
    {
        if (currState == State.Game)
        {
            snakeManager.snakeMove.Show();
            foodManager.SetActive(true);
            scoreText.SetActive(true);
            scoreText.GetComponent<TMP_Text>().text = snakeManager.snakeMove.currLength.ToString();
            gameOverText.SetActive(false);
            pauseText.SetActive(false);
            shop.SetActive(false);
            menuText.SetActive(false);
            abilities.SetActive(true);
            effectViews.SetActive(true);
            gamepad.SetActive(showGamepad);
        }
        if (currState == State.GameOver)
        {
            snakeManager.snakeMove.Hide();
            foodManager.SetActive(false);
            scoreText.SetActive(false);
            gameOverText.SetActive(true);
            pauseText.SetActive(false);
            shop.SetActive(false);
            menuText.SetActive(false);
            abilities.SetActive(false);
            effectViews.SetActive(false);
            gamepad.SetActive(false);
        }
        if (currState == State.Pause)
        {
            snakeManager.snakeMove.Hide();
            foodManager.SetActive(false);
            scoreText.SetActive(false);
            gameOverText.SetActive(false);
            /* pauseText.SetActive(true); */
            shop.SetActive(true);
            menuText.SetActive(false);
            abilities.SetActive(false);
            effectViews.SetActive(false);
            gamepad.SetActive(false);
        }
        if (currState == State.Menu)
        {
            snakeManager.snakeMove.Hide();
            foodManager.SetActive(false);
            scoreText.SetActive(false);
            gameOverText.SetActive(false);
            pauseText.SetActive(false);
            shop.SetActive(false);
            menuText.SetActive(true);
            abilities.SetActive(false);
            effectViews.SetActive(false);
            gamepad.SetActive(false);
        }
    }

    void Update()
    {
        if (snakeManager == null) { snakeManager = snakeHead.GetComponent<SnakeManager>(); }
        UpdateState();
        UpdateGame();
    }
}
