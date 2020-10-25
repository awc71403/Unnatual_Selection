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
        movement = 1;
        maxrange = 1;
        minrange = 1;
        cost = 1;
        initialmovement = 1;
        //curStatArr = Stats;
        cName = "Test";
    }

    // Update is called once per frame
    void Update() {

    }

    public override void TileToXY(TileBehavior tile)
    {
        positionx = tile.GetComponent<TileBehavior>().xPosition;
        positiony = tile.GetComponent<TileBehavior>().yPosition;
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }


    public override void Ability() {
        //TODO: wait on more info for now.
        throw new System.NotImplementedException();
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
        foreach(GameObject tile in GameObject.Find("GameManager").GetComponent<GameManager>().mapArray)
        {
            int tilex = tile.GetComponent<TileBehavior>().xPosition;
            int tiley = tile.GetComponent<TileBehavior>().yPosition;
            if (IsInRange(tilex, tiley))
            {
                retlist.Add(new int[tilex, tiley]);
            }
        }
        return retlist;
    }
    public override void ondeathhandler() //need to wait on this one to decide how summoning is implemented etc
    {
        if(currentHealth <= 0)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().player1Units.Remove(this.gameObject);
        }
    }

    public override void attack(GameObject target)
    {
        //TODO: implement this
        target.GetComponent<TestClass>().TakeDamage(damage);
        if(cName == "Grunt")
        {
            //implement grunt adjacency checks, tbh idk how to do this yet
        } else if (cName == "Beetle")
        {
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
