using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase para usar en todos los personajes, instancia los valores para cada personaje
public class CharacterStats : MonoBehaviour
{
    float vida, golpe;
    float municion, powerUp;
    string nombre, characterType;
    float speed, jumpForce;

    // golpe, speed, jumpForce son variables que pueden cambiar si se tiene un power up

    public CharacterStats(string characterType,string name)
    {
        this.characterType = characterType;
        nombre = name;

        // De acuerdo al personaje, se elige los stats que tendra
        switch (characterType)
        {
            case "Player":
                vida = 100;
                golpe = 25;
                powerUp = 0;
                municion = 12;
                speed = 8.5f;
                jumpForce = 6f;
                break;
            case "SoldierEnemy":
                vida = 100;
                golpe = 25;
                powerUp = 0;
                municion = 1000;
                speed = 0.6f;
                jumpForce = 3f;
                break;
            case "Monster":
                vida = 1200;
                golpe = 35;
                powerUp = 0;
                municion = 30;
                speed = 0.9f;
                jumpForce = 2f;
                break;
            case "Boss":
                vida = 250;
                golpe = 40;
                powerUp = 0;
                municion = 1000;
                speed = 8f;
                jumpForce = 7.5f;
                break;
            default:
                Debug.Log("No existe el nombre: "+nombre);
                break;
        }
    }

    public void ActivaPowerUp(int pwup)
    {
        //Debug.Log("Si se ejecuta este pedo");
        switch (powerUp)
        {
            case 0:
                /*  No tiene power up, regresa al estado inicial
                    deacuerdo con el tipo de personaje que es   */
                speed = 8.5f;
                jumpForce = 7f;
                break;
            case 1:
                // Afecta al ataque con el pu�o
                //golpe *= 2;
                break;
            case 2:
                // Afecta a la velocidad
                speed = 13f;
                break;
            case 3:
                // Afecta la fuerza de salto
                jumpForce = 13.7f;
                break;
        }
    }


    // Metodos de extracci�n y manipulaci�n de datos internos (get & set)

    public void setVida(float vida)
    {
        this.vida = vida;
    }

    public void setMunicion(float municion)
    {
        this.municion = municion;
    }

    public void setPowerUp(int powerUp)
    {
        this.powerUp = powerUp;
        ActivaPowerUp(powerUp);
    }

    public string getCharacterType()
    {
        return characterType;
    }

    public float getMunicion()
    {
        return municion;
    }

    public float getVida()
    {
        return vida;
    }

    public float getGolpe()
    {
        return golpe;
    }

    public float getSpeed()
    {
        return speed;
    }

    public float getJumpForce()
    {
        return jumpForce;
    }

    public string getNombre()
    {
        return nombre;
    }

}
