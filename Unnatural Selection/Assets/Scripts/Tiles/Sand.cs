using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : TileBehavior {
    private int movement = 0;

    void Awake() {
        base.Awake();
        movementCost = movement;
        tileType = "sand";
    }

    // Update is called once per frame
    void Update() {

    }
}
