using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMecha playerMecha = collision.GetComponent<PlayerMecha>();
            playerMecha.SetIsAlive(false);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
