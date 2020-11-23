using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : TileBehavior
{
    private int movement = 999;

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
