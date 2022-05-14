using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsTypes : MonoBehaviour
{
    // Esta clase sirve para controlar el tipo de arma que tiene el jugador
    int typeShooting, damage;
    string tipo;
    
    public GunsTypes(string type)
    {
        tipo = type;
        switch (tipo)
        {
            case "Fusil":
                typeShooting = 1;
                damage = 40;
                break;
            case "Rifle":
                typeShooting = 2;
                damage = 15;
                break;
            case "Shootgun":
                typeShooting = 3;
                damage = 60;
                break;
        }
    }

    public void setType(string type)
    {
        tipo = type;
        switch (tipo)
        {
            case "Fusil":
                typeShooting = 1;
                damage = 40;
                break;
            case "Rifle":
                typeShooting = 2;
                damage = 15;
                break;
            case "Shootgun":
                typeShooting = 3;
                damage = 60;
                break;
        }
    }

    public int getDamage()
    {
        return damage;
    }

    public int getTypeShooting()
    {
        return typeShooting;
    }
}
