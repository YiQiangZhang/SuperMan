using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;

public class User
{
    public static string SessionID = "";

    public static Dictionary<int, UserValue> userValueDic = new Dictionary<int, UserValue>();
    public static List<Value> configs = new List<Value>();
    public static List<Hero> heros = new List<Hero>();
    public static Value GetConfig(int valueNo)
    {
        return configs.Find(r => r.valueNo == valueNo);
    }

    public static Value GetConfigByName(string name)
    {
        return configs.Find(r => r.name == name);
    }

    public static bool ConfigHasyAttr(Value v,string attrName,string value)
    {
        return v.attr.Any(r => r.name == attrName && r.value.ToString() == value);
    }

    public static GameObject GetUser()
    {
        return GameObject.Find("Canvas/Role");
    }

    public static List<Value> GetConfigByEnumTypeAndAttr(string enumType,string attrName,string value)
    {
        return configs.Where(r=> r.enumType == enumType && ConfigHasyAttr(r, attrName, value)).ToList();
    }
    public static Dictionary<int, UserValue> GetTotalUserValue()
    {
        return userValueDic;
    }

    public static long GetCount(int valueNo)
    {
        var c = userValueDic[valueNo];

        return c == null ? 0 : c.count;
    }

    public static long GetCountByName(string name)
    {
        var c = configs.Find(r => r.name == name);
        return c == null ? 0 : GetCount(c.valueNo);
    }

    public static IEnumerator Post<T>(string route, object data, Action<T> action, Action<int> errAction = null)
    {
        string json = JsonConvert.SerializeObject(data);
        var temp = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
       // temp["sessionID"] = "2014005_5e5a066fbdb0351d59f82e5b";
        temp["sessionID"] = SessionID;
        yield return Game.Post<T>(route, temp, action, errAction);
    }

    public static T HttpSend<T>(string route, object data, int Timeout = 60000)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://118.190.101.131:3001" + route);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.UserAgent = null;
        request.Timeout = Timeout;
        string json = JsonConvert.SerializeObject(data);
        var temp = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        // temp["sessionID"] = "2014005_5e5b531cbdb0351d59f82ebf";//SessionID;
        temp["sessionID"] = SessionID;
        byte[] bData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(temp));

        request.ContentLength = bData.Length;
        request.GetRequestStream().Write(bData, 0, bData.Length);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream myResponseStream = response.GetResponseStream();
        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

        string retString = myStreamReader.ReadToEnd();
        myResponseStream.Close();
        myResponseStream.Close();
        Debug.Log(retString);
        var result = JsonConvert.DeserializeObject<GameResponse<T>>(retString);
        if (result.err != 0)
        {
            Debug.Log("弹窗：网络有误，" + result.err.ToString());
        }
        return result.data;
    }
}

