using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : System.Object
{
    private static Director _instance;
    public SceneController sceneController { set; get; }

    public static Director GetInstance()
    {
        return _instance ?? (_instance = new Director());
    }
}