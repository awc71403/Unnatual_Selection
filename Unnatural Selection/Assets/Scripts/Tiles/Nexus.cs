using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Nexus : TileBehavior
{
    // Start is called before the first frame update
    private int nexusmovement = 999;
    [SerializeField]
    private AudioClip[] damagedSounds;
    AudioSource audioSource;

    void Awake() {
        base.Awake();
        movementCost = nexusmovement;
        tileType = "nexus";
        tileName = "Portal";
        tileDesc = "Summoned all around in hopes to capture and control the Psionic Satellites. Taking 3 hits from the enemy will cause the portal to collapse.";
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPointerClick(PointerEventData data) {
        if (selectionState == null) {
            base.OnPointerClick(data);

            if (playerside == GameManager.currentPlayer && data.button == PointerEventData.InputButton.Left && !GameManager.menuOpened) {
                // Make sure you can't click on tiles while buying a unit
                GameManager.GetSingleton().OpenSummonPanel();
            }
        }
    }

    public void Damaged() {
        GameManager.GetSingleton().AddNexusObjectivePoints();
        audioSource.clip = damagedSounds[Random.Range(0, damagedSounds.Length)];
        audioSource.Play();
    }
}
