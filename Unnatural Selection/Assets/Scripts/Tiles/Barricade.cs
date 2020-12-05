using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : TileBehavior
{
    private int movement = 2;

    void Awake()
    {
        base.Awake();
        movementCost = movement;
        tileType = "barricade";
        tileName = "Barricade";
        tileDesc = "Cost: 2\n\nAlthough troublesome to get into, taking cover here will reduced the damage by half the first time you're hit.";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
