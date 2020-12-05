using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestClass : Character
{
    //strength, speed, knowledge, will
    //private int[] Stats = { 5, 5, 2, 4 };



    void Awake() {
    }

    // Update is called once per frame
    void Update() {

    }

    public override List<GameObject> getadjacent(TileBehavior tile)
    {
        List<GameObject> retunitlist = new List<GameObject>();
        int thisx = tile.GetComponent<TileBehavior>().xPosition;
        int thisy = tile.GetComponent<TileBehavior>().yPosition;
        if (thisy > 0) {
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

    public override void TileToXY(TileBehavior tile)
    {
        positionx = tile.GetComponent<TileBehavior>().xPosition;
        positiony = tile.GetComponent<TileBehavior>().yPosition;
    }

    public override void TakeDamage(int damage, bool selfDamage = false)
    {
        bool lorge = false;
        int dmgtaken = damage;
        if(faction == "Rock")
        {
            List<GameObject> adjacentlist = getadjacent(occupiedTile.GetComponent<TileBehavior>());
            foreach (GameObject unit in adjacentlist)
            {
                if (unit.GetComponent<TestClass>().unitName == "Bouldef")
                {
                    lorge = true;
                }
            }
            if( lorge == true)
            {
                dmgtaken -= 1;
            }
        }
        if (occupiedTile.GetComponent<TileBehavior>().tileType == "barricade" && !beenAttacked) {
            dmgtaken = (int) Mathf.Floor(dmgtaken / 2);
            if (dmgtaken <= 0) {
                dmgtaken = 1;
            }
        }
        beenAttacked = true;
        currentHealth -= dmgtaken;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = currentHealth.ToString();
        if (currentHealth > 0) {
            StartCoroutine("HurtAnimation", dmgtaken);
        }
        else {
            StartCoroutine("DeathAnimation");
            //Fix timer
            ondeathhandler(selfDamage);
        }
    }


    public override void Ability() {
        if(unitName == "Upholder")
        {
            if(currentHealth < totalHealth)
            {
                currentHealth++;
                gameObject.GetComponentInChildren<TextMeshProUGUI>().text = currentHealth.ToString();
            }
        }
    }

    public override void DisplayStats() {
        //open menu for character, display stats, etc.
        //TODO: can hold off for now, displays atk ranges currenthp/maxhp movement
    }

    public override bool IsInRange(int targetx, int targety)
    {
        int dist = Mathf.Abs(targetx - positionx) + Mathf.Abs(targety - positiony);
        if (dist >= minrange && dist <= maxrange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override List<int[,]> GetAttackRange()
    {
        List<int[,]> retlist = new List<int[,]>();
        for(int x = -maxrange; x <= maxrange; x++) {
            for (int y = -maxrange; y <= maxrange; y++) {
                if (Mathf.Abs(x) + Mathf.Abs(y) >= minrange && Mathf.Abs(x) + Mathf.Abs(y) <= maxrange) {
                    int[,] inRange = {{x, y }};
                    retlist.Add(inRange);
                }
            }
        }
        return retlist;
    }
    public override void ondeathhandler(bool selfDamage = false) //need to wait on this one to decide how summoning is implemented etc
    {
        GameManager gameManager = GameManager.GetSingleton();
        if (currentHealth <= 0)
        {
            if(unitName == "Keresu")
            {
                List<GameObject> adjacentlist = getadjacent(occupiedTile.GetComponent<TileBehavior>());
                foreach (GameObject unit in adjacentlist)
                {
                    if (unit.GetComponent<TestClass>().faction != "Shadow")
                    {
                        unit.GetComponent<TestClass>().TakeDamage(3);
                    }
                }
            }
            if(GameManager.GetSingleton().getCurrent() == 1)
            {
                if (!selfDamage) {
                    gameManager.AddObjPoints(1, cost);
                }
                gameManager.player1Units.Remove(this.gameObject);
            } else
            {
                if (!selfDamage) {
                    gameManager.AddObjPoints(2, cost);
                }
                gameManager.player2Units.Remove(this.gameObject);
            }
            gameManager.UpdateUI();
        }
    }

    public override void attack(GameObject target)
    {
        //TODO: implement this
        AttackSound();
        TileBehavior targettile = target.GetComponent<TestClass>().occupiedTile.GetComponent<TileBehavior>();
        int curdmg = damage;
        string targetname = target.GetComponent<TestClass>().unitName;
        if (unitName == "Amubish") {
            if (TileBehavior.executableTiles.Contains(target.GetComponent<Character>().occupiedTile)) {
                target.GetComponent<TestClass>().TakeDamage(99);
                TakeDamage(99, true);
                return;
            }
        }
        if (unitName == "Tyoudure")
        {
            List<GameObject> adjacentlist = getadjacent(occupiedTile.GetComponent<TileBehavior>());
            curdmg = 2;
            foreach (GameObject unit in adjacentlist)
            {
                if (unit.GetComponent<TestClass>().faction == "Shadow")
                {
                    curdmg += 1;
                    unit.GetComponent<TestClass>().TakeDamage(1);
                }
            }

            curdmg += 1;
            TakeDamage(1);
        }
        if (unitName == "Grunt")
        {
            //implement grunt adjacency checks, tbh idk how to do this yet
            List<GameObject> adjacentlist = getadjacent(targettile);
            curdmg = 2;
            foreach (GameObject unit in adjacentlist)
            {
                if (unit.GetComponent<TestClass>().faction == "Insect" && unit != gameObject)
                {
                    curdmg += 1;
                }
            }
        }
        if (unitName == "Geowulf")
        {
            List<GameObject> adjacentlist = getadjacent(targettile);
            foreach (GameObject unit in adjacentlist)
            {
                if (unit != gameObject)
                {
                    unit.GetComponent<TestClass>().TakeDamage(1);
                }
            }
        }
        if (unitName == "Bouldef")
        {
            int dist = 0;
            dist += Mathf.Abs(positionx - target.GetComponent<TestClass>().positionx);
            dist += Mathf.Abs(positiony - target.GetComponent<TestClass>().positiony);
            if (dist > 1)
            {
                curdmg -= 1;
            }
        }
        if (unitName == "Enforcer")
        {
            int dist = 0;
            dist += Mathf.Abs(positionx - target.GetComponent<TestClass>().positionx);
            dist += Mathf.Abs(positiony - target.GetComponent<TestClass>().positiony);
            curdmg = dist + 1;
            if (canMove == false)
            {
                curdmg = (int) Mathf.Floor(curdmg / 2);
            }
        }
        if (unitName == "Grasshopper")
        {
            if (distmoved == 4 && canMove == false)
            {
                curdmg = 5;
            }
        }
        if (unitName == "Beetle")
        {
            if (targettile.xPosition > positionx)
            {
                int place = positionx - 1;
                if (place >= 0 && GameManager.GetSingleton().mapArray[place, positiony].GetComponent<TileBehavior>().myUnit == null)
                {
                    GameManager.GetSingleton().mapArray[place, positiony].GetComponent<TileBehavior>().PlaceUnit(target);
                    target.GetComponent<Character>().TileToXY(targettile);
                    targettile.ClearUnit();
                }
            }
            else if (targettile.xPosition < positionx)
            {
                int place = positionx + 1;
                
                if (place < GameManager.GetSingleton().mapXSize && GameManager.GetSingleton().mapArray[place, positiony].GetComponent<TileBehavior>().myUnit == null)
                {
                    GameManager.GetSingleton().mapArray[place, positiony].GetComponent<TileBehavior>().PlaceUnit(target);
                    target.GetComponent<Character>().TileToXY(targettile);
                    targettile.ClearUnit();
                }
            }
            else if (targettile.yPosition > positiony)
            {
                int place = positiony - 1;
                if (place >= 0 && GameManager.GetSingleton().mapArray[positionx, place].GetComponent<TileBehavior>().myUnit == null)
                {
                    GameManager.GetSingleton().mapArray[positionx, place].GetComponent<TileBehavior>().PlaceUnit(target);
                    target.GetComponent<Character>().TileToXY(targettile);
                    targettile.ClearUnit();
                }
            }
            else if (targettile.yPosition < positiony)
            {
                int place = positiony + 1;
                if (place < GameManager.GetSingleton().mapYSize && GameManager.GetSingleton().mapArray[place, positiony].GetComponent<TileBehavior>().myUnit == null)
                {
                    GameManager.GetSingleton().mapArray[positionx, place].GetComponent<TileBehavior>().PlaceUnit(target);
                    target.GetComponent<Character>().TileToXY(targettile);
                    targettile.ClearUnit();
                }
            }
            //check if square opposite of beetle is open
            //set position of the target to the spot behind beetle
        }
        if (targetname == "Stalmight") {
            TakeDamage(2);
        }
        target.GetComponent<TestClass>().TakeDamage(curdmg);

    }
    /*public override List<int[,]> GetAttackRange() {
        int flipIfPlayer2 = 1;
        if (player == 2) {
            flipIfPlayer2 = -1;
        }

        List<int[,]> attackRanges = new List<int[,]>();

        int[,] forwardRange = {
            {1 * flipIfPlayer2, 0 },
        };

        int[,] aboveRange = {
            {0, 1},
        };

        int[,] belowRange = {
            {0, -1},
        };

        attackRanges.Add(forwardRange);
        attackRanges.Add(aboveRange);
        attackRanges.Add(belowRange);

        return attackRanges;
    }*/
}
