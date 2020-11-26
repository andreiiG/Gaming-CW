using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerC : MonoBehaviour
{
    public Player player;
    public GameController game;
    public bool turnFlag;
    public bool blocking;
    private bool forwardanim; 
    public HealthBar healthBar;
    public Animator anim;
    private IEnumerator cor;
    private int sign;
    int level;
    private int flag1;

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
        game = GameObject.Find("GameAssets").GetComponent<GameController>();
        turnFlag = true;
        blocking = false;
        sign = 1;
        healthBar.setMaxHealth(player.maxHealth);
        anim = GetComponent<Animator>();
        level = game.GetLevel();
        flag1 = 0;
    }
    void JumpFinish(float i)
    {
        if (i == 1)
        {
            forwardanim = false;
        }
    }
    void JumpForwardStart()
    {
        forwardanim = true;
    }
    void OnAnimatorMove()
    {
        if (forwardanim)
            {
            Vector3 newPosition = transform.position;
                newPosition.x += sign*5 * Time.deltaTime;
                transform.position = newPosition;
            JumpFinish(0);
            }
        }
    // Update is called once per frame
    void Update()
    {
        if (level == 0)
        {
            if (flag1 == 0 && Input.GetKeyDown("w"))
            {
                cor = forward();
                StartCoroutine(cor);
                flag1 = 1;
            }
            if (flag1 == 1 && Input.GetKeyDown("s"))
            {
                cor = backward();
                StartCoroutine(cor);
                cor = Vic();
                StartCoroutine(cor);
            }
        }
        else if (level == 1)
        {
            if (flag1 == 0 && Input.GetKeyDown("a") && game.GetDistance() <= 11f)
            {
                cor = attack(player.baseDamage + UnityEngine.Random.Range(0, player.varDamage));
                StartCoroutine(cor);
                flag1 = 1;
            }
            if (flag1 == 1 && Input.GetKeyDown("d"))
            {
                cor = block();
                StartCoroutine(cor);
            }
        }
        else if (turnFlag==true) switch (curState)
        {
            case (TurnState.READY):
                if (Input.GetKeyDown("a") && game.GetDistance() <= 11f)
                {
                    cor = attack(player.baseDamage + UnityEngine.Random.Range(0, player.varDamage));
                    StartCoroutine(cor);
                }
                if (Input.GetKeyDown("d"))
                {
                    cor = block();
                    StartCoroutine(cor);
                }
                if (Input.GetKeyDown("w") && game.GetDistance() > 11f)
                {
                    cor = forward();
                    StartCoroutine(cor);
                }
                if (Input.GetKeyDown("s") && game.GetPWall() > 11f)
                {
                    cor = backward();
                    StartCoroutine(cor);
                }
                break;

            case (TurnState.BLOCKING):
                {
                   if(turnFlag== true)
                        {
                            anim.ResetTrigger("Block");
                            anim.ResetTrigger("Block2");
                            if (player.curHealth <= 0) curState = TurnState.DEAD;
                            else
                            {
                                curState = TurnState.READY;
                            }
                        }
                }
                break;

            case (TurnState.WAITING):
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
                SoundManager.PlaySound(SoundManager.Sound.Dead);
                Debug.Log("Defeat");
                break;
        }
    }
    public void SetTurn()
    {
        if (player.curHealth <= 0) curState = TurnState.DEAD;
        else
        {
            curState = TurnState.READY;
            turnFlag = true;
        }
    }
    public void TakeDamage(int value)
    {

        if (blocking)
        {
            SoundManager.PlaySound(SoundManager.Sound.Block);
            player.curHealth -= Mathf.Max(value - player.block - player.defense, 0);
        }
        else {
            SoundManager.PlaySound(SoundManager.Sound.EnemyHit);
            player.curHealth -= Mathf.Max(value - player.defense); 
        }
        if (player.curHealth < 0) player.curHealth = 0;
        healthBar.setHealth(player.curHealth);
    }
    private IEnumerator attack(int dmg)
    {
        turnFlag = false;
        anim.SetTrigger("Attack");
        SoundManager.PlaySound(SoundManager.Sound.PlayerAttack);
        yield return new WaitForSeconds(2f);
        game.EnemyDamage(dmg);
        curState = TurnState.WAITING;
        anim.ResetTrigger("Attack");
        game.SetTurn();
    }

    private IEnumerator block()
    {
        turnFlag = false;
        anim.SetTrigger("Block");
        yield return new WaitForSeconds(1f);
        curState = TurnState.BLOCKING;
        blocking = true;
        anim.SetTrigger("Block2");
        game.SetTurn();
    }

    private IEnumerator forward()
    {
        sign = 1;
        turnFlag = false;
        SoundManager.PlaySound(SoundManager.Sound.PlayerJump);
        anim.SetTrigger("JumpForward");
        yield return new WaitForSeconds(2f);
        curState = TurnState.WAITING;
        anim.ResetTrigger("JumpForward");
        game.SetTurn();
    }

    private IEnumerator backward()
    {
        sign = -1;
        turnFlag = false;
        SoundManager.PlaySound(SoundManager.Sound.PlayerJump);
        anim.SetTrigger("JumpForward");
        yield return new WaitForSeconds(2f);
        curState = TurnState.WAITING;
        anim.ResetTrigger("JumpForward");
        game.SetTurn();
    }
    private void SetOtherTurn()
    {
        EnemyC enemy = GameObject.Find("Enemy(Clone)").GetComponent<EnemyC>();
        turnFlag = false;
        enemy.SetTurn();
    }
    private IEnumerator Vic()
    {
        yield return new WaitForSeconds(3f);
        game.Victory();
    }
}
