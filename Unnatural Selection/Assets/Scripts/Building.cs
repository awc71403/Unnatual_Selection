using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : TileBehavior
{
    private int movement = 99;

    void Awake()
    {
        base.Awake();
        movementCost = movement;
        tileType = "building";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
