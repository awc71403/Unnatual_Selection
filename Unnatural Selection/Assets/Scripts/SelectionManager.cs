using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    #region Variables
    private static SelectionManager m_Singleton;
    public static SelectionManager GetSingleton() {
        return m_Singleton;
    }

    [SerializeField]
    public Image selectionPanel;
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
    public TextMeshProUGUI factionText;

    public UnitCollection unitCollection;

    public static GameObject[] currentFaction;

    public int currentFactionInt;

    public int player1Faction;
    public int player2Faction;
    #endregion

    #region Initialization
    public void Awake() {
        // Singleton makes sure there is only one of this object
        if (m_Singleton != null) {
            DestroyImmediate(gameObject);
            return;
        }
        m_Singleton = this;

        currentFactionInt = 0;
        player1Faction = -1;
        player2Faction = -1;
    }

    public void Start()
    {
        unitCollection = UnitCollection.GetSingleton();
        currentFaction = unitCollection.FactionPicker(currentFactionInt);
        LoadFaction();
        UpdateFactionImage();
    }
    #endregion

    #region UI
    public void LoadFaction() {
        currentFaction = unitCollection.FactionPicker(currentFactionInt);
        factionText.text = currentFaction[0].GetComponent<Character>().faction;
        firstUnit.sprite = currentFaction[0].GetComponent<Character>().sprite;
        firstUnit.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = currentFaction[0].GetComponent<Character>().unitName;
        firstUnit.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"      - {currentFaction[0].GetComponent<Character>().cost}\n      - {currentFaction[0].GetComponent<Character>().totalHealth}\n      - {currentFaction[0].GetComponent<Character>().damage}\n      - {currentFaction[0].GetComponent<Character>().movement}\n      - {currentFaction[0].GetComponent<Character>().ability}";

        secondUnit.sprite = currentFaction[1].GetComponent<Character>().sprite;
        secondUnit.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = currentFaction[1].GetComponent<Character>().unitName;
        secondUnit.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"      - {currentFaction[1].GetComponent<Character>().cost}\n      - {currentFaction[1].GetComponent<Character>().totalHealth}\n      - {currentFaction[1].GetComponent<Character>().damage}\n      - {currentFaction[1].GetComponent<Character>().movement}\n      - {currentFaction[1].GetComponent<Character>().ability}";

        thirdUnit.sprite = currentFaction[2].GetComponent<Character>().sprite;
        thirdUnit.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = currentFaction[2].GetComponent<Character>().unitName;
        thirdUnit.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"      - {currentFaction[2].GetComponent<Character>().cost}\n      - {currentFaction[2].GetComponent<Character>().totalHealth}\n      - {currentFaction[2].GetComponent<Character>().damage}\n      - {currentFaction[2].GetComponent<Character>().movement}\n      - {currentFaction[2].GetComponent<Character>().ability}";
    }

    public void UpdateFactionImage() {
        if (player1Faction == -1) {
            player1Image.enabled = true;
            player1Image.sprite = currentFaction[0].GetComponent<Character>().factionSprite;
            player1Image.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = currentFaction[0].GetComponent<Character>().faction;
            player2Image.gameObject.SetActive(false);
            foreach (HoverButton button in selectionPanel.GetComponentsInChildren<HoverButton>()) {
                button.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                button.disabled = false;
            }
        }
        else {
            player2Image.enabled = true;
            player2Image.sprite = currentFaction[0].GetComponent<Character>().factionSprite;
            player2Image.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = currentFaction[0].GetComponent<Character>().faction;
            player2Image.gameObject.SetActive(true);
        }
    }

    public void Confirm() {
        if (player1Faction == -1) {
            player1Faction = currentFactionInt;
            if (currentFactionInt == 0 || currentFactionInt == 1) {
                currentFactionInt++;
            }
            else {
                currentFactionInt = 0;
            }
            LoadFaction();
            UpdateFactionImage();
        }
        else {
            player2Faction = currentFactionInt;
            UpdateFactionImage();
            Begin();
        }
    }

    public void Begin() {
        PlayerPrefs.SetInt("Player1Faction", player1Faction);
        PlayerPrefs.SetInt("Player2Faction", player2Faction);
        if (Wind.GetSingleton()) {
            Destroy(Wind.GetSingleton().gameObject);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Back() {
        if (player1Faction != -1) {
            player1Faction = -1;
            UpdateFactionImage();
        }
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
    #endregion
}
