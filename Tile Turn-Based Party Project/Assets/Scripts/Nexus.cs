using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Nexus : TileBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Successful Initialization");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPointerClick(PointerEventData data)
    {
        GameManager.GetSingleton().GetSummonUI().gameObject.SetActive(true);
        Debug.Log("Opened Summon UI");
    }
}
