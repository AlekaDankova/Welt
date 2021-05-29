using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnSave : MonoBehaviour
{
    public Button btn_save;
    private string nextScene = "Start";

    void Start()
    {
        btn_save = GetComponent<Button>();
        btn_save.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        bool save = DBManager.instance.postData();
        if(!save) SceneManager.LoadScene(nextScene);
        else Debug.Log("Save Error");
    }
}