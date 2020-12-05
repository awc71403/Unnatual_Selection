using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Image Credits;
    public Image Tutorial;
    public AudioClip press;
    public AudioSource audioSource;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }

    public void OpenCredits() {
        Credits.gameObject.SetActive(true);
    }

    public void OpenTutorial() {
        Tutorial.gameObject.SetActive(true);
    }

    public void ClosePanel() {
        Credits.gameObject.SetActive(false);
        Tutorial.gameObject.SetActive(false);
    }

    public void ButtonPress() {
        audioSource.clip = press;
        audioSource.Play();
    }
}
