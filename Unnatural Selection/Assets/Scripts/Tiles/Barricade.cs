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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
