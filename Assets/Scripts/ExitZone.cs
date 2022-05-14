using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Para destruir el bloque anterior
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelManager.instance.AddLevelBlock();
            LevelManager.instance.RemoveLevelBlock();
        }
    }
}