using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController
{
    readonly GameObject character;
    readonly Moveable moveableScript;
    readonly ClickGUI clickGUI;
    readonly int characterType;
    
    bool _isOnBoat;
    bool clickFlag = true;
    CoastController coastController;
    
    public MyCharacterController(string name, int type)
    {
        character = Object.Instantiate(Resources.Load(type == 0 ? "Perfabs/Priest" : "Perfabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
        character.name = name;
        characterType = type;
        moveableScript = character.AddComponent(typeof(Moveable)) as Moveable;
        clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
        clickGUI.setController(this);
    }

    public void setPosition(Vector3 pos)
    {
        character.transform.position = pos;
    }

    public void moveToPosition(Vector3 destination)
    {
        if(clickFlag)
            moveableScript.setDestination(destination);
    }

    public int getType()
    {
        return characterType;
    }

    public string getName()
    {
        return character.name;
    }

    public void getOnBoat(BoatController boatCtrl)
    {
        coastController = null;
        character.transform.parent = boatCtrl.getGameobj().transform;
        _isOnBoat = true;
    }

    public void getOnCoast(CoastController coastCtrl)
    {
        coastController = coastCtrl;
        character.transform.parent = null;
        _isOnBoat = false;
    }

    public bool isOnBoat()
    {
        return _isOnBoat;
    }

    public void setClickFlag(bool flag)
    {
        clickFlag = flag;
    }

    public CoastController getCoastController()
    {
        return coastController;
    }

    public void reset()
    {
        clickFlag = true;
        moveableScript.reset();
        coastController = (Director.getInstance().currentSceneController as FirstController).startCoast;
        getOnCoast(coastController);
        setPosition(coastController.getEmptyPosition());
        coastController.getOnCoast(this);
    }
}