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

    // Start is called before the first frame update

    void Awake() {
        base.Awake();
        tileType = "capturepoint";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPointerClick(PointerEventData data) {
        base.OnPointerClick(data);
        /* for reference
        if (playerside == GameManager.currentPlayer && data.button == PointerEventData.InputButton.Left) {
            // Make sure you can't click on tiles while buying a unit
            GameManager.GetSingleton().SummonPanel.gameObject.SetActive(true);
        } */
    }
}
