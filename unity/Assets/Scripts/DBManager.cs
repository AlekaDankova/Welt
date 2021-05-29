using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Net;

public class Message
{
    public bool error;
    public string message;
}

public class DBManager : MonoBehaviour
{
    public static DBManager instance = null;
    private string url = "https://sae-projekt.dankou.de/"; 
    //private string url = "http://localhost:4041/"; 

    void Awake(){
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public string get(){
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader sr = new StreamReader(response.GetResponseStream());
        Message m = JsonUtility.FromJson<Message>(sr.ReadToEnd());
        return m.message;
    }

    public string registrByName(string name, string pass){
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "registrByName/" + name + "/" + pass);
        request.Method = "GET";
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader sr = new StreamReader(response.GetResponseStream());
        return sr.ReadToEnd();
    }

    public string getDataByID(int id){
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "getDataByID/" + id);
        request.Method = "GET";
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader sr = new StreamReader(response.GetResponseStream());
        return sr.ReadToEnd();
    }

    public bool postData(){        
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "postData");
        request.Method = "POST";
        string jsonString = JsonUtility.ToJson(Player.instance.playerData);
        byte[] postBytes = Encoding.ASCII.GetBytes(jsonString);
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = postBytes.Length;
        Stream requestStream = request.GetRequestStream();
        requestStream.Write(postBytes, 0, postBytes.Length);
        requestStream.Close();
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader sr = new StreamReader(response.GetResponseStream());
        Message m = JsonUtility.FromJson<Message>(sr.ReadToEnd());
        return m.error;
    }
}