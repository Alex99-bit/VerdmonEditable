using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMecha : MonoBehaviour
{
    static int score, indice;
    Rigidbody2D rigidEnemy;
    CharacterStats statsEnemy;
    Animator animator;
    //public LayerMask playerMask;
    public ParticleSystem sangre;
    Collider2D enemyCollider;
    Text scorePlayer; // Esta variable se programa aqui, porque aqui esta la logica de muerte de cada enemigo
    bool enemyDead;

    const string IS_ON_THE_GROUND = "isOnTheGround", IS_ALIVE = "isAlive", IS_RUNNING = "isRunning", IS_SHOOTING = "isShooting";

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.currentGameState == GameState.inGame)
        {
            enemyCollider = GetComponent<Collider2D>();
            animator = GetComponent<Animator>();
            rigidEnemy = GetComponent<Rigidbody2D>();
            scorePlayer = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
            statsEnemy = new CharacterStats("SoldierEnemy", "Soldado frances");
            enemyCollider.enabled = true;
            enemyDead = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.instance.currentGameState == GameState.inGame)
        {
            if (enemyDead && score <= indice * 25)
            {
                // Esto esta creado simplemente para darle una animacion cuando se suma el score
                score++;
                if (score > indice * 25)
                {
                    score = indice * 25;
                }
                scorePlayer.text = "Score: " + score;

            }
        }
        else if(GameManager.instance.currentGameState == GameState.inicio)
        {
            score = 0;
            scorePlayer.text = "Score: " + score;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            // Se le resta vida en caso de que reciba un disparo
            statsEnemy.setVida(statsEnemy.getVida() - 34f);
            sangre.Play();
        }

        if (statsEnemy.getVida() <= 0)
        {
            // Muere el enemigo
            enemyCollider.enabled = false;
            animator.SetBool(IS_ALIVE, false);
            rigidEnemy.gravityScale = 0.5f;
            Destroy(this.gameObject, 0.5f);
            enemyDead = true;
            indice++;
        }
    }

    public int GetScore_Enemy()
    {
        return score;
    }

    public void SetScore_Enemy(int externScore)
    {
        score = externScore;
    }
}
