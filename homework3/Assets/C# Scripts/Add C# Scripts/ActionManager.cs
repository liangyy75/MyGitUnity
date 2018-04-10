using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour, ActionCallBack {
    private Dictionary<int, ObjAction> actions = new Dictionary<int, ObjAction>();
    private List<ObjAction> waitingAdd = new List<ObjAction>();
    private List<int> waitingDelete = new List<int>();

    public void actionDone(ObjAction source)
    {
        
    }
    
	// Update is called once per frame
	void Update () {
        foreach (ObjAction action in waitingAdd)
            actions.Add(action.GetInstanceID(), action);
        waitingAdd.Clear();

        foreach (KeyValuePair<int, ObjAction> action in actions)
        {
            ObjAction objAction = action.Value;
            if (objAction.destroy)
                waitingDelete.Add(objAction.GetInstanceID());
            else if(objAction.enable)
                objAction.Update();
        }

        foreach (int key in waitingDelete)
        {
            ObjAction action = actions[key];
            actions.Remove(key);
            DestroyObject(action);
        }
        waitingDelete.Clear();
	}

    public void addAction(GameObject gameObject, ObjAction action, ActionCallBack actionCallBack)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.actionCallBack = actionCallBack;
        waitingAdd.Add(action);
        action.Start();
    }
}
