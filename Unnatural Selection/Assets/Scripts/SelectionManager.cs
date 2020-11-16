using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    public Image firstUnit;
    [SerializeField]
    public Image secondUnit;
    [SerializeField]
    public Image thirdUnit;
    [SerializeField]
    public Image player1Image;
    [SerializeField]
    public Image player2Image;
    [SerializeField]
    public Text factionText;
    [SerializeField]
    public Button beginButton;

    public UnitCollection unitCollection;

    public static GameObject[] currentFaction;

    public int currentFactionInt;

    public int player1Faction;
    public int player2Faction;
    #endregion

    #region Initialization
    public void Awake() {
        currentFactionInt = 0;
        player1Faction = -1;
        player2Faction = -1;
    }

    public void Start()
    {
        unitCollection = UnitCollection.GetSingleton();
        currentFaction = unitCollection.FactionPicker(currentFactionInt);
        LoadFaction();
    }
    #endregion

    #region UI
    public void LoadFaction() {
        Debug.Log(currentFactionInt);
        currentFaction = unitCollection.FactionPicker(currentFactionInt);
        factionText.text = currentFaction[0].GetComponent<Character>().faction;
        firstUnit.sprite = currentFaction[0].GetComponent<Character>().sprite;
        firstUnit.gameObject.GetComponentInChildren<Text>().text = currentFaction[0].GetComponent<Character>().unitName;
        secondUnit.sprite = currentFaction[1].GetComponent<Character>().sprite;
        secondUnit.gameObject.GetComponentInChildren<Text>().text = currentFaction[1].GetComponent<Character>().unitName;
        thirdUnit.sprite = currentFaction[2].GetComponent<Character>().sprite;
        thirdUnit.gameObject.GetComponentInChildren<Text>().text = currentFaction[2].GetComponent<Character>().unitName;
    }

    public void Confirm() {
        if (beginButton.IsInteractable()) {
            return;
        }
        if (player1Faction == -1) {
            player1Faction = currentFactionInt;
            player1Image.sprite = currentFaction[0].GetComponent<Character>().sprite;
            player1Image.gameObject.GetComponentInChildren<Text>().text = currentFaction[0].GetComponent<Character>().faction;
            currentFactionInt++;
            LoadFaction();
        }
        else {
            player2Faction = currentFactionInt;
            player2Image.sprite = currentFaction[0].GetComponent<Character>().sprite;
            player2Image.gameObject.GetComponentInChildren<Text>().text = currentFaction[0].GetComponent<Character>().faction;
            beginButton.interactable = true;
        }
    }

    public void Begin() {
        PlayerPrefs.SetInt("Player1Faction", player1Faction);
        PlayerPrefs.SetInt("Player2Faction", player2Faction);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Left() {
        if (currentFactionInt <= 0) {
            return;
        }
        else {
            if (player1Faction != -1) {
                if (player1Faction == currentFactionInt - 1) {
                    if (currentFactionInt - 2 <= 0) {
                        return;
                    }
                    else {
                        currentFactionInt--;
                    }
                }
            }
            currentFactionInt--;

            LoadFaction();
        }
    }

    public void Right() {
        if (currentFactionInt >= 3) {
            return;
        }
        else {
            if (player1Faction != -1) {
                if (player1Faction == currentFactionInt + 1) {
                    if (currentFactionInt + 2 <= 0) {
                        return;
                    }
                    else {
                        currentFactionInt--;
                    }
                }
            }
            currentFactionInt++;
            LoadFaction();
        }
    }

    public void Back() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    #endregion
}
