using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private UserAction userAction;
    public int status = 0;
    GUIStyle messageStyle, buttonStyle;
    private int currentCount = 0;
    
    void Start()
    {
        userAction = Director.getInstance().currentSceneController as UserAction;
        messageStyle = new GUIStyle();
        messageStyle.fontSize = 40;
        messageStyle.normal.textColor = Color.red;
        messageStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 30;
    }

    public void setCount(int count)
    {
        currentCount = count;
        if (count == 0)
            status = 1;
    }

    private void OnGUI()
    {
        if (status > 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 50), status == 1 ? "Gameover!" : "You win!", messageStyle);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2 - 80, 140, 70), "Restart", buttonStyle))
            {
                status = 0;
                userAction.restart();
            }
        }
        else if(currentCount > 0)
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 50), currentCount.ToString(), messageStyle);
    }
}
