using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class TileBehavior : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler {
    #region Selection Variables
    static List<GameObject> highlightedTiles = new List<GameObject>();
    static List<GameObject> moveableTiles = new List<GameObject>();
    public static List<GameObject> executableTiles = new List<GameObject>();
    public static GameObject selectedUnit;
    public static GameObject selectedTile;
    protected static string selectionState;
    #endregion

    #region Instance Variables
    bool highlighted;
    public GameObject myUnit;
    public int movementCost = 1;
    public int xPosition;
    public int yPosition;
    public string tileType;
    public string tileName;
    public string tileDesc;
    public int playerside;

    [SerializeField]
    GameObject tileHighlighter;
    Animator tileHighlighterAnimator;
    public float playerOpacity;
    public float enemyOpacity;

    [SerializeField]
    private AudioClip[] warpSounds;
    AudioSource audioSource;

    float stepDuration = 0.1f;

    public static float tileDim;
    #endregion

    #region Initialization
    public void Awake() {
        tileHighlighter.transform.position = transform.position;
        tileHighlighterAnimator = tileHighlighter.GetComponent<Animator>();
        setHighlightOpacity(playerOpacity);
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region Opacity
    private void setHighlightOpacity(float opacity) {
        Color c = tileHighlighter.GetComponent<Renderer>().material.color;
        c.a = opacity;
        tileHighlighter.GetComponent<Renderer>().material.color = c;
    }
    #endregion

    #region Unit Functions
    public void PlaceUnit(GameObject unit) {
        if (unit.GetComponent<Character>().occupiedTile) {
            unit.GetComponent<Character>().occupiedTile.GetComponent<TileBehavior>().myUnit = null;
            unit.GetComponent<Character>().occupiedTile = gameObject;
        }
        unit.GetComponent<Character>().SetAnimVar();
        myUnit = unit;
        myUnit.transform.position = transform.position - new Vector3(0, 0, 0);
        myUnit.GetComponent<Character>().RecalculateDepth();
        myUnit.GetComponent<Character>().SetOccupiedTile(gameObject);
    }

    public bool HasUnit() {
        return myUnit != null;
    }

    public GameObject GetUnit() {
        return myUnit;
    }

    public void ClearUnit() {
        myUnit = null;
    }
    #endregion

    #region Variable Functions
    public static string GetSelectionState() {
        return selectionState;
    }

    public static void SetSelectionState(string s) {
        selectionState = s;
    }

    public int GetXPosition() {
        return xPosition;
    }

    public int GetYPosition() {
        return yPosition;
    }

    public void SetSelectedTile(GameObject unit) {
        selectedTile = unit;
    }
    #endregion

    #region Highlight Functions
    public void HighlightCanMove() {
        tileHighlighterAnimator.SetBool("canAttack", false);
        tileHighlighterAnimator.SetBool("canMove", true);
        tileHighlighterAnimator.SetBool("selected", false);
        tileHighlighterAnimator.SetBool("canExecute", false);
        tileHighlighterAnimator.SetBool("canSpawn", false);

        setHighlightOpacity(playerOpacity);
        highlighted = true;
    }

    public void HighlightCanMoveCovered() {
        tileHighlighterAnimator.SetBool("canAttack", false);
        tileHighlighterAnimator.SetBool("canMove", true);
        tileHighlighterAnimator.SetBool("selected", false);
        tileHighlighterAnimator.SetBool("canExecute", false);
        tileHighlighterAnimator.SetBool("canSpawn", false);

        setHighlightOpacity(playerOpacity / 2f);
        highlighted = true;
    }

    public void HighlightCanAttack() {
        tileHighlighterAnimator.SetBool("canAttack", true);
        tileHighlighterAnimator.SetBool("canMove", false);
        tileHighlighterAnimator.SetBool("selected", false);
        tileHighlighterAnimator.SetBool("canExecute", false);
        tileHighlighterAnimator.SetBool("canSpawn", false);
        highlighted = true;

        setHighlightOpacity(playerOpacity + 0.1f);
        
    }

    public void HighlightCanAttackEmpty() {
        tileHighlighterAnimator.SetBool("canAttack", true);
        tileHighlighterAnimator.SetBool("canMove", false);
        tileHighlighterAnimator.SetBool("selected", false);
        tileHighlighterAnimator.SetBool("canExecute", false);
        tileHighlighterAnimator.SetBool("canSpawn", false);
        setHighlightOpacity(playerOpacity / 2f);
        highlighted = true;

        setHighlightOpacity(playerOpacity / 2f);
    }

    public void HighlightCanExecute() {
        tileHighlighterAnimator.SetBool("canExecute", true);

        setHighlightOpacity(playerOpacity);
        highlighted = true;
    }

    public void HighlightCanSpawn() {
        tileHighlighterAnimator.SetBool("canSpawn", true);
        highlighted = true;

        setHighlightOpacity(playerOpacity);
    }

    public void HighlightSelected() {
        tileHighlighterAnimator.SetBool("canAttack", false);
        tileHighlighterAnimator.SetBool("canMove", false);
        tileHighlighterAnimator.SetBool("selected", true);
        tileHighlighterAnimator.SetBool("canExecute", false);
        tileHighlighterAnimator.SetBool("canSpawn", false);
        setHighlightOpacity(playerOpacity);
    }

    public void Dehighlight() {
        tileHighlighterAnimator.SetBool("canAttack", false);
        tileHighlighterAnimator.SetBool("canMove", false);
        tileHighlighterAnimator.SetBool("selected", false);
        tileHighlighterAnimator.SetBool("canExecute", false);
        tileHighlighterAnimator.SetBool("canSpawn", false);
        highlighted = false;
        setHighlightOpacity(playerOpacity);
    }
    #endregion

    #region Highlight Valid Tiles Functions

    void HighlightAttackableTiles(GameObject unit) {
        if (unit.GetComponent<Character>().unitName == "Amubish") {
            List<GameObject> units;
            GameManager gameManager = GameManager.GetSingleton();
            if (GameManager.currentPlayer == 1) {
                units = gameManager.player2Units;
            }
            else {
                units = gameManager.player1Units;
            }
            foreach (GameObject enemy in units) {
                Character enemyCharacter = enemy.GetComponent<Character>();
                float hpPercent = (float) enemyCharacter.currentHealth / (float) enemyCharacter.totalHealth;
                if (hpPercent <= .4f) {
                    highlightedTiles.Add(enemyCharacter.occupiedTile);
                    executableTiles.Add(enemyCharacter.occupiedTile);
                    enemyCharacter.occupiedTile.GetComponent<TileBehavior>().HighlightCanExecute();
                }
            }
        }

        List<int[,]> attackRanges = unit.GetComponent<Character>().GetAttackRange();
        float tileSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        foreach (int[,] attackRange in attackRanges) {
            for (int i = 0; i < attackRange.GetLength(0); i++) {
                Vector3 xOffSet = new Vector3(tileSize, 0.0f, 0.0f) * attackRange[i, 0];
                Vector3 yOffSet = new Vector3(0.0f, tileSize, 0.0f) * attackRange[i, 1];
                Vector2 tilePosition = transform.position + xOffSet + yOffSet;
                Collider2D hit = Physics2D.OverlapPoint(tilePosition);

                // If there exists a tile in that direction...
                if (hit != null) {
                    // Highlight that tile.
                    highlightedTiles.Add(hit.gameObject);

                    // If that tile has a unit...
                    if (hit.gameObject.GetComponent<TileBehavior>().HasUnit()) {
                        // And the unit belongs to the enemy team...
                        GameObject hitUnit = hit.gameObject.GetComponent<TileBehavior>().GetUnit();
                        if (hitUnit.GetComponent<Character>().GetPlayer() != selectedUnit.GetComponent<Character>().GetPlayer()) {
                            // Stop. Go no further in this direction.
                            hit.gameObject.GetComponent<TileBehavior>().HighlightCanAttack();
                            break;

                        }
                        // And the unit belongs to the player team...
                        else {
                            // Keep going.
                            hit.gameObject.GetComponent<TileBehavior>().HighlightCanAttackEmpty();
                        }
                    }
                    // If that tile is a wall...
                    else {
                        // Keep going.
                        hit.gameObject.GetComponent<TileBehavior>().HighlightCanAttackEmpty();
                    }
                }
            }
        }
    }

    void HighlightRange(int moveEnergy, int maxRange) {
        if (selectedUnit.GetComponent<Character>().unitName == "Amubish") {
            List<GameObject> units;
            GameManager gameManager = GameManager.GetSingleton();
            if (GameManager.currentPlayer == 1) {
                units = gameManager.player2Units;
            }
            else {
                units = gameManager.player1Units;
            }
            foreach (GameObject enemy in units) {
                Character enemyCharacter = enemy.GetComponent<Character>();
                float hpPercent = (float) enemyCharacter.currentHealth / (float) enemyCharacter.totalHealth;
                if (hpPercent <= .4f) {
                    highlightedTiles.Add(enemyCharacter.occupiedTile);
                    executableTiles.Add(enemyCharacter.occupiedTile);
                    enemyCharacter.occupiedTile.GetComponent<TileBehavior>().HighlightCanExecute();
                }
            }
        }

        HighlightRangeMovement(moveEnergy);

        List<int[,]> attackRanges = new List<int[,]>();
        if (selectedUnit.GetComponent<Character>().unitName == "Enforcer") {
            Character enforcer = selectedUnit.GetComponent<Character>();
            for (int x = -enforcer.maxrange; x <= enforcer.maxrange; x++) {
                for (int y = -enforcer.maxrange; y <= enforcer.maxrange; y++) {
                    if (Mathf.Abs(x) + Mathf.Abs(y) >= enforcer.minrange && Mathf.Abs(x) + Mathf.Abs(y) <= enforcer.maxrange) {
                        int[,] inRange = { { x, y } };
                        attackRanges.Add(inRange);
                    }
                }
            }
        }

        foreach (GameObject tileObject in moveableTiles) {
            TileBehavior tile = tileObject.GetComponent<TileBehavior>();
            if (selectedUnit.GetComponent<Character>().unitName == "Enforcer") {
                HighlightEnforcerAttack(selectedUnit.GetComponent<Character>(), tile, attackRanges);
            }
            else {
                tile.HighlightRangeAttack(selectedUnit.GetComponent<Character>(), tile, maxRange);
            }
        }
    }

    void HighlightRangeMovement(int moveEnergy) {
        // Don't do anything if you've run out of energy.
        if (moveEnergy < 0 || tileType == "nexus") {
            return;
        }

        //Otherwise, hightlight yourself...
        if (myUnit == null || myUnit.Equals(selectedUnit)) {
            HighlightCanMove();
            moveableTiles.Add(gameObject);
        }
        else {
            HighlightCanMoveCovered();
        }
        highlightedTiles.Add(gameObject);

        //...and all adjacent tiles (if they don't contain enemy units).

        Vector2[] directions = { Vector2.right, Vector2.left, Vector2.up, Vector2.down };
        foreach (Vector2 direction in directions) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f);
            if (hit.collider != null) {
                TileBehavior otherTile = hit.transform.GetComponent<TileBehavior>();
                int cent = 0;
                List<GameObject> adjacentlist = getadjacent(otherTile);
                foreach (GameObject unit in adjacentlist) {
                    if (unit.GetComponent<Character>().unitName == "Centurion" && selectedUnit.GetComponent<Character>().faction != "Mech") {
                        cent = 1;
                    }
                }
                int extra = 0;
                if (otherTile.myUnit == null || otherTile.myUnit.GetComponent<Character>().player == selectedUnit.GetComponent<Character>().player) {
                    if (otherTile.tileType == "sand") {
                        extra = (int)Math.Ceiling((decimal)selectedUnit.GetComponent<Character>().cost / 3) - 1;
                        if (moveEnergy > 0 && moveEnergy - otherTile.movementCost - extra - cent < 0) {
                            otherTile.HighlightRangeMovement(0);
                        }
                        else {
                            otherTile.HighlightRangeMovement(moveEnergy - otherTile.movementCost - extra - cent);
                        }
                    }
                    else {
                        otherTile.HighlightRangeMovement(moveEnergy - otherTile.movementCost - cent);
                    }
                }
            }
        }
    }

    void HighlightEnforcerAttack(Character unit, TileBehavior originalTile, List<int[,]> attackRanges) {
        float tileSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        foreach (int[,] attackRange in attackRanges) {
            for (int i = 0; i < attackRange.GetLength(0); i++) {
                Vector3 xOffSet = new Vector3(tileSize, 0.0f, 0.0f) * attackRange[i, 0];
                Vector3 yOffSet = new Vector3(0.0f, tileSize, 0.0f) * attackRange[i, 1];
                Vector2 tilePosition = originalTile.transform.position + xOffSet + yOffSet;
                Collider2D hit = Physics2D.OverlapPoint(tilePosition);

                // If there exists a tile in that direction...
                if (hit != null) {
                    // Highlight that tile.
                    highlightedTiles.Add(hit.gameObject);

                    // If that tile has a unit...
                    if (hit.gameObject.GetComponent<TileBehavior>().HasUnit()) {
                        // And the unit belongs to the enemy team...
                        GameObject hitUnit = hit.gameObject.GetComponent<TileBehavior>().GetUnit();
                        if (hitUnit.GetComponent<Character>().GetPlayer() != selectedUnit.GetComponent<Character>().GetPlayer()) {
                            hit.gameObject.GetComponent<TileBehavior>().HighlightCanAttack();
                        }
                        // And the unit belongs to the player team...
                        else {
                            // Keep going.
                            hit.gameObject.GetComponent<TileBehavior>().HighlightCanAttackEmpty();
                        }
                    }
                    else {
                        // Keep going.
                        hit.gameObject.GetComponent<TileBehavior>().HighlightCanAttackEmpty();
                    }
                }
            }
        }
    }

    void HighlightRangeAttack(Character unit, TileBehavior originalTile, int attackRange) {
        if (attackRange < 0) {
            return;
        }

        int dist = Mathf.Abs(xPosition - originalTile.xPosition) + Mathf.Abs(yPosition - originalTile.yPosition);
        if (dist >= unit.minrange && dist <= unit.maxrange) {
            if (!highlighted) {
                if (myUnit != null && myUnit.GetComponent<Character>().GetPlayer() != selectedUnit.GetComponent<Character>().GetPlayer()) {
                    HighlightCanAttack();
                }
                else {
                    HighlightCanAttackEmpty();
                }
                highlightedTiles.Add(gameObject);
            }
        }

        Vector2[] directions = { Vector2.right, Vector2.left, Vector2.up, Vector2.down };
        foreach (Vector2 direction in directions) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f);
            if (hit.collider != null) {
                TileBehavior otherTile = hit.transform.GetComponent<TileBehavior>();
                hit.transform.GetComponent<TileBehavior>().HighlightRangeAttack(unit, originalTile, attackRange - 1);
            }
        }
    }

    void HighlightSummonableTiles() {
        if (highlightedTiles.Contains(gameObject)) {
            return;
        }
        if (tileType != "nexus" && myUnit == null) {
            //Change to something else once we have the code/art
            HighlightCanSpawn();
        }
        highlightedTiles.Add(gameObject);
        Vector2[] directions = { Vector2.right, Vector2.left, Vector2.up, Vector2.down };
        foreach (Vector2 direction in directions) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f);
            if (hit.collider != null) {
                TileBehavior otherTile = hit.transform.GetComponent<TileBehavior>();
                if (otherTile.tileType == "nexus" || tileType == "nexus") {
                    hit.transform.GetComponent<TileBehavior>().HighlightSummonableTiles();
                }
            }
        }
    }
    #endregion

    #region Selection Functions
    public virtual void OnPointerClick(PointerEventData data) {
        //Condition where pointer click fails
        if (GameManager.actionInProcess || GameManager.menuOpened) {
            return;
        }
        // If nothing is currently selected...
        if (selectionState == null) {
            // and if it was a right click...
            if (data.button == PointerEventData.InputButton.Right && GameManager.infoOpened == false) {
                GameManager.GetSingleton().ShowInfoUI(myUnit, this);
            }
            // and if the tile is your Nexus
            else if (tileType == "nexus" && playerside == GameManager.currentPlayer) {
                SelectionStateToSummon();
            }
            // else if this tile has a unit on it...
            else if (myUnit != null) {
                // Right click for Unit Info (eg. stats)
                //if (data.button == PointerEventData.InputButton.Right)
                //{
                //    GameManager.GetSingleton().ShowCharacterUI(myUnit);
                //}
                // and the unit's player is equal to to the current player...
                if (GameManager.currentPlayer.Equals(myUnit.GetComponent<Character>().GetPlayer()) && myUnit.GetComponent<Character>().GetCanMove() == true) {
                    // select that unit/tile and highlight the tiles that the unit can move to (if it can move).
                    SelectionStateToMove();
                }
            }

            // and this tile does not have a unit on it...
            else {
                GameManager.GetSingleton().ShowLeftMenu();
                // do nothing.
            }
        }
        // If something is currently selected...
        else {
            // and selection state is move...
            if (selectionState.Equals("move")) {
                // and if it was a right click...
                if (data.button == PointerEventData.InputButton.Right) {
                    SelectionStateToNull();
                    return;
                }
                // and the selected character can move onto this tile...
                if (highlighted && myUnit == null && moveableTiles.Contains(gameObject)) {
                    // move that character onto this tile and dehighlight everything.
                    selectedTile.GetComponent<TileBehavior>().ClearUnit();
                    StartCoroutine(MoveUnitToThisTile(selectedUnit, selectedTile));
                }

                // and you are the selectedTile...
                else if (selectedTile.Equals(gameObject)) {
                    // and the selected unit can attack...
                    if (selectedUnit.GetComponent<Character>().GetCanAttack() == true) {
                        SelectionStateToAttack();
                    }
                    // and the selected unit can't attack...
                    else {
                        SelectionStateToNull();
                    }
                }
                // and the selected character cannot move onto this tile...
                else {
                    // Dehighlight everything.
                    SelectionStateToNull();
                }
            }
            // and selection state is attack...
            else if (selectionState.Equals("attack")) {
                // and the selected character can attack there...
                if (data.button == PointerEventData.InputButton.Right) {
                    selectedUnit.GetComponent<Character>().previousTile.GetComponent<TileBehavior>().PlaceUnit(selectedUnit);
                    selectedUnit.GetComponent<Character>().SetCanMove(true);
                    SelectionStateToNull();
                    return;
                }
                if (highlighted && myUnit != null && myUnit.GetComponent<Character>().GetPlayer() != selectedUnit.GetComponent<Character>().GetPlayer()) {
                    // (Attack), and deselect everything.

                    //ADD CODE FOR ATTACK
                    if (myUnit.GetComponent<Character>().faction != selectedUnit.GetComponent<Character>().faction) {
                        selectedUnit.GetComponent<Character>().attack(myUnit);
                    }

                    if (selectedUnit.GetComponent<Character>().occupiedTile.GetComponent<TileBehavior>().tileType == "capturepoint") {
                        selectedUnit.GetComponent<Character>().hasAttacked = true;
                    }
                    selectedUnit.GetComponent<Character>().SetCanMove(false);
                    selectedUnit.GetComponent<Character>().SetCanAttack(false);
                    SelectionStateToNull();

                }
                // and if the tile is the enemy's Nexus
                else if (highlighted && tileType == "nexus" && playerside != GameManager.currentPlayer)
                {
                    GetComponent<Nexus>().Damaged();
                    selectedUnit.GetComponent<Character>().SetCanMove(false);
                    selectedUnit.GetComponent<Character>().SetCanAttack(false);
                    SelectionStateToNull();
                }
                // and you are the selectedTile...
                else if (selectedTile.Equals(gameObject)) {
                    if (tileType == "capturepoint") {
                        gameObject.GetComponent<CapturePoint>().Capture();
                    }
                    selectedUnit.GetComponent<Character>().SetCanMove(false);
                    selectedUnit.GetComponent<Character>().SetCanAttack(false);
                    SelectionStateToNull();
                }
            }
            // and selection state is summoning...
            else if (selectionState == "summoning") {
                GameManager gameManager = GameManager.GetSingleton();
                // and ifit was a right click...
                if (data.button == PointerEventData.InputButton.Right) {
                    gameManager.ExitSummonPanel();
                    SelectionStateToNull();
                }
                // and if it was a left click...
                else if (highlighted && tileType != "nexus") {
                    gameManager.PlaceCharacterOnTile(GameManager.GetSingleton().boughtUnit, xPosition, yPosition, GameManager.currentPlayer);
                    gameManager.SubtractCost();
                    // Add sound
                    // Play random step sound
                    System.Random r = new System.Random();
                    int warpNum = r.Next(0, warpSounds.Length);
                    audioSource.clip = warpSounds[warpNum];
                    audioSource.Play();
                    SelectionStateToNull();
                }
            }
        }
    }

    public virtual void OnPointerDown(PointerEventData data) {

        if (myUnit != null && data.button == PointerEventData.InputButton.Left) {
            if (selectionState == null) {
                if (!GameManager.currentPlayer.Equals(myUnit.GetComponent<Character>().GetPlayer()) || (!myUnit.GetComponent<Character>().GetCanMove() && !myUnit.GetComponent<Character>().GetCanAttack())) {
                    //Highlight
                    SelectionStateToCheckRange();
                }
            }
        }
    }

    public virtual void OnPointerUp(PointerEventData data) {
        // Needs to be changed
        if (selectionState == "checkRange") {
            Unhighlight();
            selectionState = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (myUnit != null && !GameManager.menuOpened) {
            GameManager.GetSingleton().ShowCharacterUI(myUnit);
        }
        else {
            GameManager.GetSingleton().unitUI.SetActive(false);
        }
    }
    #endregion

    #region Selection State To Functions
    public void SelectionStateToNull() {
        // Deselect everything else
        Deselect();
    }

    public void SelectionStateToSummon() {
        // Deselect everything else
        Deselect();

        // Switch selection state to summon
        selectionState = "summoning";

        // Select this tile
        selectedTile = gameObject;
        HighlightSummonableTiles();
    }

    public void SelectionStateToMove() {
        // Deselect everything else
        Deselect();

        // Switch selection state to move
        selectionState = "move";

        // Select this tile and its unit
        selectedUnit = myUnit;
        selectedTile = gameObject;
        HighlightSelected();
        selectedUnit.GetComponent<Character>().previousTile = gameObject;

        // Open the Character UI
        GameManager.GetSingleton().ShowCharacterUI(selectedUnit);

        // Highlight moveable tiles
        if (selectedUnit.GetComponent<Character>().GetCanMove()) {
            //Highlight range
            HighlightRange(myUnit.GetComponent<Character>().movement, myUnit.GetComponent<Character>().maxrange);
        }
    }

    public void SelectionStateToAttack() {
        // Deselect everything else
        Deselect();

        // Switch selection state to move
        selectionState = "attack";

        // Select this tile and its unit
        selectedUnit = myUnit;
        selectedTile = gameObject;
        HighlightSelected();

        // Open the Character UI
        GameManager.GetSingleton().ShowCharacterUI(selectedUnit);

        //Highlight attackable tiles
        selectedTile.transform.GetComponent<TileBehavior>().HighlightAttackableTiles(selectedUnit);
    }

    public void SelectionStateToCheckRange() {
        // Deselect everything else
        Deselect();

        // Switch selection state to move
        selectionState = "checkRange";

        // Select this tile and its unit
        selectedUnit = myUnit;
        selectedTile = gameObject;
        HighlightCanMove();

        //Highlight range
        HighlightRange(myUnit.GetComponent<Character>().movement, myUnit.GetComponent<Character>().maxrange);
    }
    #endregion

    #region Deselect
    public static void Deselect() {
        Unhighlight();

        // Reset all selection variables
        selectedUnit = null;
        if (selectedTile != null) {
            selectedTile.GetComponent<TileBehavior>().Dehighlight();
        }
        selectedTile = null;
        selectionState = null;

        //Get rid of all the UI
        GameManager.GetSingleton().ClearUI();
    }

    public static void Unhighlight() {
        // Dehighlight everything
        foreach (GameObject highlightedTile in highlightedTiles) {
            highlightedTile.transform.GetComponent<TileBehavior>().Dehighlight();
        }
        // Clear the list of highlighted tiles
        highlightedTiles.Clear();
        // Clear the list of moveable tiles
        moveableTiles.Clear();
    }
    #endregion

    #region Movement Functions
    IEnumerator MoveUnitToThisTile(GameObject unit, GameObject originalTile) {
        // Action in process!
        GameManager.actionInProcess = true;

        // Deselect everything
        Deselect();
        float tileSize = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        tileDim = tileSize;

        // Calculate the steps you need to take
        int unitPlayer = unit.GetComponent<Character>().player;
        List<string> movementSteps = CalculateMovement(new List<string>(), originalTile, gameObject, unit.GetComponent<Character>().GetMovement(), unitPlayer);

        //Take those steps!
        foreach (string step in movementSteps) {
            if (step.Equals("up")) {
                unit.transform.position += new Vector3(0, tileSize);
            }
            else if (step.Equals("right")) {
                unit.transform.position += new Vector3(tileSize, 0);
            }
            else if (step.Equals("down")) {
                unit.transform.position -= new Vector3(0, tileSize);
            }
            else if (step.Equals("left")) {
                unit.transform.position -= new Vector3(tileSize, 0);
            }
            unit.GetComponent<Character>().RecalculateDepth();
            unit.GetComponent<Character>().StartBounceAnimation();
            yield return new WaitForSeconds(stepDuration);
        }
        PlaceUnit(unit);
        unit.GetComponent<Character>().SetCanMove(false);
        int total = 0;
        total += Mathf.Abs(xPosition - originalTile.GetComponent<TileBehavior>().xPosition) + Mathf.Abs(yPosition - originalTile.GetComponent<TileBehavior>().yPosition);
        unit.GetComponent<TestClass>().distmoved = total;

        // Action over!
        GameManager.actionInProcess = false;
        unit.GetComponent<Character>().TileToXY(GetComponent<TileBehavior>());
        gameObject.GetComponent<TileBehavior>().SelectionStateToAttack();
        
    }

    // Recursive helper function to calculate the steps to take to get from tile A to tile B
    public static List<string> CalculateMovement(List<string> movement, GameObject currentTile, GameObject goalTile, int moveEnergy, int unitPlayer) {
        // If you're there, return the movement path.
        if (currentTile.Equals(goalTile)) {
            return movement;
        }

        // If you're out of energy, it's an invalid path.
        if (moveEnergy < 0) {
            return null;
        }

        List<List<string>> validPaths = new List<List<string>>();
        // Check for all adjacent tiles:
        Vector2[] directions = { Vector2.right, Vector2.left, Vector2.up, Vector2.down };
        foreach (Vector2 direction in directions) {
            RaycastHit2D hit = Physics2D.Raycast(currentTile.transform.position, direction, 1.0f);
            if (hit.collider != null) {
                GameObject otherTileUnit = hit.transform.GetComponent<TileBehavior>().myUnit;
                if (otherTileUnit == null || otherTileUnit.GetComponent<Character>().player == unitPlayer) {
                    List<string> newMovement = new List<string>(movement.ToArray());
                    if (direction.Equals(Vector2.right)) {
                        newMovement.Add("right");
                    }
                    if (direction.Equals(Vector2.left)) {
                        newMovement.Add("left");
                    }
                    if (direction.Equals(Vector2.up)) {
                        newMovement.Add("up");
                    }
                    if (direction.Equals(Vector2.down)) {
                        newMovement.Add("down");
                    }
                    List<string> path = CalculateMovement(newMovement, hit.collider.gameObject, goalTile, moveEnergy - currentTile.GetComponent<TileBehavior>().movementCost, unitPlayer);
                    if (path != null) {
                        validPaths.Add(path);
                    }
                }
            }
        }

        // Return the shortest valid path
        if (validPaths.Count != 0) {
            List<string> shortestList = validPaths[0];
            foreach (List<string> path in validPaths) {
                if (path.Count < shortestList.Count) {
                    shortestList = path;
                }
            }
            return shortestList;
        }

        // If there are no valid paths from this point, return null
        return null;
    }
    #endregion

    #region Helper Functions
    public List<GameObject> getadjacent(TileBehavior tile)
    {
        List<GameObject> retunitlist = new List<GameObject>();
        int thisx = tile.GetComponent<TileBehavior>().xPosition;
        int thisy = tile.GetComponent<TileBehavior>().yPosition;
        if (thisy > 0)
        {
            int[,] up = new int[thisx, thisy - 1];
            GameObject unit = GameManager.GetSingleton().mapArray[thisx, thisy - 1].GetComponent<TileBehavior>().myUnit;
            if (unit != null)
            {
                retunitlist.Add(unit);
            }
        }
        if (thisy < 12)
        {
            int[,] down = new int[thisx, thisy + 1];
            GameObject unit = GameManager.GetSingleton().mapArray[thisx, thisy + 1].GetComponent<TileBehavior>().myUnit;
            if (unit != null)
            {
                retunitlist.Add(unit);
            }
        }
        if (thisx > 0)
        {
            int[,] left = new int[thisx - 1, thisy];
            GameObject unit = GameManager.GetSingleton().mapArray[thisx - 1, thisy].GetComponent<TileBehavior>().myUnit;
            if (unit != null)
            {
                retunitlist.Add(unit);
            }
        }
        if (thisx < 18)
        {
            int[,] right = new int[thisx + 1, thisy];
            GameObject unit = GameManager.GetSingleton().mapArray[thisx + 1, thisy].GetComponent<TileBehavior>().myUnit;
            if (unit != null)
            {
                retunitlist.Add(unit);
            }
        }
        return retunitlist;
    }
    #endregion
}
