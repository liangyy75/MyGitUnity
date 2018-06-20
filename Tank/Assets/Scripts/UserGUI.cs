using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    SceneController sceneController;
    GUIStyle style1;
    GUIStyle style2;
    GUIStyle style3;

	void Start ()
    {
        sceneController = Director.GetInstance().sceneController;
        style1 = new GUIStyle();
        style1.normal.textColor = Color.red;
        style1.fontSize = 20;
        style2 = new GUIStyle(style1);
        style2.alignment = TextAnchor.MiddleCenter;
        style2.fontSize = 30;
        style3 = new GUIStyle("Button");
        style3.fontSize = 30;
        style3.normal.textColor = Color.red;
    }
	
	void OnGUI ()
    {
        if (GameEventManager.isContinue)
        {
            GUI.Label(new Rect(20, 10, 200, 30), "工厂血量：" + ((int)sceneController.GetFactoryHp()).ToString(), style1);
            GUI.Label(new Rect(20, 50, 200, 30), "玩家血量：" + ((int)sceneController.GetPlayerHp()).ToString(), style1);
            GUI.Label(new Rect(20, 90, 200, 30), "玩家得分：" + sceneController.GetScore().ToString(), style1);
        }
        else
        {
            float w = Screen.width;
            float h = Screen.height;
            GUI.Label(new Rect(w / 2 - 150, h / 3 - 25, 300, 50), sceneController.GetFactoryHp() < 0 ? "You Win!" : "You Lose!", style2);
            if(GUI.Button(new Rect(w / 2 - 100, h / 3 * 2 - 25, 200, 50), "Restart", style3))
                Singleton<GameEventManager>.Instance.GameBegin();
        }
	}
}