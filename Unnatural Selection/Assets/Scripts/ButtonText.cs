using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    public AudioClip hover;
    public AudioSource audioSource;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        audioSource.clip = hover;
        audioSource.Play();
        gameObject.GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255);
    }

    public void OnPointerExit(PointerEventData eventData) {
        gameObject.GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0);
    }
}
