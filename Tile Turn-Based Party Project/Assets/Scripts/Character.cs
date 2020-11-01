using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour {

    //public int[] curStatArr;
    public int maxrange;
    public int minrange;
    public int damage;
    public int movement;
    public int initialmovement;
    protected string cName;
    public string faction;
    public int cost;
    public int totalHealth;
    public int currentHealth;
    public int positionx;
    public int positiony;
    public int distmoved;

    public int player;

    public bool canMove = true;
    private bool canAttack = true;

    public GameObject occupiedTile;

    // Sprite Rendering
    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    public abstract void TakeDamage(int damage);
    public abstract void Ability();
    public abstract List<GameObject> getadjacent(TileBehavior tile);
    public abstract void DisplayStats();
    public abstract List<int[,]> GetAttackRange();
    public abstract void TileToXY(TileBehavior tile);
    public abstract bool IsInRange(int targetx, int targety);
    //public abstract bool IsInMoveRange(int targetx, int targety);
    public abstract void ondeathhandler();
    public abstract void attack(GameObject target);
    //public abstract void move(int targetx, int targety);

    // Movement Bounce Animation
    float totalStretch = 0.3f;
    float totalSquish = 0.3f;

    #region Initialization
    void Start() {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        SetHPFull();
    }
    #endregion

    #region Getter and Setter
    public string GetName() {
        return cName;
    }

    public int GetHP() {
        return currentHealth;
    }

    public void SetHPFull() {
        currentHealth = totalHealth;
    }

    //public int[] GetCopyStats() {
     //   int[] copy = new int[curStatArr.Length];
    //    for (int i = 0; i < curStatArr.Length; i++) {
   //         copy[i] = curStatArr[i];
    //    }
     //   return copy;
   // }

    /*public void ModifyStats(int[] stats) {
        if (stats.Length == curStatArr.Length) {
            for (int i = 0; i < stats.Length; i++) {
                curStatArr[i] = stats[i];
            }
        }
    }*/

    public virtual int GetAtk() {
        return damage;
    }

    public int GetMovement() {
        return movement;
    }

    public bool GetCanMove() {
        return canMove;
    }

    public void SetCanMove(bool canMoveBool) {
        canMove = canMoveBool;
    }

    public bool GetCanAttack() {
        return canAttack;
    }

    public void SetCanAttack(bool canAttackBool) {
        canAttack = canAttackBool;

        if (canAttackBool == true) {
            GetComponent<Renderer>().material.color = Color.white;
        }
        else if (canAttackBool == false) {
            GetComponent<Renderer>().material.color = Color.grey;
        }
    }

    public void SetMovement(int value)
    {
        movement = value;
    }

    public void SetPlayer(int playerNumber) {
        player = playerNumber;
        //anim.SetInteger("player", playerNumber);
    }

    public int GetPlayer() {
        return player;
    }

    public void RecalculateDepth() {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    public GameObject GetOccupiedTile() {
        return occupiedTile;
    }

    public void SetOccupiedTile(GameObject tile) {
        occupiedTile = tile;
    }
    #endregion

    #region Sprite
    void WhiteSprite() {
        myRenderer.material.shader = shaderGUItext;
        myRenderer.color = Color.white;
    }

    void NormalSprite() {
        myRenderer.material.shader = shaderSpritesDefault;
        myRenderer.color = Color.white;
    }
    #endregion

    #region Animation
    IEnumerator HurtAnimation(int damage) {
        // Go white
        WhiteSprite();

        yield return new WaitForSeconds(0.1f);

        // Go normal
        NormalSprite();
    }

    IEnumerator DeathAnimation() {
        // loop over 0.5 second backwards
        print("death time");
        for (float i = 0.25f; i >= 0; i -= Time.deltaTime) {
            // set color with i as alpha
            myRenderer.color = new Color(1, 1, 1, i);
            transform.localScale = new Vector3(1.5f - i, 1.5f - i, 1);
            yield return null;
        }

        myRenderer.color = new Color(1, 1, 1, 1);
        transform.localScale = new Vector3(1, 1, 1);
        gameObject.SetActive(false);
    }

    public void StartBounceAnimation() {
        StartCoroutine("BounceAnimation");
    }

    IEnumerator BounceAnimation() {
        int frames = 3;
        //Vector3 originalPosition = transform.position;
        float stretch = totalStretch;
        float squish = totalSquish;
        for (int i = frames; i > 0; i--) {
            transform.localScale = new Vector3(1 + stretch, 1 - squish, 1);
            yield return new WaitForSeconds(0.01f);
            stretch /= 2.5f;
            squish /= 2.5f;
        }
        transform.localScale = new Vector3(1, 1, 1);

        // Add sound
    }
    #endregion

    public void SetAnimVar() {
        if (player == 1) {
            //anim.SetInteger("player", 1);
        }
        else if (player == 2) {
            //anim.SetInteger("player", 2);
        }
    }

    #region Stats
    public void ResetStats() {
        currentHealth = totalHealth;
    }

    public void HPDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth > 0) {
            StartCoroutine("HurtAnimation", damage);
        }
        else {
            StartCoroutine("DeathAnimation");
        }
    }
    #endregion
}
