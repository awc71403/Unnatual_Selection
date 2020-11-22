using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2ScoreDisplay : MonoBehaviour
{
    private Slider score;
    private int currentScore = 60;

    // Use this for initialization
    void Awake()
    {
        score = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        score.value = currentScore;
    }
}
