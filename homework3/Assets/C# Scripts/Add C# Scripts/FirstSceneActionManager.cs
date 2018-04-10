using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneActionManager : ActionManager {
    public void moveBoat(BoatController boat)
    {
        MoveToAction action = MoveToAction.getAction(boat.getDestination(), boat.speed);
        this.addAction(boat.getGameobj(), action, this);
    }

    public void moveCharacter(MyCharacterController character, Vector3 dest)
    {
        Vector3 now = character.getPosition();
        Vector3 mid = now;
        if (dest.y > now.y)
            mid.y = dest.y;
        else
            mid.x = dest.x;
        ObjAction action1 = MoveToAction.getAction(mid, character.speed);
        ObjAction action2 = MoveToAction.getAction(dest, character.speed);
        ObjAction sequenceAction = SequenceAction.getAction(new List<ObjAction> { action1, action2 }, 1, 0);
        this.addAction(character.GetGameObject(), sequenceAction, this);
    }
}
