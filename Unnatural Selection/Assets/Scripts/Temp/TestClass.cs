using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : Character
{
    //strength, speed, knowledge, will
    //private int[] Stats = { 5, 5, 2, 4 };



    void Awake() {
        totalHealth = 35;
        currentHealth = totalHealth;
        damage = 1; //placeholder replace for every unit type
        movement = 2;
        maxrange = 2;
        minrange = 1;
        cost = 1;
        initialmovement = 1;
        //curStatArr = Stats;
        unitName = "Test";
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

    public override void TakeDamage(int damage)
    {
        bool lorge = false;
        int dmgtaken = damage;
        if(faction == "rock")
        {
            List<GameObject> adjacentlist = getadjacent(occupiedTile.GetComponent<TileBehavior>());
            foreach (GameObject unit in adjacentlist)
            {
                if (unit.GetComponent<TestClass>().cName == "Bouldef")
                {
                    lorge = true;
                }
            }
            if( lorge == true)
            {
                dmgtaken -= 1;
            }
        }
        currentHealth -= dmgtaken;
        if(currentHealth <= 0)
        {
            ondeathhandler();
        }
    }


    public override void Ability() {
        if( cName == "Upholder")
        {
            if(currentHealth < totalHealth)
            {
                currentHealth++;
            }
        }
    }

    public override void DisplayStats() {
        //open menu for character, display stats, etc.
        //TODO: can hold off for now, displays atk ranges currenthp/maxhp movement
    }

    public override bool IsInRange(int targetx, int targety)
    {
        int dist = 0;
        dist += Mathf.Abs(targetx - positionx) + Mathf.Abs(targety - positiony);
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
    public override void ondeathhandler() //need to wait on this one to decide how summoning is implemented etc
    {
        if(currentHealth <= 0)
        {
            if(GameManager.GetSingleton().getCurrent() == 1)
            {
                GameManager.GetSingleton().player2ObjectivePoints += cost;
            } else
            {
                GameManager.GetSingleton().player1ObjectivePoints += cost;
            }
            GameManager.GetSingleton().player1Units.Remove(this.gameObject);
        }
    }

    public override void attack(GameObject target)
    {
        //TODO: implement this
        TileBehavior targettile = target.GetComponent<TestClass>().occupiedTile.GetComponent<TileBehavior>();
        int curdmg = damage;
        string targetname = target.GetComponent<TestClass>().cName;
        if (cName == "Grunt")
        {
            //implement grunt adjacency checks, tbh idk how to do this yet
            List<GameObject> adjacentlist = getadjacent(targettile);
            curdmg = -1;
            foreach (GameObject unit in adjacentlist)
            {
                if (unit.GetComponent<TestClass>().faction == "insect")
                {
                    curdmg += 1;
                }
            }
        }
        if (cName == "Geowulf")
        {
            List<GameObject> adjacentlist = getadjacent(targettile);
            foreach (GameObject unit in adjacentlist)
            {
                if (unit.GetComponent<TestClass>().faction != "rock")
                {
                    unit.GetComponent<TestClass>().TakeDamage(1);
                }
            }
        }
        if (cName == "Bouldef")
        {
            int dist = 0;
            dist += Mathf.Abs(positionx - target.GetComponent<TestClass>().positionx);
            dist += Mathf.Abs(positiony - target.GetComponent<TestClass>().positiony);
            if (dist > 1)
            {
                curdmg -= 1;
            }
        }
        if (cName == "Enforcer")
        {
            int dist = 0;
            dist += Mathf.Abs(positionx - target.GetComponent<TestClass>().positionx);
            dist += Mathf.Abs(positiony - target.GetComponent<TestClass>().positiony);
            curdmg = dist + 1;
            if( canMove == false)
            {
                Mathf.Floor(curdmg / 2);
            }
        }
        if (cName == "Grasshopper")
        {
            if (distmoved == 4 && canMove == false)
            {
                curdmg = 4;
            }
        }
        target.GetComponent<TestClass>().TakeDamage(curdmg);
        if (unitName == "Beetle")
        {
            if (targettile.xPosition > positionx)
            {
                int place = positionx - 1;
                if (GameManager.GetSingleton().mapArray[place, positiony].GetComponent<TileBehavior>().myUnit == null)
                {
                    GameManager.GetSingleton().mapArray[place, positiony].GetComponent<TileBehavior>().PlaceUnit(target);
                    targettile.ClearUnit();
                }
            }
            else if (targettile.xPosition < positionx)
            {
                int place = positionx + 1;
                if (GameManager.GetSingleton().mapArray[place, positiony].GetComponent<TileBehavior>().myUnit == null)
                {
                    GameManager.GetSingleton().mapArray[place, positiony].GetComponent<TileBehavior>().PlaceUnit(target);
                    targettile.ClearUnit();
                }
            }
            else if (targettile.yPosition > positiony)
            {
                int place = positiony - 1;
                if (GameManager.GetSingleton().mapArray[positionx, place].GetComponent<TileBehavior>().myUnit == null)
                {
                    GameManager.GetSingleton().mapArray[positionx, place].GetComponent<TileBehavior>().PlaceUnit(target);
                    targettile.ClearUnit();
                }
            }
            if (targettile.yPosition < positiony)
            {
                int place = positiony + 1;
                if (GameManager.GetSingleton().mapArray[place, positiony].GetComponent<TileBehavior>().myUnit == null)
                {
                    GameManager.GetSingleton().mapArray[positionx, place].GetComponent<TileBehavior>().PlaceUnit(target);
                    targettile.ClearUnit();
                }
            }
            if (targetname == "Stalmight")
            {
                TakeDamage(2);
            }
            //check if square opposite of beetle is open
            //set position of the target to the spot behind beetle
        }
       
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
