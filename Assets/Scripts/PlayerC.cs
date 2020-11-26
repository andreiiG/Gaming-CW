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
    //public GameObject currentPrefabObject;

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
                newPosition.x += sign*5 * Time.deltaTime;
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
                if (Input.GetKeyDown("a") && game.GetDistance() <= 10f)
                {
                    cor = attack(player.baseDamage + UnityEngine.Random.Range(0, player.varDamage));
                    StartCoroutine(cor);
                }
                if (Input.GetKeyDown("d"))
                {
                    cor = block();
                    StartCoroutine(cor);
                }
                if (Input.GetKeyDown("w") && game.GetDistance() > 10f)
                {
                    cor = forward();
                    StartCoroutine(cor);
                }
                if (Input.GetKeyDown("s") && game.GetPWall() > 10f)
                {
                    cor = backward();
                    StartCoroutine(cor);
                }
                /* if (Input.GetKeyDown("r")){
                     cor = fireball();
                     StartCoroutine(cor);
                 }*/
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
                SoundManager.PlaySound(SoundManager.Sound.Dead);
                Debug.Log("Defeat");
                break;
        }
    }
    public void SetTurn()
    {
        Debug.Log("hi");
        turnFlag = true;
    }
    public void TakeDamage(int value)
    {
        SoundManager.PlaySound(SoundManager.Sound.EnemyHit);
        if (blocking) player.curHealth -= Mathf.Max(value - player.block - player.defense, 0);
        else player.curHealth -= Mathf.Max(value - player.defense);
        if (player.curHealth < 0) player.curHealth = 0;
        healthBar.setHealth(player.curHealth);
    }
    private IEnumerator attack(int dmg)
    {
        anim.SetTrigger("Attack");
        SoundManager.PlaySound(SoundManager.Sound.PlayerAttack);
        yield return new WaitForSeconds(1);
        game.EnemyDamage(dmg);
        turnFlag = false;
        curState = TurnState.WAITING;
        anim.ResetTrigger("Attack");
        SetOtherTurn();
        Debug.Log("Attack11");
    }

    private IEnumerator block()
    {
        anim.SetTrigger("Block");
        yield return new WaitForSeconds(1f);
        turnFlag = false;
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
        anim.SetTrigger("JumpForward");
        SoundManager.PlaySound(SoundManager.Sound.PlayerJump);
        yield return new WaitForSeconds(1.5f);
        curState = TurnState.WAITING;
        anim.ResetTrigger("JumpForward");
        turnFlag = false;
        game.SetTurn();
    }

    private IEnumerator backward()
    {
        SoundManager.PlaySound(SoundManager.Sound.PlayerJump);
        anim.SetTrigger("JumpForward");
        yield return new WaitForSeconds(1.5f);
        curState = TurnState.WAITING;
        anim.ResetTrigger("JumpForward");
        turnFlag = false;
        game.SetTurn();
    }
    //test 
   /* private IEnumerator fireball()
    {
        yield return new WaitForSeconds(1.5f);
        Vector3 pos;
            float yRot = transform.rotation.eulerAngles.y;
            Vector3 forwardY = Quaternion.Euler(0.0f, yRot, 0.0f) * Vector3.forward;
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            Vector3 up = transform.up;
            Quaternion rotation = Quaternion.identity;
                // set the start point in front of the player a ways, rotated the same way as the player
                pos = transform.position + (forwardY * 5.0f);
                rotation = transform.rotation;
                pos.y = 0.0f;
        FireProjectileScript projectileScript = currentPrefabObject.GetComponentInChildren<FireProjectileScript>();
        if (projectileScript != null)
        {
            // make sure we don't collide with other fire layers
            projectileScript.ProjectileCollisionLayers &= (~UnityEngine.LayerMask.NameToLayer("FireLayer"));
        }
        currentPrefabObject.transform.position = pos;
        currentPrefabObject.transform.rotation = rotation;
    }*/

    private void SetOtherTurn()
    {
        EnemyC enemy = GameObject.Find("Enemy(Clone)").GetComponent<EnemyC>();
        turnFlag = false;
        enemy.SetTurn();
    }
}
