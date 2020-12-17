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
        tileDesc = "Cost: 2\n\nAlthough troublesome to get into, taking cover here negate the first attack for enemies each turn.";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
