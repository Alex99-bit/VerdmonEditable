using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton
    public static LevelManager instance;

    // Listas
    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();
    public List<LevelBlock> currentLevelBlocks = new List<LevelBlock>();
    public Transform lvlStartPos;
    PlayerMecha player;
    static int i;
    bool boss;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        boss = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        player = new PlayerMecha();
        GenerateInitialBlocks();
    }

    private void Update()
    {
        if (player.GetPoint() >= 10)
        {
            // Que el siguiente nivel sea si o si el 4
            boss = true;
        }
    }

    public void AddLevelBlock()
    {
        int randomIdx = Random.Range(0, allTheLevelBlocks.Count - 1);

        LevelBlock block;

        Vector3 spawnPosition = Vector3.zero;

        if (boss)
        {
            randomIdx = 3;
            boss = false;
        }

        // Si aun no hay bloques de nivel, se añade el primero
        if (currentLevelBlocks.Count == 0)
        {
            block = Instantiate(allTheLevelBlocks[0]);
            spawnPosition = lvlStartPos.position;
            i++;
        }
        else
        {
            block = Instantiate(allTheLevelBlocks[randomIdx]);
            spawnPosition = currentLevelBlocks[currentLevelBlocks.Count - 1].exitPoint.position;
            i++;
        }

        if (i >= allTheLevelBlocks.Count)
        {
            i = 0;
        }

        // Para hacer que todos los bloques sean hijos de esta clase
        block.transform.SetParent(this.transform, false);

        Vector3 correction = new Vector3(spawnPosition.x - block.startPoint.position.x,
                                    spawnPosition.y - block.startPoint.position.y, 0);

        block.transform.position = correction;

        currentLevelBlocks.Add(block);
    }

    public void RemoveLevelBlock()
    {
        LevelBlock oldBlock = currentLevelBlocks[0];
        currentLevelBlocks.Remove(oldBlock);
        Destroy(oldBlock.gameObject);
    }

    public void RemoveAllLevelBlocks()
    {
        while (currentLevelBlocks.Count > 0)
        {
            RemoveLevelBlock();
        }
    }

    public void GenerateInitialBlocks()
    {
        for (int i = 0; i < 2; i++)
        {
            AddLevelBlock();
        }
    }
}
