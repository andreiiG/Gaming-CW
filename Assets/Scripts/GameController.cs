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
        turn = 0;
        LevelReset(1);
        playerController.SetTurn();
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
       // SetTurn();
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerController.turnFlag == false && enemyController.turnFlag == false) SetTurn();
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
        //if (turn == 2||turn==0)
        //{
        //    turn = 1;
        //    Debug.Log("Turn 1");
        //    playerController.SetTurn();
        //}
        //else
        //{
        //    turn = 2;
        //    Debug.Log("Turn 2");
        //    enemyController.SetTurn();
        //}
    }

    public float GetDistance()
    {
        return enemy.transform.position.z - player.transform.position.z;
    }
    public float GetPWall()
    {
        return player.transform.position.z - 50;
    }
    public float GetEWall()
    {
        return 50 - enemy.transform.position.z;
    }
}
