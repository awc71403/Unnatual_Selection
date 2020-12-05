using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubble : TileBehavior
{
    private int movement = 2;

    void Awake()
    {
        base.Awake();
        movementCost = movement;
        tileType = "rubble";
        tileName = "Rubble";
        tileDesc = "Cost: 2\n\nThe remains of buildings that have been completely totaled.";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
