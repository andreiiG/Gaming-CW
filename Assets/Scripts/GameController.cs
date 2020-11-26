using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject enemy2Prefab;

    public PlayerC playerController;
    public EnemyC enemyController;

    public GameObject player;
    public GameObject enemy;

    public int turn;

    // Start is called before the first frame update
    void Start()
    {
        LevelReset(1);   
    }

    private void LevelReset(int level)
    {
        player = Instantiate(playerPrefab) as GameObject;
        player.transform.position = new Vector3(-25, 0, 0);
        playerController = player.GetComponent<PlayerC>();
        if (level > 0)
        {
            enemy = Instantiate(enemyPrefab) as GameObject;
            enemy.transform.position = new Vector3(25, 0, 0);
            enemyController = enemy.GetComponent<EnemyC>();
        }
        turn = 1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayerDamage(int value)
    {
        playerController.TakeDamage(value);
    }
    public void EnemyDamage(int value)
    {
        enemyController.TakeDamage(value);
    }

    public void SetTurn()
    {
        if (turn == 2||turn==0)
        {
            turn = 1;
            Debug.Log("Turn 1");
            playerController.SetTurn();
        }
        else
        {
            turn = 2;
            Debug.Log("Turn 2");
            enemyController.SetTurn();
        }
    }

    public float GetDistance()
    {
        return enemy.transform.position.x - player.transform.position.x;
    }
    public float GetPWall()
    {
        return player.transform.position.x - 50;
    }
    public float GetEWall()
    {
        return 50 - enemy.transform.position.x;
    }
}
