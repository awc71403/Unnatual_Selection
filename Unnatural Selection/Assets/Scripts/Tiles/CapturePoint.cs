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
    public bool unitBool;
    [SerializeField]
    private AudioClip[] captureSounds;
    AudioSource audioSource;

    // Start is called before the first frame update

    void Awake() {
        base.Awake();
        tileType = "capturepoint";
        tileName = "Psionic Satellite";
        tileDesc = "Cost: 1\n\nThere are only a few in this world. Factions gather and fight over control of the satellite to change the climate of the area.";
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myUnit == null && unitBool) {
            if (ownedBy == 0) {
                Debug.Log("Update check");
                if (unitOnPoint) {
                    unitOnPoint.GetComponent<Character>().hasAttacked = false;
                    unitOnPoint = null;
                }
                unitBool = false;
                turn = 0;
                audioSource.clip = captureSounds[2];
                audioSource.Play();
            }
        }
    }

    public void Capture() {
        Debug.Log("Capture check");
        if (!myUnit.GetComponent<Character>().hasAttacked) {
            if (ownedBy == 0) {
                if (turn == 0) {
                    unitOnPoint = myUnit;
                    unitBool = true;
                    turn++;
                    audioSource.clip = captureSounds[1];
                    audioSource.Play();
                }
                else if (turn == 1) {
                    if (unitBool) {
                        turn++;
                        ownedBy = unitOnPoint.GetComponent<Character>().player;
                        if (unitOnPoint.GetComponent<Character>().faction == "Insect") {
                            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/insectcap");
                        }
                        else if (unitOnPoint.GetComponent<Character>().faction == "Mech") {
                            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/mechcap");
                        }
                        else if (unitOnPoint.GetComponent<Character>().faction == "Rock") {
                            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/rockcap");
                        }
                        else if (unitOnPoint.GetComponent<Character>().faction == "Shadow") {
                            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/shadowcap");
                        }
                        audioSource.clip = captureSounds[0];
                        audioSource.Play();
                    }
                }
            }
            else if (myUnit.GetComponent<Character>().player != ownedBy) {
                ownedBy = 0;
                turn = 0;
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/neutralcap");
                audioSource.clip = captureSounds[2];
                audioSource.Play();
            }
        }
    }
}
