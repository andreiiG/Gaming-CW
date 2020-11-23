using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerC : MonoBehaviour
{
    public Player player;
    public EnemyC opponent;
    public bool turnFlag;
    public bool blocking;
    private bool forwardanim; 
    public float z;
    public HealthBar healthBar;
    public Animator anim;
    private IEnumerator cor;

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
        curState = TurnState.READY;
        opponent = GameObject.Find("Enemy").GetComponent<EnemyC>();
        turnFlag = true;
        blocking = false;
        z = GetComponent<Transform>().position.z;
        healthBar.setMaxHealth(player.maxHealth);
        anim = GetComponent<Animator>();
    }


    void JumpFinish(float i)
    {
        if (i == 1)
        {
            forwardanim = false;
        }
    }
    void JumpStart()
    {
        forwardanim = true;
    }
    void OnAnimatorMove()
    {
        if (forwardanim )
            {
                Vector3 newPosition = transform.position;
                newPosition.x += -5 * Time.deltaTime;
                transform.position = newPosition;
            JumpFinish(0);
            }
        }
    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case (TurnState.READY):
                if (Input.GetKeyDown("a")) //&&Mathf.Abs(z-opponent.z)<10
                {
                    cor = attack(player.baseDamage + UnityEngine.Random.Range(0, player.varDamage));
                    StartCoroutine(cor);
                }
                if (Input.GetKeyDown("d"))
                {
                    cor = block();
                    StartCoroutine(cor);
                }
                if (Input.GetKeyDown("w"))
                {
                    cor = forward();
                    StartCoroutine(cor);
                }
                if (Input.GetKeyDown("s"))
                {
                    cor = backward();
                    StartCoroutine(cor);
                }
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
                    if (player.curHealth <= 0) curState = TurnState.DEAD;
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
        if (blocking) player.curHealth -= Mathf.Max(value - player.block - player.defense, 0);
        else player.curHealth -= Mathf.Max(value - player.defense);
        if (player.curHealth < 0) player.curHealth = 0;
        healthBar.setHealth(player.curHealth);
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
