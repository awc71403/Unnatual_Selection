using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    private static GameManager m_Singleton;
    public static GameManager GetSingleton() {
        return m_Singleton;
    }

    public static int currentPlayer = 1;
    public static bool actionInProcess;

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

    public GameObject[,] mapArray;
        float tileSize;

    GameObject m_tilesObject;

    public CapturePoint capturePoint;

    public Image SummonPanel;

    public GameObject boughtUnit;

    public UnitCollection unitCollection;
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
        player1Energy = 0;
        player1ObjectivePoints = 0;

        player2Units = new List<GameObject>();
        player2Energy = 0;
        player2ObjectivePoints = 0;

        tileSize = tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        CreateTiles();
    }

    public void Start() {
        unitCollection = UnitCollection.GetSingleton();
        player1Faction = unitCollection.FactionPicker(PlayerPrefs.GetInt("Player1Faction"));
        player2Faction = unitCollection.FactionPicker(PlayerPrefs.GetInt("Player2Faction"));
    }
    #endregion

    #region Set Up
    public void CreateTiles() {

        m_tilesObject = new GameObject();
        m_tilesObject.name = "Tiles";

        string[] mapData = ReadLevelText();

        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;

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

        // Creates a new tile instance.
        GameObject newTile = Instantiate(tilePrefabs[tileIndex]);
        if (newTile.GetComponent<TileBehavior>().tileType == "capturepoint") {
            capturePoint = newTile.GetComponent<CapturePoint>();
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
        }
    }
    #endregion

    #region UI
    // currently only summons a testCharacter as a bought unit
    public void SummonUnit(int picker) {
        //Needs edit later when we implement all the faction units to generalize
        //Needs to check money but not subtract money yet
        if (currentPlayer == 1) {
            boughtUnit = player1Faction[picker];
        }
        else {
            boughtUnit = player2Faction[picker];
        }
    }

    public void OpenSummonPanel() {
        if (currentPlayer == 1) {
            SummonPanel.gameObject.GetComponentsInChildren<Image>()[1].sprite = player1Faction[0].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<Image>()[2].sprite = player1Faction[1].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<Image>()[3].sprite = player1Faction[2].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[0].text = player1Faction[0].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[1].text = player1Faction[1].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[2].text = player1Faction[2].GetComponent<Character>().name;
        }
        else {
            SummonPanel.gameObject.GetComponentsInChildren<Image>()[1].sprite = player2Faction[0].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<Image>()[2].sprite = player2Faction[1].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<Image>()[3].sprite = player2Faction[2].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[0].text = player2Faction[0].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[1].text = player2Faction[1].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<Text>()[2].text = player2Faction[2].GetComponent<Character>().name;
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
    }

    public void ClearUI() {

    }

    public void PressEndTurnButton() {
        if (currentPlayer == 1) {
            currentPlayer = 2;
        } else {
            currentPlayer = 1;
        }

        //For every character in Player 1, set can move and can attack.
        foreach (GameObject unit in player1Units) {
            unit.GetComponent<Character>().SetCanMove(true);
            unit.GetComponent<Character>().SetCanAttack(true);
        }
        //For every character in Player 2/Enemy, set can move and can attack.
        foreach (GameObject unit in player2Units) {
            unit.GetComponent<Character>().SetCanMove(true);
            unit.GetComponent<Character>().SetCanAttack(true);
        }
        //Reset selection state.
        if (TileBehavior.GetSelectionState() != null) {
            TileBehavior.selectedTile.GetComponent<TileBehavior>().SelectionStateToNull();
        }
        AddCTPObjectivePoints();
        Debug.Log($"{player1ObjectivePoints}:{player2ObjectivePoints}");
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
        }
        else {
            player2ObjectivePoints += 20;
        }
    }

    public void AddCTPObjectivePoints()
    {
        if (currentPlayer == capturePoint.ownedBy) {
            if (currentPlayer == 1) {
                player1ObjectivePoints += 10;
            } else {
                player2ObjectivePoints += 10;
            }
        }
    }
    #endregion
}
