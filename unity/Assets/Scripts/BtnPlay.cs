using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerRegistr
{
    public int id;
    public string name;
    public string pass;
}

public class BtnPlay : MonoBehaviour
{
    public Button btn_play;
    public InputField input_name;
    public InputField input_pass;
    private string nextScene = "World";

    void Start()
    {
        btn_play = GetComponent<Button>();
        btn_play.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        string registr = DBManager.instance.registrByName(input_name.text, input_pass.text);
        PlayerRegistr player = JsonUtility.FromJson<PlayerRegistr>(registr);
        if(player.id != 0){
            PlayerPrefs.SetInt("playerId", player.id);
            PlayerPrefs.SetString("playerName", player.name);
            PlayerPrefs.SetString("playerName", input_name.text);
            SceneManager.LoadScene(nextScene);
        }
    }
}