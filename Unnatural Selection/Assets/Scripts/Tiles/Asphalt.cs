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
        tileName = "Asphalt";
        tileDesc = "Cost: 1\n\nSince the war began, there's been nothing but this in every city.";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
