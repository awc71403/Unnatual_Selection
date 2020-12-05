using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : TileBehavior
{
    private int movement = 3;

    void Awake()
    {
        base.Awake();
        movementCost = movement;
        tileType = "building";
        tileName = "Building";
        tileDesc = "Cost: 3\n\nUsed to be tall beautiful skyscrapers. Now just an obstacle.";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
