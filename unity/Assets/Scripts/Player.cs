using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public int id;
    public int id_name;
    public int hp = 10;
    public int attack = 1;
    public int lvl = 1;
    public int exp = 0;
    public int tree = 0;
    public string name;
}

public class Player : MovingObject
{
    public static Player instance = null;  
    public PlayerData playerData;
    private int pointsPerFood = 1;
    private int pointsPerSoda = 1;
    public Text txtName;
    public Text txtLvl;
    public Text txtExp;
    public Text txtHp;
    public Text txtAttack;
    public Text txtTree;
    public Text txtTime;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    private Animator animator;
    private Vector2 touchOrigin = -Vector2.one;
    private bool soda = false;
    private int sodaTime = 20;
    public Button btn_top;
    public Button btn_left;
    public Button btn_right;
    public Button btn_bottom;
        int horizontal = 0; 
        int vertical = 0;  

    void Awake() 
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);    
        DontDestroyOnLoad(gameObject);
        playerData = new PlayerData();
        playerData.id = PlayerPrefs.GetInt("playerId");
        string pd = DBManager.instance.getDataByID(playerData.id);
        playerData = JsonUtility.FromJson<PlayerData>(pd);
        playerData.name = PlayerPrefs.GetString("playerName");

        btn_top = GameObject.Find("Btn Top").GetComponent<Button>();
        btn_top.onClick.AddListener(OnClickTop);
        btn_left = GameObject.Find("Btn Left").GetComponent<Button>();
        btn_left.onClick.AddListener(OnClickLeft);
        btn_right = GameObject.Find("Btn Right").GetComponent<Button>();
        btn_right.onClick.AddListener(OnClickRight);
        btn_bottom = GameObject.Find("Btn Bottom").GetComponent<Button>();
        btn_bottom.onClick.AddListener(OnClickBottom);
    }

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        txtName.text = "Name: " + playerData.name;
        txtLvl.text = "LVL: " + playerData.lvl;
        txtExp.text = "EXP: " + playerData.exp;
        txtHp.text = "HP: " + playerData.hp;
        txtAttack.text = "Attack: " + playerData.attack;
        txtTree.text = "Tree: " + playerData.tree;
        txtTime.text = "Day:" + GameManager.instance.day + "-" + GameManager.instance.time + "%";

        base.Start ();
    }

    void OnClickTop()
    {
        vertical = 1;
    }

    void OnClickLeft()
    {
        horizontal = -1;
    }

    void OnClickRight()
    {
        horizontal = 1;
    }

    void OnClickBottom()
    {
        vertical = -1;
    }

    private void Update ()
    {
        if(!GameManager.instance.playersTurn) return;
        if(horizontal != 0)
        {
            vertical = 0;
        }
        if(horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall> (horizontal, vertical);
            AttemptMove<Enemy> (horizontal, vertical);
        }
        horizontal = 0;
        vertical = 0;
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        txtName.text = "Name: " + playerData.name;
        txtLvl.text = "LVL: " + playerData.lvl;
        txtExp.text = "EXP: " + playerData.exp;
        txtHp.text = "HP: " + playerData.hp;
        txtTree.text = "Tree: " + playerData.tree;
        if(soda) {
            sodaTime--;
            txtAttack.text = "Attack+1: " + playerData.attack;
            if(sodaTime == 0){
                sodaTime = 20;
                soda = false;
                playerData.attack = playerData.attack - pointsPerSoda;
                txtAttack.text = "Attack: " + playerData.attack;
            }
            }
        else txtAttack.text = "Attack: " + playerData.attack;
        GameManager.instance.time = GameManager.instance.time - 2;
        if(GameManager.instance.time == 0){
            GameManager.instance.time = 100;
            if(GameManager.instance.day_night == true) GameManager.instance.day_night = false;
            else {
                GameManager.instance.day++;
                GameManager.instance.day_night = true;
            }
        }
        if(GameManager.instance.day_night) txtTime.text = "Day:" + GameManager.instance.day + "-" + GameManager.instance.time + "%";
        else txtTime.text = "Night:" + GameManager.instance.day + "-" + GameManager.instance.time + "%";
        base.AttemptMove <T> (xDir, yDir);
        RaycastHit2D hit;
        if (Move (xDir, yDir, out hit)) 
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }
        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove <T> (T component)
    {
        if(component.name == "Wall1(Clone)" || component.name == "Wall2(Clone)" || component.name == "Wall3(Clone)" || component.name == "Wall4(Clone)" || component.name == "Wall5(Clone)" || component.name == "Wall6(Clone)" || component.name == "Wall7(Clone)" || component.name == "Wall8(Clone)")
        {
            Wall hitWall = component as Wall;
            hitWall.DamageWall (playerData.attack);
        }
        else if(component.name == "Enemy1(Clone)" || component.name == "Enemy2(Clone)")
        {
            Enemy hitEnemy = component as Enemy;
            bool die = hitEnemy.DamageEnemy (playerData.attack);
            if(die)
            {
                playerData.exp++;
                if(playerData.exp == 10)
                {
                    playerData.exp = 0;
                    playerData.lvl++;
                    playerData.attack++;
                    txtLvl.text = "+1 LVL";
                } else 
                {
                    txtExp.text = "+1 EXP";
                }
            }
        }
        animator.SetTrigger ("playerChop");
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "Food")
        {
            playerData.hp += pointsPerFood;
            txtHp.text = "+" + pointsPerFood + " HP";
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive (false);
        } else if(other.tag == "Soda")
        {
            if(soda == true) 
            {
                playerData.attack = playerData.attack;
                sodaTime = 20;
            }
            else playerData.attack = playerData.attack + pointsPerSoda;
            soda = true;
            txtAttack.text = "Attack+1: " + playerData.attack;
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive (false);
        }
    }
        
    public void LoseHP (int loss)
    {
        animator.SetTrigger ("playerHit");
        playerData.hp -= loss;
        txtHp.text = "-" + loss + " HP";
        if(playerData.hp < 1) GameManager.instance.GameOver();
    }
}
