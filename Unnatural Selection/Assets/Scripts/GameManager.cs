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
    public static bool infoOpened;

    [SerializeField]
    public Button endButton;

    [SerializeField]
    private GameObject[] tilePrefabs;

    public List<GameObject> player1Units;
    public List<GameObject> player2Units;
    public GameObject[] player1Faction;
    public GameObject[] player2Faction;

    public int player1Energy;
    public int player2Energy;

    public int player1ObjectivePoints;
    public int player2ObjectivePoints;
    public string player1FactionName;

    public int player1NexusStrikes;
    public int player2NexusStrikes;
    public string player2FactionName;

    public GameObject[,] mapArray;
    float tileSize;

    GameObject m_tilesObject;

    public CapturePoint capturePoint;

    public Image SummonPanel;

    public Image SurrenderPanel;

    public Image InfoUI;

    public Image summon1;
    public Image summon2;
    public Image summon3;

    public GameObject boughtUnit;

    public UnitCollection unitCollection;

    public Image leftMenu;

    public float timer;
    private float timeRemaining;

    public int mapXSize;
    public int mapYSize;
    #region UI Variables
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI timerText;

    public GameObject unitUI;
    public TextMeshProUGUI unitInfo;
    public Transform unitImageTransform;

    public TextMeshProUGUI player1EnergyText;
    public TextMeshProUGUI player2EnergyText;

    public GameObject player1NexusStrikesUI;
    public GameObject player2NexusStrikesUI;

    public PlayerReference player1PlayerUI;
    public PlayerReference player2PlayerUI;

    public Slider player1ObjSlider;
    public Slider player2ObjSlider;
    public Slider player1ReserveSlider;
    public Slider player2ReserveSlider;

    public TextMeshProUGUI player1ObjText;
    public TextMeshProUGUI player2ObjText;
    public TextMeshProUGUI player1ReserveText;
    public TextMeshProUGUI player2ReserveText;

    public Image player1FactionImage;
    public Image player2FactionImage;

    private float player1Reserve;
    private float player2Reserve;

    private float reserveTime;
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

        reserveTime = 120;

        player1Units = new List<GameObject>();
        player1Energy = 10;
        player1ObjectivePoints = 0;
        player1NexusStrikes = 0;
        player1Reserve = reserveTime;
        player1ReserveText.text = ((int)player1Reserve).ToString();

        player2Units = new List<GameObject>();
        player2Energy = 2;
        player2ObjectivePoints = 0;
        player2NexusStrikes = 0;
        player2Reserve = reserveTime;
        player2ReserveText.text = ((int)player2Reserve).ToString();

        timeRemaining = timer;

        menuOpened = false;

        tileSize = tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        CreateTiles();
    }

    public void Start() {
        unitCollection = UnitCollection.GetSingleton();
        player1Faction = unitCollection.FactionPicker(PlayerPrefs.GetInt("Player1Faction"));
        player2Faction = unitCollection.FactionPicker(PlayerPrefs.GetInt("Player2Faction"));

        player1PlayerUI.unit1.sprite = player1Faction[0].GetComponent<Character>().sprite;
        player1PlayerUI.unit2.sprite = player1Faction[1].GetComponent<Character>().sprite;
        player1PlayerUI.unit3.sprite = player1Faction[2].GetComponent<Character>().sprite;

        player2PlayerUI.unit1.sprite = player2Faction[0].GetComponent<Character>().sprite;
        player2PlayerUI.unit2.sprite = player2Faction[1].GetComponent<Character>().sprite;
        player2PlayerUI.unit3.sprite = player2Faction[2].GetComponent<Character>().sprite;

        player1FactionName = player1Faction[0].GetComponent<Character>().faction;
        player2FactionName = player2Faction[0].GetComponent<Character>().faction;

        player1FactionImage.sprite = player1Faction[0].GetComponent<Character>().factionSprite;
        player2FactionImage.sprite = player2Faction[0].GetComponent<Character>().factionSprite;
        switch (player1FactionName) {
            case "Insect":
                player1ObjSlider.GetComponentsInChildren<Image>()[1].color = new Color(1, 0, 0);
                foreach (Image image in player1NexusStrikesUI.GetComponentsInChildren<Image>()) {
                    image.color = new Color(1, 0, 0);
                }
                break;
            case "Mech":
                player1ObjSlider.GetComponentsInChildren<Image>()[1].color = new Color(0, 0, 1);
                foreach (Image image in player1NexusStrikesUI.GetComponentsInChildren<Image>()) {
                    image.color = new Color(0, 0, 1);
                }
                break;
            case "Rock":
                player1ObjSlider.GetComponentsInChildren<Image>()[1].color = new Color(1, 0, 1);
                foreach (Image image in player1NexusStrikesUI.GetComponentsInChildren<Image>()) {
                    image.color = new Color(1, 0, 1);
                }
                break;
            case "Shadow":
                player1ObjSlider.GetComponentsInChildren<Image>()[1].color = new Color(0, 1, 0);
                foreach (Image image in player1NexusStrikesUI.GetComponentsInChildren<Image>()) {
                    image.color = new Color(0, 1, 0);
                }
                break;
        }

        switch (player2FactionName) {
            case "Insect":
                player2ObjSlider.GetComponentsInChildren<Image>()[1].color = new Color(1, 0, 0);
                foreach (Image image in player2NexusStrikesUI.GetComponentsInChildren<Image>()) {
                    image.color = new Color(1, 0, 0);
                }
                break;
            case "Mech":
                player2ObjSlider.GetComponentsInChildren<Image>()[1].color = new Color(0, 0, 1);
                foreach (Image image in player2NexusStrikesUI.GetComponentsInChildren<Image>()) {
                    image.color = new Color(0, 0, 1);
                }
                break;
            case "Rock":
                player2ObjSlider.GetComponentsInChildren<Image>()[1].color = new Color(1, 0, 1);
                foreach (Image image in player2NexusStrikesUI.GetComponentsInChildren<Image>()) {
                    image.color = new Color(1, 0, 1);
                }
                break;
            case "Shadow":
                player2ObjSlider.GetComponentsInChildren<Image>()[1].color = new Color(0, 1, 0);
                foreach (Image image in player2NexusStrikesUI.GetComponentsInChildren<Image>()) {
                    image.color = new Color(0, 1, 0);
                }
                break;
        }

        ColorBlock colors = endButton.GetComponent<Button>().colors;
        switch (player1FactionName) {
            case "Insect":
                colors.normalColor = Color.red;
                break;
            case "Mech":
                colors.normalColor = Color.blue;
                break;
            case "Rock":
                colors.normalColor = new Color(1, 0, 1);
                break;
            case "Shadow":
                colors.normalColor = Color.green;
                break;
        }
        endButton.GetComponent<Button>().colors = colors;

        UpdateUI();


    }

    void Update() {
        if (Input.GetMouseButtonUp(1) && menuOpened) {
            HideMenus();
        }
        if (timeRemaining > 0) {
            timeRemaining -= Time.deltaTime;
            timerText.text = ((int)timeRemaining).ToString();
        }
        else {
            if (currentPlayer == 1) {
                if (player1Reserve > 0) {
                    player1Reserve -= Time.deltaTime;
                    player1ReserveSlider.value = player1Reserve / reserveTime;
                    player1ReserveText.text = ((int)player1Reserve).ToString();
                }
                else {
                    PressEndTurnButton();
                }
            }
            else {
                if (player2Reserve > 0) {
                    player2Reserve -= Time.deltaTime;
                    player2ReserveSlider.value = player2Reserve / reserveTime;
                    player2ReserveText.gameObject.GetComponent<TextMeshProUGUI>().text = ((int)player2Reserve).ToString();
                }
                else {
                    PressEndTurnButton();
                }
            }
        }
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
            else {
                boughtUnit = null;
            }
        }
        else {
            if (player2Energy >= player2Faction[picker].GetComponent<Character>().cost) {
                boughtUnit = player2Faction[picker];
            }
            else {
                boughtUnit = null;
            }
        }
        for (int i = 0; i < SummonPanel.GetComponentsInChildren<Button>().Length; i++) {
            if (picker == i) {
                SummonPanel.GetComponentsInChildren<Button>()[i].interactable = false;
                SummonPanel.GetComponentsInChildren<Button>()[i].gameObject.GetComponent<Image>().color = new Color(.5f, .5f, .5f);
            }
            else {
                SummonPanel.GetComponentsInChildren<Button>()[i].interactable = true;
                SummonPanel.GetComponentsInChildren<Button>()[i].gameObject.GetComponent<Image>().color = new Color(255, 255, 255);
            }
        }
    }

    public void OpenSummonPanel() {
        menuOpened = true;
        if (currentPlayer == 1) {
            summon1.sprite = player1Faction[0].GetComponent<Character>().sprite;
            summon2.sprite = player1Faction[1].GetComponent<Character>().sprite;
            summon3.sprite = player1Faction[2].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = player1Faction[0].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"      - {player1Faction[0].GetComponent<Character>().cost}\n      - {player1Faction[0].GetComponent<Character>().totalHealth}\n      - {player1Faction[0].GetComponent<Character>().damage}\n      - {player1Faction[0].GetComponent<Character>().movement}\n      - {player1Faction[0].GetComponent<Character>().ability}";
            SummonPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[2].text = player1Faction[1].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[3].text = $"      - {player1Faction[1].GetComponent<Character>().cost}\n      - {player1Faction[1].GetComponent<Character>().totalHealth}\n      - {player1Faction[1].GetComponent<Character>().damage}\n      - {player1Faction[1].GetComponent<Character>().movement}\n      - {player1Faction[1].GetComponent<Character>().ability}";
            SummonPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[4].text = player1Faction[2].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[5].text = $"      - {player1Faction[2].GetComponent<Character>().cost}\n      - {player1Faction[2].GetComponent<Character>().totalHealth}\n      - {player1Faction[2].GetComponent<Character>().damage}\n      - {player1Faction[2].GetComponent<Character>().movement}\n      - {player1Faction[2].GetComponent<Character>().ability}";
        }
        else {
            summon1.sprite = player2Faction[0].GetComponent<Character>().sprite;
            summon2.sprite = player2Faction[1].GetComponent<Character>().sprite;
            summon3.sprite = player2Faction[2].GetComponent<Character>().sprite;
            SummonPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0].text = player2Faction[0].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"      - {player2Faction[0].GetComponent<Character>().cost}\n      - {player2Faction[0].GetComponent<Character>().totalHealth}\n      - {player2Faction[0].GetComponent<Character>().damage}\n      - {player2Faction[0].GetComponent<Character>().movement}\n      - {player2Faction[0].GetComponent<Character>().ability}";
            SummonPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[2].text = player2Faction[1].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[3].text = $"      - {player2Faction[1].GetComponent<Character>().cost}\n      - {player2Faction[1].GetComponent<Character>().totalHealth}\n      - {player2Faction[1].GetComponent<Character>().damage}\n      - {player2Faction[1].GetComponent<Character>().movement}\n      - {player2Faction[1].GetComponent<Character>().ability}";
            SummonPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[4].text = player2Faction[2].GetComponent<Character>().name;
            SummonPanel.gameObject.GetComponentsInChildren<TextMeshProUGUI>()[5].text = $"      - {player2Faction[2].GetComponent<Character>().cost}\n      - {player2Faction[2].GetComponent<Character>().totalHealth}\n      - {player2Faction[2].GetComponent<Character>().damage}\n      - {player2Faction[2].GetComponent<Character>().movement}\n      - {player2Faction[2].GetComponent<Character>().ability}";
        }
        Button[] buttons = SummonPanel.GetComponentsInChildren<Button>();
        GameObject[] faction;
        int energy;
        if (currentPlayer == 1) {
            faction = player1Faction;
            energy = player1Energy;
        }
        else {
            faction = player2Faction;
            energy = player2Energy;
        }

        for (int i = 0; i < 3; i++) {
            if (energy < faction[i].GetComponent<Character>().cost) {
                buttons[i].gameObject.GetComponentsInParent<Image>(true)[1].color = new Color(1f, .5f, .5f, .3f);
            }
            else {
                buttons[i].gameObject.GetComponentsInParent<Image>(true)[1].color = new Color(1f, 1f, 1f, .3f);
            }
            buttons[i].interactable = true;
            buttons[i].gameObject.GetComponent<Image>().color = new Color(255, 255, 255);
        }
        SummonPanel.gameObject.SetActive(true);
    }

    public void ConfirmSummonPanel() {
        if (boughtUnit != null) {
            SummonPanel.gameObject.SetActive(false);
            menuOpened = false;
        }
    }

    public void ExitSummonPanel() {
        //Unhighlight everything
        TileBehavior.Deselect();
        boughtUnit = null;
        menuOpened = false;
        SummonPanel.gameObject.SetActive(false);
    }

    public void ShowInfoUI(GameObject selectedUnit, TileBehavior selectedTile) {
        infoOpened = true;
        menuOpened = true;
        InfoUI.gameObject.SetActive(true);
        ReferenceHolder Info = InfoUI.GetComponent<ReferenceHolder>();
        Info.unitObject.SetActive(true);

        if (selectedUnit == null) {
            Info.unitObject.SetActive(false);
        }
        else {
            Info.unitImage.sprite = selectedUnit.GetComponent<Character>().sprite;
            Info.unitName.text = selectedUnit.GetComponent<Character>().unitName;
            Info.unitInfo.text = $"      - {selectedUnit.GetComponent<Character>().cost}\n      - {selectedUnit.GetComponent<Character>().totalHealth}\n      - {selectedUnit.GetComponent<Character>().damage}\n      - {selectedUnit.GetComponent<Character>().movement}\n      - {selectedUnit.GetComponent<Character>().ability}";
        }
        Info.tileImage.sprite = selectedTile.GetComponent<SpriteRenderer>().sprite;
        Info.tileName.text = selectedTile.tileName;
        Info.tileInfo.text = selectedTile.tileDesc;
    }

    public void ShowCharacterUI(GameObject selectedUnit) {
        unitUI.SetActive(true);
        Character unit = selectedUnit.GetComponent<Character>();
        unitInfo.text = $"      - {unit.currentHealth.ToString()}/{unit.totalHealth.ToString()}\n      - {unit.damage.ToString()}";
        unitUI.GetComponentsInChildren<Image>()[1].sprite = unit.sprite;
        RectTransform transform = unitUI.GetComponent<RectTransform>();
        var pos = transform.localPosition;
        if (selectedUnit.GetComponent<Character>().faction == player1FactionName) {
            pos.x = (float)-869.5;
            unitImageTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else {
            pos.x = (float)850;
            unitImageTransform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        transform.localPosition = pos;
    }

    public void UpdateUI() {
        if (currentPlayer == 1) {
            player1EnergyText.text = $"- {player1Energy.ToString()}";
            player1PlayerUI.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 0f);
            player2PlayerUI.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
        else {
            player2EnergyText.text = $"- {player2Energy.ToString()}";
            player1PlayerUI.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            player2PlayerUI.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 0f);
        }
        player1ObjText.text = $"{player1ObjectivePoints}/100";
        player2ObjText.text = $"{player2ObjectivePoints}/100";

        player1ObjSlider.value = player1ObjectivePoints / 100.0f;
        player2ObjSlider.value = player2ObjectivePoints / 100.0f;

        if (turnCounter <= 12) {
            turnText.text = $"Turn {turnCounter.ToString()}";
        }
    }

    public void ClearUI() {
        unitUI.SetActive(false);
    }

    public void PressEndTurnButton() {
        if (currentPlayer == 1) {
            currentPlayer = 2;
            player2Energy += 10;
            ColorBlock colors = endButton.GetComponent<Button>().colors;
            switch (player2FactionName) {
                case "Insect":
                    colors.normalColor = Color.red;
                    break;
                case "Mech":
                    colors.normalColor = Color.blue;
                    break;
                case "Rock":
                    colors.normalColor = new Color(1, 0, 1);
                    break;
                case "Shadow":
                    colors.normalColor = Color.green;
                    break;
            }
            endButton.GetComponent<Button>().colors = colors;
        } else {
            turnCounter++;
            if (turnCounter > 12) {
                CheckEndGame();
                return;
            } else {
                currentPlayer = 1;
                player1Energy += 10;
                ColorBlock colors = endButton.GetComponent<Button>().colors;
                switch (player1FactionName) {
                    case "Insect":
                        colors.normalColor = Color.red;
                        break;
                    case "Mech":
                        colors.normalColor = Color.blue;
                        break;
                    case "Rock":
                        colors.normalColor = new Color(1, 0, 1);
                        break;
                    case "Shadow":
                        colors.normalColor = Color.green;
                        break;
                }
                endButton.GetComponent<Button>().colors = colors;
            }
        }
        HideMenus();
        UpdateUI();

        //For every character in Player 1, set can move and can attack.
        foreach (GameObject unit in player1Units) {
            unit.GetComponent<Character>().SetCanMove(true);
            unit.GetComponent<Character>().SetCanAttack(true);
            unit.GetComponent<Character>().ResetStats();
            if (player1FactionName == "Mech" && currentPlayer == 1) {
                unit.GetComponent<TestClass>().Ability();
            }
        }
        //For every character in Player 2/Enemy, set can move and can attack.
        foreach (GameObject unit in player2Units) {
            unit.GetComponent<Character>().SetCanMove(true);
            unit.GetComponent<Character>().SetCanAttack(true);
            unit.GetComponent<Character>().ResetStats();
            if (player2FactionName == "Mech" && currentPlayer == 2) {
                unit.GetComponent<TestClass>().Ability();
            }
        }
        //Reset selection state.
        if (TileBehavior.GetSelectionState() != null) {
            TileBehavior.selectedTile.GetComponent<TileBehavior>().SelectionStateToNull();
        }
        AddCTPObjectivePoints();
        HideMenus();

        timeRemaining = timer;
    }

    public void ShowLeftMenu() {
        leftMenu.gameObject.SetActive(true);
        leftMenu.transform.position = Input.mousePosition;
        menuOpened = true;
    }

    public void HideMenus() {
        if (infoOpened) {
            infoOpened = false;
        }
        else {
            menuOpened = false;
            infoOpened = false;
            leftMenu.gameObject.SetActive(false);
            SurrenderPanel.gameObject.SetActive(false);
            InfoUI.gameObject.SetActive(false);
            ExitSummonPanel();
        }
    }

    public void SurrenderOption() {
        leftMenu.gameObject.SetActive(false);
        SurrenderPanel.gameObject.SetActive(true);
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
            player2NexusStrikesUI.GetComponentsInChildren<Image>()[player1NexusStrikes].color = new Color(.5f, .5f, .5f);
            player1NexusStrikes++;
            if (player1NexusStrikes >= 3) {
                TriggerEndGame(1);
            }
        }
        else {
            player2ObjectivePoints += 20;
            player1NexusStrikesUI.GetComponentsInChildren<Image>()[player1NexusStrikes].color = new Color(.5f, .5f, .5f);
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
            else if (player2ObjectivePoints > player1ObjectivePoints) {
                TriggerEndGame(2);
            }
            else {
                turnText.text = "Overtime";
            }
        }
    }

    public void TriggerEndGame(int player = 0) {
        if (player == 1) {
            PlayerPrefs.SetString("Winner", player1Faction[0].GetComponent<Character>().faction);
        } else {
            PlayerPrefs.SetString("Winner", player2Faction[0].GetComponent<Character>().faction);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReturnTitle() {
        SceneManager.LoadScene(0);
        menuOpened = false;
        Destroy(UnitCollection.GetSingleton().gameObject);
    }
    #endregion
}
