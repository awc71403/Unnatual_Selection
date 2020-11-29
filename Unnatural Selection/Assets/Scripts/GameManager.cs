using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    private static GameManager m_Singleton;
    public static GameManager GetSingleton() {
        return m_Singleton;
    }

    public static int currentPlayer = 1;
    public int turnCounter = 1;
    public static bool actionInProcess;
    public static bool menuOpened;

    [SerializeField]
    public Button endButton;

    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    public GameObject testCharacter;

    public List<GameObject> player1Units;
    public List<GameObject> player2Units;
    public GameObject[] player1Faction;
    public GameObject[] player2Faction;

    public int player1Energy;
    public int player2Energy;

    public int player1ObjectivePoints;
    public int player2ObjectivePoints;

    public int player1NexusStrikes;
    public int player2NexusStrikes;

    public GameObject[,] mapArray;
    float tileSize;

    GameObject m_tilesObject;

    public CapturePoint capturePoint;

    public Image SummonPanel;

    public Image CharacterUI;

    public Image WinPanel;

    public GameObject boughtUnit;

    public UnitCollection unitCollection;

    public Image leftMenu;

    public int mapXSize;
    public int mapYSize;
    #region UI Variables
    public Text turnText;
    public Text currentFaction;

    public GameObject unitUI;
    public Text unitName;
    public Text unitHP;
    public Text unitDMG;

    public Text player1EnergyText;
    public Text player2EnergyText;

    public Slider player1ObjSlider;
    public Slider player2ObjSlider;

    public Text player1ObjText;
    public Text player2ObjText;
    #endregion
    #endregion

    #region Initialization
    public void Awake() {
        // Singleton makes sure there is only one of this object
        if (m_Singleton != null) {
            DestroyImmediate(gameObject);
            return;
        }
        m_Singleton = this;

        player1Units = new List<GameObject>();
        player1Energy = 10;
        player1ObjectivePoints = 0;
        player1NexusStrikes = 0;

        player2Units = new List<GameObject>();
        player2Energy = 2;
        player2ObjectivePoints = 0;
        player2NexusStrikes = 0;

        tileSize = tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        CreateTiles();
    }

    public void Start() {
        unitCollection = UnitCollection.GetSingleton();
        player1Faction = unitCollection.FactionPicker(PlayerPrefs.GetInt("Player1Faction"));
        player2Faction = unitCollection.FactionPicker(PlayerPrefs.GetInt("Player2Faction"));

        UpdateUI();
    }

    void Update() {
        if (Input.GetMouseButtonDown(1) && menuOpened)
            HideMenus();
    }
    #endregion

    #region Set Up
    public void CreateTiles() {

        m_tilesObject = new GameObject();
        m_tilesObject.name = "Tiles";

        string[] mapData = ReadLevelText();

        mapXSize = mapData[0].ToCharArray().Length;
        mapYSize = mapData.Length;

        // Fill mapArray, which should be empty at first.
        mapArray = new GameObject[mapXSize, mapYSize];

        // Calculate the size of the map.
        float mapWidth = mapXSize * tileSize;
        float mapHeight = mapYSize * tileSize;

        // Finds the top left corner.
        Vector3 worldStart = new Vector3(-mapWidth / 2.0f + (0.5f * tileSize), mapHeight / 2.0f - (0.5f * tileSize));

        // Nested for loop that creates mapYSize * mapXSize tiles.
        for (int y = 0; y < mapYSize; y++) {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapXSize; x++) {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

        for (int y = 0; y < mapYSize; y++) {
            Nexus player1side = mapArray[0, y].GetComponent<Nexus>();
            Nexus player2side = mapArray[mapXSize - 1, y].GetComponent<Nexus>();

            if (player1side) {
                player1side.playerside = 1;
            }
            if (player2side) {
                player2side.playerside = 2;
            }
        }
    }

    // Places a tile at position (x, y).
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart) {
        int tileIndex = int.Parse(tileType);

        GameObject newTile = Instantiate(tilePrefabs[tileIndex]);
        if (newTile.GetComponent<TileBehavior>().tileType == "capturepoint") {
            capturePoint = newTile.GetComponent<CapturePoint>();
        }

        if (newTile.GetComponent<TileBehavior>().tileType == "asphalt") {
            newTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/asphalt{Random.Range(1, 3)}");
        }

        //Put under tile object in Hierarchy
        newTile.transform.SetParent(m_tilesObject.transform);

        // Calculates where it should go.
        float newX = worldStart.x + (tileSize * x);
        float newY = worldStart.y - (tileSize * y);

        // Puts it there.
        newTile.transform.position = new Vector3(newX, newY, 0);
        newTile.GetComponent<TileBehavior>().xPosition = x;
        newTile.GetComponent<TileBehavior>().yPosition = y;

        // Adds it to mapArray so we can keep track of it later.
        mapArray[x, y] = newTile;
    }

    private string[] ReadLevelText() {
        TextAsset bindData = Resources.Load("Test") as TextAsset;
        string data = bindData.text.Replace("\r\n", string.Empty);
        return data.Split('-');
    }

    public void PlaceCharacterOnTile(GameObject unit, int x, int y, int player, bool exhausted = true) {
        // Instantiate an instance of the unit and place it on the given tile.
        GameObject newUnit = Instantiate(unit);
        newUnit.GetComponent<Character>().SetPlayer(player);
        newUnit.GetComponent<Character>().SetHPFull();
        mapArray[x, y].transform.GetComponent<TileBehavior>().PlaceUnit(newUnit);
        newUnit.GetComponent<Character>().previousTile = newUnit.GetComponent<Character>().occupiedTile;

        if (exhausted) {
            newUnit.GetComponent<Character>().SetCanMove(false);
            newUnit.GetComponent<Character>().SetCanAttack(false);
        }

        // Put the unit in the right player's array.
        if (player == 1) {
            player1Units.Add(newUnit);

        }
        else if (player == 2) {
            player2Units.Add(newUnit);
            newUnit.transform.rotation = Quaternion.Euler(0, 180, 0);
            Debug.Log(newUnit.GetComponentInChildren<TextMeshProUGUI>().gameObject.name);
            newUnit.GetComponentInChildren<TextMeshProUGUI>().rectTransform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    #endregion

    #region UI
    // currently only summons a testCharacter as a bought unit
    public void SummonUnit(int picker) {
        //Needs edit later when we implement all the faction units to generalize
        //Needs to check money but not subtract money yet
        if (currentPlayer == 1) {
            if (player1Energy >= player1Faction[picker].GetComponent<Character>().cost) {
                boughtUnit = player1Faction[picker];
            }
        }
        else {
            if (player2Energy >= player2Faction[picker].GetComponent<Character>().cost) {
                boughtUnit = player2Faction[picker];
            }
        }
        for (int i = 0; i < SummonPanel.GetComponentsInChildren<Button>().Length; i++) {
            if (picker == i) {
                SummonPanel.GetComponentsInChildren<Button>()[i].interactable = false;
            }
            else {
                SummonPanel.GetComponentsInChildren<Button>()[i].interactable = true;
            }
        }
    }

    public void OpenSummonPanel() {
        if (currentPlayer == 1) {
            SummonPanel.gameObject.GetComponentsInChildren<Image>()[2].sprite = player1Faction[0].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<Image>()[4].sprite = player1Faction[1].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<Image>()[6].sprite = player1Faction[2].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[0].text = player1Faction[0].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[1].text = $"Cost: {player1Faction[0].GetComponent<Character>().cost}\n\nHP: {player1Faction[0].GetComponent<Character>().totalHealth}\nDMG: {player1Faction[0].GetComponent<Character>().damage}\nSpeed: {player1Faction[0].GetComponent<Character>().movement}\n★: {player1Faction[0].GetComponent<Character>().ability}";
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[2].text = player1Faction[1].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[3].text = $"Cost: {player1Faction[1].GetComponent<Character>().cost}\n\nHP: {player1Faction[1].GetComponent<Character>().totalHealth}\nDMG: {player1Faction[1].GetComponent<Character>().damage}\nSpeed: {player1Faction[1].GetComponent<Character>().movement}\n★: {player1Faction[1].GetComponent<Character>().ability}";
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[4].text = player1Faction[2].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[5].text = $"Cost: {player1Faction[2].GetComponent<Character>().cost}\n\nHP: {player1Faction[2].GetComponent<Character>().totalHealth}\nDMG: {player1Faction[2].GetComponent<Character>().damage}\nSpeed: {player1Faction[2].GetComponent<Character>().movement}\n★: {player1Faction[2].GetComponent<Character>().ability}";
        }
        else {
            SummonPanel.gameObject.GetComponentsInChildren<Image>()[2].sprite = player2Faction[0].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<Image>()[4].sprite = player2Faction[1].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<Image>()[6].sprite = player2Faction[2].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[0].text = player2Faction[0].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[1].text = $"Cost: {player2Faction[0].GetComponent<Character>().cost}\n\nHP: {player2Faction[0].GetComponent<Character>().totalHealth}\nDMG: {player2Faction[0].GetComponent<Character>().damage}\nSpeed: {player2Faction[0].GetComponent<Character>().movement}\n★: {player2Faction[0].GetComponent<Character>().ability}";
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[2].text = player2Faction[1].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[3].text = $"Cost: {player2Faction[1].GetComponent<Character>().cost}\n\nHP: {player2Faction[1].GetComponent<Character>().totalHealth}\nDMG: {player2Faction[1].GetComponent<Character>().damage}\nSpeed: {player2Faction[1].GetComponent<Character>().movement}\n★: {player2Faction[1].GetComponent<Character>().ability}";
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[4].text = player2Faction[2].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[5].text = $"Cost: {player2Faction[2].GetComponent<Character>().cost}\n\nHP: {player2Faction[2].GetComponent<Character>().totalHealth}\nDMG: {player2Faction[2].GetComponent<Character>().damage}\nSpeed: {player2Faction[2].GetComponent<Character>().movement}\n★: {player2Faction[2].GetComponent<Character>().ability}";
        }
        foreach (Button button in SummonPanel.GetComponentsInChildren<Button>()) {
            button.interactable = true;
        }
        SummonPanel.gameObject.SetActive(true);
    }

    public void ConfirmSummonPanel() {
        if (boughtUnit != null) {
            SummonPanel.gameObject.SetActive(false);
        }
    }

    public void ExitSummonPanel() {
        //Unhighlight everything
        TileBehavior.Deselect();
        boughtUnit = null;
        SummonPanel.gameObject.SetActive(false);

        endButton.gameObject.SetActive(true);
    }

    public void ShowCharacterUI(GameObject selectedUnit) {
        unitUI.SetActive(true);
        Character unit = selectedUnit.GetComponent<Character>();
        unitName.text = unit.unitName;
        unitHP.text = $"HP: {unit.currentHealth.ToString()}/{unit.totalHealth.ToString()}";
        unitDMG.text = $"DMG: {unit.damage.ToString()}";
    }

    public void UpdateUI() {
        if (currentPlayer == 1) {
            player1EnergyText.text = $"Energy: {player1Energy.ToString()}";
            currentFaction.text = $"{player1Faction[0].GetComponent<Character>().faction}'s Turn";
        }
        else {
            player2EnergyText.text = $"Energy: {player2Energy.ToString()}";
            currentFaction.text = $"{player2Faction[0].GetComponent<Character>().faction}'s Turn";
        }
        player1ObjText.text = $"Objective: {player1ObjectivePoints}/100";
        player2ObjText.text = $"Objective: {player2ObjectivePoints}/100";

        player1ObjSlider.value = player1ObjectivePoints / 100.0f;
        player2ObjSlider.value = player2ObjectivePoints / 100.0f;
    }

    public void ClearUI() {
        unitUI.SetActive(false);
    }

    public void PressEndTurnButton() {
        if (currentPlayer == 1) {
            currentPlayer = 2;
            player2Energy += 10;
            endButton.GetComponent<Image>().color = new Color(0f, 0f, 1f, 1f);
        } else {
            currentPlayer = 1;
            player1Energy += 10;
            turnCounter++;
            endButton.GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
            if (turnCounter > 12) {
                CheckEndGame();
            }
            else {
                turnText.text = $"Turn {turnCounter.ToString()}";
            }
        }
        UpdateUI();

        //For every character in Player 1, set can move and can attack.
        foreach (GameObject unit in player1Units) {
            unit.GetComponent<Character>().SetCanMove(true);
            unit.GetComponent<Character>().SetCanAttack(true);
            unit.GetComponent<Character>().ResetStats();
            if (player1Faction[0].GetComponent<Character>().faction == "Mech" && currentPlayer == 1) {
                unit.GetComponent<TestClass>().Ability();
            }
        }
        //For every character in Player 2/Enemy, set can move and can attack.
        foreach (GameObject unit in player2Units) {
            unit.GetComponent<Character>().SetCanMove(true);
            unit.GetComponent<Character>().SetCanAttack(true);
            unit.GetComponent<Character>().ResetStats();
            if (player2Faction[0].GetComponent<Character>().faction == "Mech" && currentPlayer == 2) {
                unit.GetComponent<TestClass>().Ability();
            }
        }
        //Reset selection state.
        if (TileBehavior.GetSelectionState() != null) {
            TileBehavior.selectedTile.GetComponent<TileBehavior>().SelectionStateToNull();
        }
        AddCTPObjectivePoints();
        HideMenus();
    }

    public void ShowLeftMenu() {
        leftMenu.gameObject.SetActive(true);
        leftMenu.transform.position = Input.mousePosition;
        menuOpened = true;
    }

    public void HideMenus() {
        leftMenu.gameObject.SetActive(false);
        menuOpened = false;
    }

    public void Surrender() {
        HideMenus();
        if (currentPlayer == 1) {
            TriggerEndGame(2);
        }
        else {
            TriggerEndGame(1);
        }
    }
    #endregion

    #region Helper
    public void SubtractCost() {
        if (currentPlayer == 1) {
            player1Energy -= boughtUnit.GetComponent<Character>().cost;

        }
        else {
            player2Energy -= boughtUnit.GetComponent<Character>().cost;
        }
        UpdateUI();
        endButton.gameObject.SetActive(true);
        boughtUnit = null;
    }

    public int getCurrent()
    {
        return currentPlayer;
    }

    public void AddNexusObjectivePoints()
    {
        if (currentPlayer == 1) { 
            player1ObjectivePoints += 20;
            player1NexusStrikes++;
            if (player1NexusStrikes >= 3) {
                TriggerEndGame(1);
            }
        }
        else {
            player2ObjectivePoints += 20;
            player2NexusStrikes++;
            if (player2NexusStrikes >= 3) {
                TriggerEndGame(2);
            }
        }
        UpdateUI();
    }

    public void AddCTPObjectivePoints()
    {
        if (currentPlayer == capturePoint.ownedBy) {
            if (currentPlayer == 1) {
                player1ObjectivePoints += 10;
            } else {
                player2ObjectivePoints += 10;
            }
            UpdateUI();
            CheckEndGame();
        }
    }

    public void AddObjPoints(int player, int points) {
        if (player == 1) {
            player1ObjectivePoints += points;
        }
        else {
            player2ObjectivePoints += points;
        }
        CheckEndGame();
    }

    public void CheckEndGame() {
        if (player1ObjectivePoints >= 100) {
            TriggerEndGame(1);
        }
        else if (player2ObjectivePoints >= 100) {
            TriggerEndGame(2);
        }
        else if (turnCounter > 12) {
            if (player1ObjectivePoints > player2ObjectivePoints) {
                TriggerEndGame(1);
            }
            else {
                TriggerEndGame(2);
            }
        }
    }

    public void TriggerEndGame(int player = 0) {
        menuOpened = true;
        WinPanel.gameObject.SetActive(true);
        if (player == 1) {
            WinPanel.GetComponentInChildren<TextMeshProUGUI>().text = $"{player1Faction[0].GetComponent<Character>().faction}s Win!";
        } else {
            WinPanel.GetComponentInChildren<TextMeshProUGUI>().text = $"{player2Faction[0].GetComponent<Character>().faction}s Win!";
        }
    }

    public void ReturnTitle() {
        SceneManager.LoadScene(0);
        menuOpened = false;
        Destroy(UnitCollection.GetSingleton().gameObject);
    }
    #endregion
}
