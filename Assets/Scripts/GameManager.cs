using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameState currentGameState;
    public static GameManager instance;
    static int score;
    PlayerMecha playerControl;
    EnemyMecha enemyControl;
    public GameObject pantallaInicio, pausa, gameOver, creditos;
    [SerializeField] Text scoreGameOver, scorePlayer, gameOverText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Time.timeScale = 0;
        // Extrae el componente del jugador
        enemyControl = new EnemyMecha();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMecha>();
        //playerControl = new PlayerMecha();
    }

    private void Update()
    {
        // Se cambia el estado dependiendo de lo que suceda en el juego

        if(Input.GetButtonDown("Pausa")  && currentGameState == GameState.inGame)
        {
            // Pone el juego en pausa
            //currentGameState = GameState.pausa;
            Pause();
        }

        if(Input.GetButtonDown("Back") && currentGameState == GameState.pausa)
        {
            // Quita la pausa en caso de que vuelva a presionar el boton de pausa
            //currentGameState = GameState.inGame;
            Reanudar();
            pausa.SetActive(false);
        }

        // Verifica si sigues vivo o no
        if (!playerControl.GetIsAlive() && currentGameState == GameState.inGame)
        {
            Invoke("GameOver", 0.5f);
            playerControl.SetIsAlive(true);
        }
    }

    public void Pause()
    {
        SetGameState(GameState.pausa);
        //currentGameState = GameState.pausa;
    }

    public void Reanudar()
    {
        SetGameState(GameState.inGame);
        //currentGameState = GameState.inGame;
    }

    public void StartGame()
    {
        SetGameState(GameState.inGame);
        //currentGameState = GameState.inGame;
    }

    public void ExitGame()
    {
        //Invoke("AuxMenu", 0.2f);
        AuxMenu();
    }

    void AuxMenu()
    {
        SetGameState(GameState.inicio);
    }

    public void GameOver()
    {
        // Muestra la pantalla de game over
        SetGameState(GameState.gameOver);
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void VictoryGame()
    {
        // Partida ganada
        SetGameState(GameState.victory);
    }

    public void Creditos()
    {
        SetGameState(GameState.creditos);
    }

    public void SetScore(int setScore)
    {
        score = setScore;
    }

    //* ******NOTA: Checar bien el game manager que de pronto vale madre****** *//

    public void SetGameState(GameState newGameState)
    {
        if(newGameState == GameState.inicio)
        {
            // Cuando este en la pantalla de inicio, restaurar todas las cosas
            // Programar la logica del menu
            Time.timeScale = 0;

            scorePlayer = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
            scorePlayer.text = "Score: " + 0;
            pantallaInicio.SetActive(true);
            gameOver.SetActive(false);
            pausa.SetActive(false);
            creditos.SetActive(false);
            enemyControl.SetScore_Enemy(0);
            LevelManager.instance.RemoveAllLevelBlocks();
            LevelManager.instance.GenerateInitialBlocks();
            playerControl.SetIsAlive(true);
        }
        else if(newGameState == GameState.inGame)
        {
            Time.timeScale = 1;
        }
        else if(newGameState == GameState.pausa)
        {
            Time.timeScale = 0;
            pausa.SetActive(true);
        }
        else if(newGameState == GameState.gameOver)
        {
            //Time.timeScale = 0;
            gameOver.SetActive(true);
            scoreGameOver = GameObject.FindGameObjectWithTag("GOScore").GetComponent<Text>();
            scorePlayer = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
            gameOverText = GameObject.FindGameObjectWithTag("GOText").GetComponent<Text>();
            gameOverText.text = "GAME OVER";
            scoreGameOver.text = scorePlayer.text;
        }
        else if(newGameState == GameState.victory)
        {
            // Pantalla de victoria
            //Time.timeScale = 0;
            
            gameOver.SetActive(true);
            scoreGameOver = GameObject.FindGameObjectWithTag("GOScore").GetComponent<Text>();
            scorePlayer = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
            gameOverText = GameObject.FindGameObjectWithTag("GOText").GetComponent<Text>();
            gameOverText.text = "VICTORY";
            score *= 5;
            scoreGameOver.text = "Score: " + score;
        }
        else if(newGameState == GameState.creditos)
        {
            // Pantalla de creditos
            Time.timeScale = 0;
            creditos.SetActive(true);
        }

        currentGameState = newGameState;
    }
}

public enum GameState
{
    inicio,
    inGame,
    pausa,
    gameOver,
    victory,
    creditos
}
