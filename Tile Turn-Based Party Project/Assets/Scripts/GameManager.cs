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

    public static int currentPlayer = 2;
    public static bool actionInProcess;

    [SerializeField]
    private Button m_attackButton;

    [SerializeField]
    private Button SummonUI;

    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    GameObject testCharacter;

    public static List<GameObject> player1Units; // changed to public static by Levana
    public static List<GameObject> player2Units; // changed to public static by Levana

    public static GameObject[,] mapArray; // changed to public static by Levana
        float tileSize;

    GameObject m_tilesObject;

    

    public static GameObject boughtUnit; // changed to public static by Levana
    #endregion

    #region Initialization
    public Button GetSummonUI() // added by Levana for access in other classes
    {
        return SummonUI;
    }

    public void Awake() {
        // Singleton makes sure there is only one of this object
        if (m_Singleton != null) {
            DestroyImmediate(gameObject);
            return;
        }
        m_Singleton = this;

        player1Units = new List<GameObject>();
        player2Units = new List<GameObject>();

        m_attackButton.onClick.AddListener(PressAttackButton);
        GetSummonUI().onClick.AddListener(SummonClick); // ensures you can do something with summon button in SummonClick

        tileSize = tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        CreateTiles();
    }

    public void Start() {
        // FOR TESTING PURPOSES
        PlaceCharacterOnTile(testCharacter, 0, 1, 1);
        //added by Levana to test summoning tile behavior. may remove if necessary
        SummonUI.gameObject.SetActive(false); // ensures summon button is closed at start. button only appears when Nexus is clicked
        
    }
    // added by Levana to test summoning tile behavior.
    // I wanted to make sure that the summon button appears when I click on a nexus tile
    // and disappears when I click on the summon button.
    // May change/remove if necessary.
    public void SummonClick()
    {
        SummonUnit(); // goes through the process to summon a unit
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
    }

    // Places a tile at position (x, y).
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart) {
        int tileIndex = int.Parse(tileType);

        // Creates a new tile instance.
        GameObject newTile = Instantiate(tilePrefabs[tileIndex]);

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

    public static void PlaceCharacterOnTile(GameObject unit, int x, int y, int player) { // changed to public static by Levana
        // Instantiate an instance of the unit and place it on the given tile.
        GameObject newUnit = Instantiate(unit);
        newUnit.GetComponent<Character>().SetPlayer(player);
        newUnit.GetComponent<Character>().SetHPFull();
        mapArray[x, y].transform.GetComponent<TileBehavior>().PlaceUnit(newUnit);

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
    // Coded by Levana
    public void SummonUnit()
    {
        boughtUnit = testCharacter;
        // closes the Summon UI
        SummonUI.gameObject.SetActive(false);
        // designates 1,1 to be the selectedTile (so that Levana can debug)
        // for now, this will also be where the bought unit will go (which definitely isn't where it should actually go)
        TileBehavior.SetSelectedTile(mapArray[1,1]);
        TileBehavior.selectedTile.GetComponent<TileBehavior>().SelectionStateToSummon();

    }
    public void ShowCharacterUI(GameObject selectedUnit) {
    }

    public void ClearUI() {

    }

    public void PressAttackButton() {
        if (TileBehavior.selectedUnit != null && TileBehavior.selectedUnit.GetComponent<Character>().GetCanAttack()) {
            TileBehavior.AttackSelection();
        }
    }

    public void PressEndTurnButton() {
        if (currentPlayer == 1)
        {
            currentPlayer = 2;
        }
        else
        {
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
    }
    #endregion
}
