using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private int level;
    private int flag1;
    public Text text;


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
        level = game.GetLevel();
        flag1 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(turnFlag==true)
            if (level == 1)
            {
                if (flag1 == 0)
                {
                    cor = attack(enemy.baseDamage + UnityEngine.Random.Range(0, enemy.varDamage));
                    StartCoroutine(cor);
                    flag1 = 1;
                }
                else
                {
                    cor = attack(enemy.baseDamage + UnityEngine.Random.Range(0, enemy.varDamage));
                    StartCoroutine(cor);
                    cor = Vic();
                    StartCoroutine(cor);
                }
            }
            else
                switch (curState)
            {
                case (TurnState.READY):
                    {
                        anim.ResetTrigger("Block");
                        anim.ResetTrigger("Block2");
                        if (game.GetDistance() <= 11f)
                            cor = block();// cor = attack(enemy.baseDamage + UnityEngine.Random.Range(0, enemy.varDamage));
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
                            anim.SetTrigger("Block2");
                            curState = TurnState.READY;
                        }
                    }
                    break;

                case (TurnState.DEAD):
                    anim.SetTrigger("Dead");
                    SoundManager.PlaySound(SoundManager.Sound.Dead);
                    turnFlag = false;
                        text.text = "YOU WON THE DEMO";
                    break;
            }
        }

    public void SetTurn()
    {
        turnFlag = true;
    }
    public void TakeDamage(int value)
    {
        if (blocking)
        {
            enemy.curHealth -= Mathf.Max(value - enemy.block - enemy.defense, 0);
            SoundManager.PlaySound(SoundManager.Sound.Block);
        }
        else {
            SoundManager.PlaySound(SoundManager.Sound.EnemyHit);
            enemy.curHealth -= Mathf.Max(value - enemy.defense); 
        }
        if (enemy.curHealth < 0) enemy.curHealth = 0;
        healthBar.setHealth(enemy.curHealth);
    }

    private IEnumerator attack(int dmg)
    {
        turnFlag = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(2f);
        game.PlayerDamage(dmg);
        curState = TurnState.WAITING;
        anim.ResetTrigger("Attack");
        game.SetTurn();
    }

    private IEnumerator block()
    {
        turnFlag = false;
        anim.SetTrigger("Block");
        yield return new WaitForSeconds(0.5f);
        curState = TurnState.BLOCKING;
        blocking = true;
        game.SetTurn();
    }

    private IEnumerator block2()
    {

        yield return new WaitForSeconds(0.5f);
        curState = TurnState.WAITING;
        blocking = false;
    }

    private IEnumerator forward()
    {
        turnFlag = false;
        SoundManager.PlaySound(SoundManager.Sound.PlayerJump);
        anim.SetTrigger("JumpForward");
        yield return new WaitForSeconds(2f);
        anim.ResetTrigger("JumpForward");
        curState = TurnState.WAITING;
        game.SetTurn();
    }

    private IEnumerator backward()
    {
        turnFlag = false;
        SoundManager.PlaySound(SoundManager.Sound.PlayerJump);
        anim.SetTrigger("JumpBackward");
        yield return new WaitForSeconds(2f);
        anim.ResetTrigger("JumpBackward");
        curState = TurnState.WAITING;
        game.SetTurn();
    }

    void JumpFinish(float i)
    {
        if (i == 1)
        {
            forwardanim = false;
        }
    }
    void JumpForwardStart(float i)
    {
            forwardanim = true;

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
    private IEnumerator Vic()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("You win !");
        game.Victory();
    }
}
