using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MovingObject
{
    private int playerDamage = 1;                          
    private Animator animator;                     
    private Transform target;                      
    private bool skipMove;            
    private int hp = 2;
    public Text txtObject;

    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    protected override void Start ()
    {
        txtObject = GameObject.Find("Txt Object").GetComponent<Text>();
        GameManager.instance.AddEnemyToList (this);
        animator = GetComponent<Animator> ();
        target = GameObject.FindGameObjectWithTag ("Player").transform;
        hp = hp * Player.instance.playerData.lvl;  
        playerDamage = playerDamage * Player.instance.playerData.lvl; 
        base.Start ();
    }

    void OnMouseDown() 
    {
        txtObject.text = "Name: Enemy\nHP: " + hp + "\nAttack: " + playerDamage;
    }

    public bool DamageEnemy (int loss)
    {
        hp -= loss;
        if(hp <= 0)
        {
            GameManager.instance.DeleteEnemyFromList (this);
            Destroy (gameObject); 
            return true;
        }
        return false;
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        if(GameManager.instance.day_night) return;
        if(skipMove)
        {
            skipMove = false;
            return;
        }
        base.AttemptMove <T> (xDir, yDir);
        skipMove = true;
    }

    public void MoveEnemy ()
    {
        int xDir = 0;
        int yDir = 0;
        if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon) yDir = target.position.y > transform.position.y ? 1 : -1;
        else xDir = target.position.x > transform.position.x ? 1 : -1;
        AttemptMove <Player> (xDir, yDir);
    }

    protected override void OnCantMove <T> (T component)
    {
        Player hitPlayer = component as Player;
        hitPlayer.LoseHP(playerDamage);
        animator.SetTrigger ("enemyAttack");
        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
    }
}