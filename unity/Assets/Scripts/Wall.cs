using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wall : MonoBehaviour
{
    public AudioClip chopSound1;           
    public AudioClip chopSound2;             
    private int hp = 3;
    public Text txtObject;

    void Start()
    {
        txtObject = GameObject.Find("Txt Object").GetComponent<Text>();
    }

    void OnMouseDown() 
    {
        txtObject.text = "Name: Busch\nHP: " + hp;
    }

    public void DamageWall (int loss)
    {
        SoundManager.instance.RandomizeSfx (chopSound1, chopSound2);
        hp -= loss;
        if(hp <= 0)
        {
            Player.instance.playerData.tree++;
            gameObject.SetActive (false);
        }
    }
}