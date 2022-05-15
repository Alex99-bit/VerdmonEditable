using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMecha : MonoBehaviour
{
    Rigidbody2D rigidPlayer;
    CharacterStats statsPlayer;
    public GameObject canon, vel, salto;
    Animator animator;
    public GameObject bullet, mensaje, bossbar;
    public LayerMask playerMask;
    public ParticleSystem humo, destellos, sangre, powerParticle;
    static int pwup;
    public Slider barVida, barMunicion;
    public Transform startPosition;
    bool recover, recarga, powerUp, jump;//**//
    float cooldown ,cooldownUP;
    static bool puedeDisparar, isAlive, muestraMensaje;
    public Text municion, botellas; 
    AudioManager audioManager;
    static int points;

    // Estados del jugador
    const string IS_ON_THE_GROUND = "isOnTheGround", IS_ALIVE = "isAlive", IS_RUNNING = "isRunning", IS_SHOOTING = "isShooting";

    // Start is called before the first frame update
    void Start()
    {
        statsPlayer = new CharacterStats("Player", "Anton Doe");
        rigidPlayer = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        barVida = GameObject.FindGameObjectWithTag("BarraVida").GetComponent<Slider>();
        //barMunicion = GetComponent<Slider>();
        //municion = GetComponent<Text>();
        mensaje = GameObject.FindGameObjectWithTag("Mensaje");
        recover = true;
        recarga = false;
        puedeDisparar = true;
        muestraMensaje = true;
        cooldown = 0;
        cooldownUP = 0;
        pwup = 0;
        points = 0;
        powerUp = false;
        isAlive = true;
        //**//
        jump = false;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.inGame)
        {

            Shooting();
            Movement();
            Jump();

            Debug.DrawRay(this.transform.position, Vector2.down * 1.95f, Color.red);
            Debug.DrawRay(this.transform.position, Vector2.left * 0.6f, Color.red);
            Debug.DrawRay(this.transform.position, Vector2.right * 0.6f, Color.red);

            if (statsPlayer.getVida() <= 0 || !isAlive)
            {
                // Si el jugador muere
                animator.SetBool(IS_ALIVE, false);
                isAlive = false;
                vel.SetActive(false);
                salto.SetActive(false);
                // Pantalla de game over
                //GameManager.instance.currentGameState = GameState.gameOver;
                //GameManager.instance.GameOver();
                //Invoke("Revive", 1f); // De momento revive al player en el punto de reaparicion 
            }

            if (muestraMensaje)
            {
                Invoke("OcultarMensaje", 6f);
            }

            if (recover && statsPlayer.getVida() <= 70)
            {
                // Recarga la vida poco a poco en caso de no recivir daño
                statsPlayer.setVida(statsPlayer.getVida() + 0.5f);
                barVida.value = statsPlayer.getVida();

                if(statsPlayer.getVida() > 70)
                {
                    statsPlayer.setVida(70);
                    barVida.value = statsPlayer.getVida();
                }
            }

            // Codigo para invocar al jefazo
            if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.D) && Input.GetKeyDown(KeyCode.F))
            {
                points = 10;
                botellas.text = "Botellas: " + points + " / 10";
            }

            if (!recover)
            {
                // Genera un tiempo de espera para recargar la vida en caso de que no se reciva daño
                cooldown += Time.deltaTime;
                if (cooldown > 6f) // Cooldown de n segundos
                {
                    recover = true;
                    cooldown = 0;
                }
            }

            if (points >= 10)
            {
                bossbar.SetActive(true);
            }

            if (Input.GetButton("Recarga"))
            {
                // Se activa la recarga
                recarga = true;
            }



            // Si hace un salto o simplemente no esta tocando el suelo, no puede recargar
            if (!IsTouchingTheGround())
            {
                recarga = false;
            }            

            /*Para el power up solo hacer un cool down como con la vida pero a la inversa*/
            // Hace valido el power up
            statsPlayer.setPowerUp(pwup);
            // Despues de x tiempo, regresa a 0 el power up
            cooldownUP += Time.deltaTime;
            if (cooldownUP > 15f && powerUp)
            {
                statsPlayer.ActivaPowerUp(0);
                pwup = 0;
                cooldownUP = 0;
                powerUp = false;
                vel.SetActive(false);
                salto.SetActive(false);
                print("Si se mato perro");
            }
        }
        else if(GameManager.instance.currentGameState == GameState.inicio)
        {
            // Reiniciar todo
            Revive();
            MostrarMensaje("A & D = Movimiento\nR = Recarga\nEspacio = Salto\nEspacio x2 = Doble salto\nClick Izq o K = Disparar");
        }

    }

    private void FixedUpdate()
    {
        if(GameManager.instance.currentGameState == GameState.inGame)
        {
            // Mecanicas basicas del jugador
            /***/
            if (jump)
            {
                // Hace que el personaje se eleve aplicando la fuerza de salto
                rigidPlayer.AddForce(Vector2.up * statsPlayer.getJumpForce(), ForceMode2D.Impulse);
                jump = false;
            }

            // Mueve al personaje aplicando una aceleración
            rigidPlayer.velocity = new Vector2(Input.GetAxis("Horizontal") * statsPlayer.getSpeed(), rigidPlayer.velocity.y);
            /***/
        }

        // Invoca la funcion para recargar solo en caso de que se pueda recargar
        if (recarga)
        {
            RecargaBalas();
        }
    }

    // Sirve para pasar a otras clases el estado de las botellas, por medio de una variable estatica 
    public int GetPoint()
    {
        return points;
    }

    // Este metodo se manda a llamar en una slider event
    public void SonidoRecarga()
    {
        if (recarga) // Solo se produce si recarga esta activo (esta recargando)
        {
            // Si el numero es entero y esta recargando, suena la recarga
            audioManager.SelectAudio(2, 0.30f);
        }
    }

    void RecargaBalas()
    {

        if (vel.active)
        {
            // En caso de que el power up este activo, recarga más rapido
            statsPlayer.setMunicion(statsPlayer.getMunicion() + 0.16f);
        }
        else
        {
            // Metodo para recargar balas
            statsPlayer.setMunicion(statsPlayer.getMunicion() + 0.07f);
        }

        if (statsPlayer.getMunicion() >= 12)
        {
            recarga = false;
            statsPlayer.setMunicion(12);
            //audioManager.SelectAudio(2, 0.5f);
        }

        if(statsPlayer.getMunicion() > 0 && statsPlayer.getMunicion() <= 1 && recarga)
        {
            statsPlayer.setMunicion(1);
        }

        barMunicion.value = (int)statsPlayer.getMunicion();
        municion.text = (int)statsPlayer.getMunicion()+ " / 12";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "powerup velocity" || collision.name == "powerup velocity(Clone)")
        {
            powerUp = true; // Sirve para saber que tomo un power UP
            pwup = 2;
            cooldownUP = 0;
            Destroy(collision.gameObject);
            powerParticle.Play();
            vel.SetActive(true);
        }

        if(collision.name == "powerup jump" || collision.name == "powerup jump(Clone)")
        {
            powerUp = true;
            pwup = 3;
            cooldownUP = 0;
            Destroy(collision.gameObject);
            powerParticle.Play();
            salto.SetActive(true);
        }

        if (collision.gameObject.CompareTag("BulletEnemy")){
            recover = false;
            // Le resta vida al jugador desde el manager de personajes
            statsPlayer.setVida(statsPlayer.getVida() - 8.5f);
            sangre.Play();
            barVida.value = statsPlayer.getVida(); // Se actualiza la barra de vida
        }

        if (collision.gameObject.CompareTag("Monster"))
        {
            // Hacer logica para las bolas de acido del monstruo
            recover = false;
            // Le resta vida al jugador desde el manager de personajes
            statsPlayer.setVida(statsPlayer.getVida() - 35.5f);
            sangre.Play();
            barVida.value = statsPlayer.getVida(); // Se actualiza la barra de vida
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Points"))
        {
            points++;
            botellas.text = "Botellas: " + points + " / 10";
            Destroy(collision.gameObject);
            if(statsPlayer.getVida() < 100) // Recarga 20 pts de vida y activa la regeneracion
            {
                recover = true;
                statsPlayer.setVida(statsPlayer.getVida() + 20);
                if(statsPlayer.getVida() > 100)
                {
                    statsPlayer.setVida(100);
                }
                barVida.value = statsPlayer.getVida();
            }
            powerParticle.Play();
        }

        if (collision.collider.CompareTag("BulletEnemy"))
        {
            // Si toca o colisona con una bola de acido, le baja vida
            recover = false;
            // Le resta vida al jugador desde el manager de personajes
            statsPlayer.setVida(statsPlayer.getVida() - 25f);
            sangre.Play();
            barVida.value = statsPlayer.getVida(); // Se actualiza la barra de vida
        }

        if (collision.collider.CompareTag("Monster"))
        {
            // Si toca al monstruo le baja vida
            recover = false;
            // Le resta vida al jugador desde el manager de personajes
            statsPlayer.setVida(statsPlayer.getVida() - 35.5f);
            sangre.Play();
            barVida.value = statsPlayer.getVida(); // Se actualiza la barra de vida
        }
    }

    bool NoTocaPared()
    {
        // Funcion que checa si esta o no colisionando con una pared para frenar la animacion
        // en caso de que retorne false, es que esta tocando una pared (negacion de no toca pared)
        if(Physics2D.Raycast(this.transform.position, Vector2.left, 0.5f, playerMask))
        {
            return false;
        }
        else if(Physics2D.Raycast(this.transform.position, Vector2.right, 0.5f, playerMask))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void Revive()
    {
        // Revive al personaje con sus atributos 
        bossbar.SetActive(false);
        points = 0;
        botellas.text = "Botellas: " + points + " / 10";
        this.transform.position = startPosition.position;
        animator.SetBool(IS_ALIVE, true);
        //destellos.Play();
        isAlive = true;
        statsPlayer.setMunicion(10);
        statsPlayer.setPowerUp(0);
        pwup = 0;
        vel.SetActive(false);
        salto.SetActive(false);
        municion.text = statsPlayer.getMunicion() + " / 12";
        barMunicion.value = statsPlayer.getMunicion();
        statsPlayer.setVida(100);
        barVida.value = statsPlayer.getVida();
    }

    void Jump()
    {
        // Metodo para saltar
        
        
        if (Input.GetButtonDown("Jump") && IsTouchingTheGround())
        {
            // Manda la señal de que salta, para que se ejecute en un fixedupdate y no se bugue 
            jump = true;
        }
        

        animator.SetBool(IS_ON_THE_GROUND, IsTouchingTheGround());
    }

    void Movement()
    {
        // Metodo para moverse en horizontal
        if (Input.GetAxis("Horizontal") < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            animator.SetBool(IS_RUNNING, NoTocaPared());
        }
        else if(Input.GetAxis("Horizontal") > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animator.SetBool(IS_RUNNING, NoTocaPared());
        }
        else
        {
            animator.SetBool(IS_RUNNING, false);
        }
    }

    void Shooting()
    {
        // Metodo para disparar
        /* Hacer una mecanica para que se gasten las balas y el jugador tenga que recargar 
            que esto se refleje en la barra de fuego */
        if (Input.GetButtonDown("Fire1") && puedeDisparar)
        {
            recarga = false;
            Instantiate(bullet, canon.transform);
            animator.SetBool(IS_SHOOTING, true);
            statsPlayer.setMunicion(statsPlayer.getMunicion() - 1);
            
            if (statsPlayer.getMunicion() < 1)
            {
                statsPlayer.setMunicion(0);
            }
            barMunicion.value = (int)statsPlayer.getMunicion();
            municion.text = (int)statsPlayer.getMunicion() + " / 12";
            audioManager.SelectAudio(1, 0.4f);
        }
        else if(Input.GetButtonDown("Fire1") && !puedeDisparar)
        {
            // Si no tiene balas, suena el gatillazo
            audioManager.SelectAudio(0, 0.4f);
        }
        else
        {
            animator.SetBool(IS_SHOOTING, false);
        }

        // Como las balas son float, puede ser que quede con un numero irracional y le sirva como una bala,
        // por ende se comprueba si el valor es menor a 1 para clasificarla como cero y no puede disparar
        if (statsPlayer.getMunicion() < 1)
        {
            puedeDisparar = false;
        }
        else
        {
            puedeDisparar = true;
        }
    }
    
    bool IsTouchingTheGround()
    {
        // Metodo que retorna si el jugador esta tocando el suelo o no
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 3.75f, playerMask) && !salto.active)
        {
            return true;
        }
        else if(Physics2D.Raycast(this.transform.position, Vector2.down, 1.95f, playerMask) && salto.active)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Funcion que sirve para ocultar el mensaje, esta es llamada cada que se enciende un mensaje y tarda cierto tiempo en ejecutarse
    public void OcultarMensaje()
    {
        mensaje.SetActive(false);
        muestraMensaje = false;
    }

    // Funcion o metodo que se utiliza para generar un mensaje
    public void MostrarMensaje(string mensaje)
    {
        this.mensaje.SetActive(true);
        muestraMensaje = true;
        Text textMensaje = GameObject.FindGameObjectWithTag("TextoMensaje").GetComponent<Text>();
        textMensaje.text = mensaje;
    }

    //**// Metodos GET y SET para retornar a otras clases si el jugador esta vivo o muerto
    public bool GetIsAlive()
    {
        return isAlive;
    }

    public void SetIsAlive(bool alive)
    {
        isAlive = alive;
    }
    //**//

    public void setHumo()
    {
        humo.Play();
    }
}
