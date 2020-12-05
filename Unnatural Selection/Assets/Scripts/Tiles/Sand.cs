using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : TileBehavior {
    private int movement = 1;

    void Awake() {
        base.Awake();
        movementCost = movement;
        tileType = "sand";
        tileName = "Dark Sand";
        tileDesc = "Cost: X\n\nKnown to be very hard to move in for bigger units. Costs 1, 2, 3 to move into for units that cost 1-3, 4-6, 7+ respectively.";
    }

    // Update is called once per frame
    void Update() {

    }
}
