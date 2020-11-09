using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CapturePoint : TileBehavior
{
    //ownedBy = 1 when captured by player 1
    public int ownedBy = 0;
    //turn = 1 when one turn has been ended on the point w/o attacking, turn = 2 when two turns have been ended on the point w/o attacking
    public int turn = 0;
    public GameObject unitOnPoint;

    // Start is called before the first frame update

    void Awake() {
        base.Awake();
        tileType = "capturepoint";
    }

    // Update is called once per frame
    void Update()
    {
        if (myUnit == null && unitOnPoint != null) {
            if (ownedBy == 0) {
                unitOnPoint.GetComponent<Character>().attacked = false;
                unitOnPoint = null;
                turn = 0;
                Debug.Log("Cap stopped");
            }
        }
    }

    public void Capture() {
        if (!myUnit.GetComponent<Character>().attacked) {
            if (ownedBy == 0) {
                if (turn == 0) {
                    unitOnPoint = myUnit;
                    turn++;
                    Debug.Log("Capping");
                }
                else if (turn == 1) {
                    if (unitOnPoint.Equals(myUnit)) {
                        turn++;
                        ownedBy = unitOnPoint.GetComponent<Character>().player;
                        Debug.Log("Capped");
                    }
                }
            }
            else if (myUnit.GetComponent<Character>().player != ownedBy) {
                ownedBy = 0;
                Debug.Log("Cap neutralized");
            }
        }
    }
}
