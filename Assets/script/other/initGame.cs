using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameResponse<T>
{
    public int err { get; set; }

    public T data { get; set; }

    public string Message { get; set; }
}



public class LoginResponse
{
    public bool ok { get; set; }
    public string session { get; set; }

}

public class initGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var userName = Find<InputField>("Canvas/userName");
        var passWord = Find<InputField>("Canvas/passWord");
        userName.text = "2014005";
        passWord.text = "Dianchu666";
        //获取按钮游戏对象
        GameObject btnObj = GameObject.Find("Canvas/loginBtn");
        //获取按钮脚本组件
        Button btn = (Button)btnObj.GetComponent<Button>();
        //添加点击侦听
        btn.onClick.AddListener(onClick);
    }

    T Find<T>(string path)
    {
        //获取按钮游戏对象
        GameObject obj = GameObject.Find(path);
        //获取按钮脚本组件
        var comm = (T)obj.GetComponent<T>();
        Debug.Log(comm);
        return comm;
    }

    private bool start = false;


    void onClick()
    {
        var userName = Find<InputField>("Canvas/userName");
        var passWord = Find<InputField>("Canvas/passWord");
        StartCoroutine(Game.yieldAll(new Action<CallBack>[]{
            (CallBack action) =>
            {
                Debug.Log("方法1");
                StartCoroutine(  Game.Post<LoginResponse>("/account/login", new
                {
                    userName = userName.text,
                    passWord = passWord.text,
                },(LoginResponse res) => {
                    Debug.Log(string.Format("登录成功:{0}{1}",res.ok,res.session));
                    //跳转场景
                    //拉取配置
                    //进入场景
                    User.SessionID =res.session;
                    action.value["session"] = res.session;
                    action.next();
                    }));
            },
            (CallBack action) =>
            {
                Debug.Log("方法2");

                StartCoroutine( Game.Post("/config/enum", new{
                    sessionID = action.value["session"]
                },(List<Value> res) => {
                    User.configs=res;
                    Debug.Log(string.Format("配置{0}",res.Count));
                    action.next();
                 }));
            },
            (CallBack action) =>
            {
                StartCoroutine( Game.Post("/game/enter", new{
                    sessionID = action.value["session"]
                },(GameEnterRequest res) => {
                    User.userValueDic = res.value;
                    Debug.Log(res.value);
                    start = true;
                    action.next();
                 }));
            }
        }));

        Debug.Log(start);

        Debug.Log("结束");
    }

    private int curProgressValue = 0;

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            var progressBar = Find<Image>("Canvas/progressBar");
            var loadingText = Find<Text>("Canvas/loadingText");
            int progressValue = 100;
            if (curProgressValue < progressValue)
            {
                curProgressValue++;
            }

            loadingText.text = curProgressValue + "%";//实时更新进度百分比的文本显示  

            progressBar.fillAmount = curProgressValue / 100f;//实时更新滑动进度图片的fillAmount值  
            if (curProgressValue == 100)
            {
                loadingText.text = "OK";//文本显示完成OK  
                start = false;
                curProgressValue = 0;
                DOTween.To(() => Camera.main.fieldOfView, v => Camera.main.fieldOfView = v, 110, 2);
                Camera.main.transform.DOMove(new Vector3(-100, 0, -1), 2).OnComplete(() =>
                {
                    SceneManager.LoadScene("MapScene");
                });


            }
            else
            {
                Debug.Log("等待");
            }
        }
    }


}

public class GameEnterRequest
{
    public Dictionary<int, UserValue> value { get; set; }
}

public class Prop
{
    public int valueNo;
    public int count;
}
public class Value
{
    public string enumType { get; set; }

    public int valueNo { get; set; }

    public string name { get; set; }

    public List<ValueAttr> attr { get; set; }

    public T TryGet<T>(string attrName)
    {
        if (attr == null)
        {
            return default(T);
        }

        var entry = attr.Find(r => r.name == attrName);
        if (entry == null)
        {
            return default(T);
        }
        return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(entry.value));
    }
}

public class ValueAttr
{
    public string enumType { get; set; }

    public int valueNo { get; set; }

    public string key { get; set; }

    public string name { get; set; }

    public object value { get; set; }

    public string valueType { get; set; }
}

public class CallBack
{
    public Action next { get; set; }

    public Dictionary<string, object> value = new Dictionary<string, object>();
}