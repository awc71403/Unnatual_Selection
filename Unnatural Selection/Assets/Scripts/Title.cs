using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    AudioSource audioSource;
    bool played;

    private void Awake() {
        played = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (Input.anyKey && !played) {
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.Tab)) {
                return;
            }
            played = true;
            StartCoroutine("StartGame");
        }
    }

    IEnumerator StartGame() {
        audioSource.Play();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
