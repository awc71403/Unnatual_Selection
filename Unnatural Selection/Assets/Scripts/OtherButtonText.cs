using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OtherButtonText : MonoBehaviour, IPointerEnterHandler {

    public AudioClip hover;
    public AudioSource audioSource;


    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        audioSource.clip = hover;
        audioSource.Play();
    }
}
