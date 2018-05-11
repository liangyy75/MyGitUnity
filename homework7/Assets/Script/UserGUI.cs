using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    private IUserAction userAction;
    GUIStyle labelStyle;
    GUIStyle buttonStyle;
    GUIStyle finalStyle;

	// Use this for initialization
	void Start () {
        userAction = Director.GetInstance().sceneController as IUserAction;

        labelStyle = new GUIStyle();
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.fontSize = 14;
        labelStyle.alignment = TextAnchor.UpperCenter;

        buttonStyle = new GUIStyle("Button");
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle.fontSize = 16;

        finalStyle = new GUIStyle(labelStyle);
        finalStyle.fontSize = 20;
        finalStyle.normal.textColor = Color.red;
    }
	
	void OnGUI ()
    {
        int w = Screen.width;
        int h = Screen.height;
        GUI.Label(new Rect(w / 2 - 100, 10, 200, 30), "躲避过一个怪物加一分，超过30即可获得胜利", labelStyle);
        if(!GameEventManager.isContinue && GUI.Button(new Rect(w / 2 - 40, 60, 80, 30), "Start", buttonStyle))
        {
            Singleton<GameEventManager>.Instance.GameBegin();
        }
        if (GameEventManager.isContinue)
        {
            GUI.Label(new Rect(w - 160, 10, 100, 30), "Score: " + userAction.GetScore().ToString(), labelStyle);
        }
        else if(userAction.IsGameOver())
        {
            GUI.Label(new Rect(w / 2 - 50, h / 2 - 15, 100, 30), userAction.GetScore() > 29 ? "You Win!" : "Fail!", finalStyle);
        }
    }
}
