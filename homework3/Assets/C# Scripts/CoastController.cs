using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastController
{
    readonly GameObject coast;
    readonly Vector3[] positions;
    readonly int type;
    
    MyCharacterController[] passengerPlaner;

    public CoastController(string name)
    {
        passengerPlaner = new MyCharacterController[6];

        if (name == "start")
        {
            coast = Object.Instantiate(Resources.Load("Perfabs/Coast", typeof(GameObject)), new Vector3(9, 1, 0), Quaternion.identity, null) as GameObject;
            positions = new Vector3[] {new Vector3(6.5F,2.25F,0), new Vector3(7.5F,2.25F,0), new Vector3(8.5F,2.25F,0),
                new Vector3(9.5F,2.25F,0), new Vector3(10.5F,2.25F,0), new Vector3(11.5F,2.25F,0)};
            type = 1;
        }
        else
        {
            coast = Object.Instantiate(Resources.Load("Perfabs/Coast", typeof(GameObject)), new Vector3(-9, 1, 0), Quaternion.identity, null) as GameObject;
            positions = new Vector3[] {new Vector3(-6.5F,2.25F,0), new Vector3(-7.5F,2.25F,0), new Vector3(-8.5F,2.25F,0),
                new Vector3(-9.5F,2.25F,0), new Vector3(-10.5F,2.25F,0), new Vector3(-11.5F,2.25F,0)};
            type = -1;
        }
        coast.name = name;
    }

    public int getEmptyIndex()
    {
        for (int i = 0; i < passengerPlaner.Length; i++)
            if (passengerPlaner[i] == null)
                return i;
        return -1;
    }

    public Vector3 getEmptyPosition()
    {
        return positions[getEmptyIndex()];
    }

    public void getOnCoast(MyCharacterController characterCtrl)
    {
        passengerPlaner[getEmptyIndex()] = characterCtrl;
    }

    public MyCharacterController getOffCoast(string passenger_name)
    {
        for (int i = 0; i < passengerPlaner.Length; i++)
        {
            if (passengerPlaner[i] != null && passengerPlaner[i].getName() == passenger_name)
            {
                MyCharacterController charactorCtrl = passengerPlaner[i];
                passengerPlaner[i] = null;
                return charactorCtrl;
            }
        }
        return null;
    }

    public int getType()
    {
        return type;
    }

    public int[] getPassengersType()
    {
        int[] count = { 0, 0 };
        for (int i = 0; i < passengerPlaner.Length; i++)
        {
            if (passengerPlaner[i] == null)
                continue;
            if (passengerPlaner[i].getType() == 0)
                count[0]++;
            else
                count[1]++;
        }
        return count;
    }

    public void reset()
    {
        passengerPlaner = new MyCharacterController[6];
    }
}
