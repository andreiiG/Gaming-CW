using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : MonoBehaviour
{
    public Enemy enemy;
    public PlayerC opponent;
    public bool turnFlag;
    public bool blocking;
    public float z;
    public HealthBar healthBar;
    public Animator anim;
    public IEnumerator cor;

    public enum TurnState
    {
        READY,
        WAITING,
        BLOCKING,
        MOVING,
        DEAD
    }

    public TurnState curState;

    // Start is called before the first frame update
    void Start()
    {
        curState = TurnState.WAITING;
        opponent = GameObject.Find("Player").GetComponent<PlayerC>();
        turnFlag = false;
        blocking = false;
        z = GetComponent<Transform>().position.z;
        healthBar.setMaxHealth(enemy.maxHealth);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case (TurnState.READY):
                cor = forward();
                StartCoroutine(cor);
                break;

            case (TurnState.BLOCKING):
                if (turnFlag == true)
                {
                    cor = block2();
                    StartCoroutine(cor);
                }
                break;

            case (TurnState.WAITING):
                if (turnFlag == true)
                {
                    if (enemy.curHealth <= 0) curState = TurnState.DEAD;
                    else
                    {
                        curState = TurnState.READY;
                    }
                }
                break;

            case (TurnState.DEAD):
                anim.SetTrigger("Death");
                Debug.Log("Defeat");
                break;
        }
    }

    public void setTurn()
    {
        turnFlag = true;
    }
    public void takeDamage(int value)
    {
        if (blocking) enemy.curHealth -= Mathf.Max(value - enemy.block - enemy.defense, 0);
        else enemy.curHealth -= Mathf.Max(value - enemy.defense);
        if (enemy.curHealth < 0) enemy.curHealth = 0;
        healthBar.setHealth(enemy.curHealth);
    }

    private IEnumerator attack(int dmg)
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1);
        opponent.takeDamage(dmg);
        turnFlag = false;
        curState = TurnState.WAITING;
        anim.ResetTrigger("Attack");
        opponent.setTurn();
    }

    private IEnumerator block()
    {
        anim.SetTrigger("Block");
        yield return new WaitForSeconds(1f);
        turnFlag = false;
        curState = TurnState.BLOCKING;
        blocking = true;
        opponent.setTurn();
    }

    private IEnumerator block2()
    {
        anim.SetTrigger("Block2");
        yield return new WaitForSeconds(0.5f);
        curState = TurnState.WAITING;
        blocking = false;
        anim.ResetTrigger("Block2");
        anim.ResetTrigger("Block");
    }

    private IEnumerator forward()
    {
        anim.SetTrigger("JumpForward");
        yield return new WaitForSeconds(1.5f);
        curState = TurnState.WAITING;
        anim.ResetTrigger("JumpForward");
        turnFlag = false;
        opponent.setTurn();
    }

    private IEnumerator backward()
    {
        anim.SetTrigger("JumpBackward");
        yield return new WaitForSeconds(1.5f);
        curState = TurnState.WAITING;
        anim.ResetTrigger("JumpBackward");
        turnFlag = false;
        opponent.setTurn();
    }
}
