using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceAction : ObjAction, ActionCallBack {
    public List<ObjAction> objActions;
    public int repeat = 1;
    public int index = 0;

    public static SequenceAction getAction(List<ObjAction> objActions, int repeat, int index)
    {
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
        action.objActions = objActions;
        action.repeat = repeat;
        action.index = index;
        return action;
    }

    public override void Update()
    {
        if (objActions.Count == 0)
            return;
        if (index < objActions.Count)
        {
            objActions[index].Update();
        }
    }

    public void actionDone(ObjAction source)
    {
        source.destroy = false;
        if(++index >= objActions.Count)
        {
            index = 0;
            if (repeat > 0)
                repeat--;
            if (repeat == 0)
            {
                destroy = true;
                actionCallBack.actionDone(this);
            }
        }
    }

    public override void Start()
    {
        foreach (ObjAction action in objActions)
        {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.actionCallBack = this;
            action.Start();
        }
    }

    private void OnDestroy()
    {
        foreach (ObjAction action in objActions)
            DestroyObject(action);
    }
}
