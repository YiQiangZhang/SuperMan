using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class MainScene : MonoBehaviour
{
    public Joystick joystick;
    float speed = 600;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void moveTo(Vector3 mousePosition)
    {
        GameObject obj = GameObject.Find("Canvas/Role");
        obj.transform.DOMove(mousePosition, 2).OnComplete(() =>
         {
             Debug.Log(string.Format("结束 {0},{1}", mousePosition.x, mousePosition.y));
         });
    }


    public void StopRole()
    {
        GameObject obj = GameObject.Find("Canvas/Role");
        obj.transform.DOKill();
    }

    // Update is called once per frame
    void Update()
    {
        /* if (Input.GetMouseButtonDown(0))
         {
             moveTo(Input.mousePosition);
         }*/

        //指定圆形范围，当玩家超出这个方范围时，背景方向移动,人物不移动
        GameObject role = GameObject.Find("Canvas/Role");

        GameObject bg = GameObject.Find("Canvas/bg");
        var bgRect = (bg.transform as RectTransform);
        Vector3 bgMove = new Vector3(0, 0, 0);
        Vector3 roleMove = new Vector3(0, 0, 0);
        var mStepX = joystick.sickPos.x * speed * Time.deltaTime;
        var mStepY = joystick.sickPos.y * speed * Time.deltaTime;
        var xLimit = (bgRect.rect.width - 1696) / 2;
        var yLimit = (bgRect.rect.height - 848) / 2;
        var xRoleLimit = (1696 - 100) / 2;
        var yRoleLimit = (848 - 100) / 2;
        if (-xLimit < bg.transform.localPosition.x - mStepX && bg.transform.localPosition.x - mStepX < xLimit)
        {
            if ((10 < Mathf.Abs(role.transform.localPosition.x + mStepX) && Mathf.Abs(role.transform.localPosition.x + mStepX) < xRoleLimit))
            {
                roleMove.x += mStepX;
            }
            else
            {
                bgMove.x -= mStepX;
              
            }
        }
        else
        {
            if (-xRoleLimit < role.transform.localPosition.x + mStepX && role.transform.localPosition.x + mStepX < xRoleLimit)
            {
                roleMove.x += mStepX;
            }

        }

        if (-yLimit < bg.transform.localPosition.y - mStepY && bg.transform.localPosition.y - mStepY < yLimit)
        {
            if (10 < Mathf.Abs(role.transform.localPosition.y + mStepY) && Mathf.Abs(role.transform.localPosition.y + mStepY) < yRoleLimit)
            {
                roleMove.y += mStepY;
            }
            else
            {
                bgMove.y -= mStepY;
            }
        }
        else
        {
            if (-yRoleLimit < role.transform.localPosition.y + mStepY && role.transform.localPosition.y + mStepY < yRoleLimit)
            {
                roleMove.y += mStepY;
            }
        }
      

        bg.transform.Translate(bgMove);
        role.transform.Translate(roleMove);

    }

    public static void LogPosition(string key, Vector3 position)
    {
        Log(key + ": x:{0},y:{1}", position.x, position.y);
    }
    public static void Log(string format, params object[] args)
    {
        Debug.Log(string.Format(format, args));
    }

}

