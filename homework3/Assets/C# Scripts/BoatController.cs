using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController
{
    readonly GameObject boat;
    readonly Vector3 startPosition = new Vector3(5, 1, 0);
    readonly Vector3 endPosition = new Vector3(-5, 1, 0);
    readonly Vector3[] startPositions;
    readonly Vector3[] endPositions;
    public readonly float speed = 20;
    
    int status;
    MyCharacterController[] passenger = new MyCharacterController[2];

    public BoatController()
    {
        status = 1;
        startPositions = new Vector3[] { new Vector3(4.5F, 1.5F, 0), new Vector3(5.5F, 1.5F, 0) };
        endPositions = new Vector3[] { new Vector3(-5.5F, 1.5F, 0), new Vector3(-4.5F, 1.5F, 0) };
        boat = Object.Instantiate(Resources.Load("Perfabs/Boat", typeof(GameObject)), startPosition, Quaternion.identity, null) as GameObject;
        boat.name = "boat";
        boat.AddComponent(typeof(ClickGUI));
    }

    public Vector3 getDestination()
    {
        return status == -1 ? startPosition : endPosition;
    }

    public int getEmptyIndex()
    {
        return passenger[0] == null ? 0 : (passenger[1] == null ? 1 : -1);
    }

    public bool isEmpty()
    {
        for (int i = 0; i < passenger.Length; i++)
            if (passenger[i] != null)
                return false;
        return true;
    }

    public GameObject getGameobj()
    {
        return boat;
    }

    public int getStatus()
    {
        return status;
    }

    public Vector3 getEmptyPosition()
    {
        int emptyIndex = getEmptyIndex();
        return status == -1 ? endPositions[emptyIndex] : startPositions[emptyIndex];
    }

    public void move()
    {
        status = 0 - status;
    }

    public void GetOnBoat(MyCharacterController characterCtrl)
    {
        passenger[getEmptyIndex()] = characterCtrl;
    }

    public MyCharacterController GetOffBoat(string passenger_name)
    {
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] != null && passenger[i].getName() == passenger_name)
            {
                MyCharacterController characendrCtrl = passenger[i];
                passenger[i] = null;
                return characendrCtrl;
            }
        }
        return null;
    }

    public int[] getPassengersType()
    {
        int[] count = { 0, 0 };
        for (int i = 0; i < passenger.Length; i++)
        {
            if (passenger[i] == null)
                continue;
            if (passenger[i].getType() == 0)
                count[0]++;
            else
                count[1]++;
        }
        return count;
    }

    public void reset()
    {
        if (status == -1)
            move();
        boat.transform.position = startPosition;
        passenger = new MyCharacterController[2];
    }
}