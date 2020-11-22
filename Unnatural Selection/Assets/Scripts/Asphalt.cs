using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asphalt : TileBehavior
{
    private int movement = 1;

    void Awake()
    {
        base.Awake();
        movementCost = movement;
        tileType = "asphalt";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
