using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : MonoBehaviour
{
    public Enemy enemy;
    public GameController game;
    public bool turnFlag;
    public bool blocking;
    public HealthBar healthBar;
    public Animator anim;
    private bool forwardanim;
    public IEnumerator cor;
    private int sign;

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
        game = GameObject.Find("GameAssets").GetComponent<GameController>();
        turnFlag = false;
        blocking = false;
        sign = -1;
        healthBar.setMaxHealth(enemy.maxHealth);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(turnFlag==true)
            switch (curState)
            {
                case (TurnState.READY):
                    {
                        if(game.GetDistance()<=11f) cor = attack(enemy.baseDamage + UnityEngine.Random.Range(0, enemy.varDamage));
                        else cor = forward();
                        StartCoroutine(cor);
                    }
                    break;
                case (TurnState.BLOCKING):
                    {
                        cor = block2();
                        StartCoroutine(cor);
                    }
                    break;

                case (TurnState.WAITING):
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

    public void SetTurn()
    {
        Debug.Log("bye");
        turnFlag = true;
    }
    public void TakeDamage(int value)
    {
        if (blocking) enemy.curHealth -= Mathf.Max(value - enemy.block - enemy.defense, 0);
        else enemy.curHealth -= Mathf.Max(value - enemy.defense);
        if (enemy.curHealth < 0) enemy.curHealth = 0;
        healthBar.setHealth(enemy.curHealth);
    }

    private IEnumerator attack(int dmg)
    {
        turnFlag = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1);
        game.PlayerDamage(dmg);
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
        game.SetTurn();
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
        turnFlag = false;
        anim.SetTrigger("JumpForward");
        yield return new WaitForSeconds(1.5f);
        curState = TurnState.WAITING;
        anim.ResetTrigger("JumpForward");
        game.SetTurn();
    }

    private IEnumerator backward()
    {
        turnFlag = false;
        anim.SetTrigger("JumpBackward");
        yield return new WaitForSeconds(1.5f);
        curState = TurnState.WAITING;
        anim.ResetTrigger("JumpBackward");
        game.SetTurn();
    }

    void JumpFinish(float i)
    {
        if (i == 1)
        {
            forwardanim = false;
            Debug.Log("Haide ma");
        }
    }
    void JumpForwardStart()
    {
        forwardanim = true;
        Debug.Log("o data");
    }
    void OnAnimatorMove()
    {
        if (forwardanim)
        {
            Vector3 newPosition = transform.position;
            newPosition.x += sign * 5 * Time.deltaTime;
            transform.position = newPosition;
            JumpFinish(0);
        }
    }

    private void SetOtherTurn()
    {
        PlayerC player = GameObject.Find("Player(Clone)").GetComponent<PlayerC>();
        turnFlag = false;
        player.SetTurn();
    }
}
