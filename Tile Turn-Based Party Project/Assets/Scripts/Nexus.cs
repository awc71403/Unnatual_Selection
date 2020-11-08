using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Nexus : TileBehavior
{
    // Start is called before the first frame update
    private int nexusmovement = 999;

    void Awake() {
        base.Awake();
        movementCost = nexusmovement;
        tileType = "nexus";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPointerClick(PointerEventData data) {
        base.OnPointerClick(data);

        if (playerside == GameManager.currentPlayer && data.button == PointerEventData.InputButton.Left) {
            // Make sure you can't click on tiles while buying a unit
            GameManager.GetSingleton().SummonPanel.gameObject.SetActive(true);
        }
    }
}
