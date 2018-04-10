using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    float move_speed = 20;
    
    int moving_status;
    Vector3 dest;
    Vector3 middle;

    // 7
    // 10

    void Update()
    {
        if (moving_status == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, middle, move_speed * Time.deltaTime);
            if (transform.position == middle)
                moving_status = 2;
        }
        else if (moving_status == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, move_speed * Time.deltaTime);
            if (transform.position == dest)
                moving_status = 0;
        }
    }
    public void setDestination(Vector3 _dest)
    {
        dest = _dest;
        middle = _dest;
        if (_dest.y == transform.position.y)
            moving_status = 2;
        else if (_dest.y < transform.position.y)
            middle.y = transform.position.y;
        else
            middle.x = transform.position.x;
        moving_status = 1;
    }

    public void reset()
    {
        moving_status = 0;
    }
}