using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : System.Object
{
    private static Director _instance;
    public SceneController currentSceneController { get; set; }

    public static Director getInstance()
    {
        return _instance ?? (_instance = new Director());
    }
}
