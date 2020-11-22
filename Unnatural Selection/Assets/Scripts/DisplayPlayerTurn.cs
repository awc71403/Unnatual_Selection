using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerTurn : MonoBehaviour
{
    private Text txt;
    private int currentPlayer = GameManager.currentPlayer;

    // Use this for initialization
    void Awake()
    {
        txt = gameObject.GetComponent<Text>();
        txt.text = "Current Player: " + currentPlayer;

    }

    // Update is called once per frame
    void Update()
    {
        txt.text = "Current Player: " + currentPlayer;
    }

}
