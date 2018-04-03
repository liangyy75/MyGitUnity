using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UserAction
{
    void moveBoat();
    void characterIsClicked(MyCharacterController characterCtrl);
    void restart();
}
