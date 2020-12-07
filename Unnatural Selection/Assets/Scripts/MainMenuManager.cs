using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Button left;
    public Button right;

    public TextMeshProUGUI tutorialText;
    int tutorialPage;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        tutorialPage = 0;
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
        tutorialPage = 0;
        OpenTutorialText(0);
    }

    public void ClosePanel() {
        Credits.gameObject.SetActive(false);
        Tutorial.gameObject.SetActive(false);
    }

    public void TutorialLeftArrow() {
        tutorialPage--;
        OpenTutorialText(tutorialPage);
    }

    public void TutorialRightArrow() {
        tutorialPage++;
        OpenTutorialText(tutorialPage);
    }

    public void OpenTutorialText(int i) {
        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);
        switch (i) {
            case 0:
                tutorialText.text = "UI:\nEnergy - Resources you have to summon units\nPoints - Reach 100 points or have the most points after 12 turns to win the game\nTimer - You have 90 seconds to make your move. You also have a reserve of 120 seconds.\n\nKilling a unit - X points where X is the summoning cost\nOwning the capture point - 10 points at the start of your turn\nAttacking the nexus - 20 points\n*You can also win by attacking the nexus 3 times";
                left.gameObject.SetActive(false);
                break;
            case 1:
                tutorialText.text = "Taking a turn:\nClick on a unit and click on an available space to move\nIf there is an enemy in your attack range, click on the enemy to attack\nIf your unit hasn't finished its actions, you can right click to undo\nTo view the info of a unit, right click on it\nTo see the threat radius of an enemy unit, click and hold\nTo summon a unit, click on your nexus, sleect a unit, and then select where it will be placed\nTo end turn, left click an empty tile for the menu";
                break;
            case 2:
                tutorialText.text = "Terrain:\nAsphalt - 1 movement\nRubble - 2 movement\nBuilding - 3 movement\nBarricade - 2 movement, negate the first attack.\nSand - X movement, where X is 1 / 2 / 3 for units that cost 1 - 3 / 4 - 6 / 7 + to summon respectively\n\nObjective Terrain:\nPsionic Satellite - 1 movement, the same unit moves and stays(does not attack) for two turns. If an enemy stays(does not attack) on your captured satellite, the satellite will be neutralized.";
                right.gameObject.SetActive(false);
                break;
        }
    }

    public void ButtonPress() {
        audioSource.clip = press;
        audioSource.Play();
    }
}
