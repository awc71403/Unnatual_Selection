using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour {

    #region Variables
    //public int[] curStatArr;
    public int maxrange;
    public int minrange;
    public int damage;
    public int movement;
    public int initialmovement;
    public int cost;
    public int totalHealth;
    public int currentHealth;
    public int positionx;
    public int positiony;
    public int distmoved;

    public string unitName;
    public string faction;
    public string desc;
    public string ability;
    public Sprite sprite;
    public Sprite factionSprite;

    public int player;

    public bool canMove = true;
    public bool canAttack = true;
    public bool hasAttacked = false;
    public bool beenAttacked = false;

    public GameObject occupiedTile;
    public GameObject previousTile;

    // Sprite Rendering
    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    #region Extra
    [SerializeField]
    private Text damageTextPrefab;
    [SerializeField]
    private AudioClip[] stepSounds;
    [SerializeField]
    private AudioClip[] attackSounds;
    private AudioSource audioSource;

    // Movement Bounce Animation
    float totalStretch = 0.3f;
    float totalSquish = 0.3f;
    #endregion
    #endregion

    #region Abstract
    public abstract void TakeDamage(int damage, bool selfDamage = false);
    public abstract void Ability();
    public abstract List<GameObject> getadjacent(TileBehavior tile);
    public abstract void DisplayStats();
    public abstract List<int[,]> GetAttackRange();
    public abstract void TileToXY(TileBehavior tile);
    public abstract bool IsInRange(int targetx, int targety);
    //public abstract bool IsInMoveRange(int targetx, int targety);
    public abstract void ondeathhandler(bool selfDamage = false);
    public abstract void attack(GameObject target);
    //public abstract void move(int targetx, int targety);
    #endregion

    #region Initialization
    void Start() {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        audioSource = GetComponent<AudioSource>();
        SetHPFull();
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = currentHealth.ToString();
    }
    #endregion

    #region Getter and Setter
    public string GetName() {
        return unitName;
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

    public virtual void SetOccupiedTile(GameObject tile) {
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

        //Create Damage Text
        Text damageText = Instantiate(damageTextPrefab, GameManager.GetSingleton().SummonPanel.canvas.transform);
        //damageText.transform.SetParent(GameObject.Find("Canvas").transform, false);
        Vector3 textPositionOffset = new Vector3(0, 1.25f, 0);
        damageText.transform.position = Camera.main.WorldToScreenPoint(transform.position + textPositionOffset);
        damageText.GetComponent<DamageText>().SetDamage(damage);

        // Shaking
        Vector3 defaultPosition = transform.position;
        System.Random r = new System.Random();
        for (int i = 0; i < 5; i++) {
            double horizontalOffset = r.NextDouble() * 0.2 - 0.1f;
            //double verticalOffset = r.NextDouble() * 0.2 - 0.1f;
            Vector3 vectorOffset = new Vector3((float)horizontalOffset, 0, 0);
            transform.position += vectorOffset;
            yield return new WaitForSeconds(0.025f);
            transform.position = defaultPosition;
        }

        // Go normal
        NormalSprite();
    }

    IEnumerator DeathAnimation() {
        // loop over 0.5 second backwards
        GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
        for (float i = 0.5f; i >= 0; i -= Time.deltaTime) {
            // set color with i as alpha
            myRenderer.color = new Color(1, 1, 1, i);
            transform.localScale = new Vector3(1.5f - i, 1.5f - i, 1);
            yield return null;
        }

        myRenderer.color = new Color(1, 1, 1, 1);
        transform.localScale = new Vector3(1, 1, 1);
        if (GameManager.GetSingleton().getCurrent() == 1) {
            GameManager.GetSingleton().player2Units.Remove(this.gameObject);
        }
        else {
            GameManager.GetSingleton().player1Units.Remove(this.gameObject);
        }
        //occupiedTile.GetComponent<TileBehavior>().myUnit = null;
        Destroy(gameObject);
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
        // Play random step sound
        System.Random r = new System.Random();
        int stepNum = r.Next(0, stepSounds.Length);
        audioSource.clip = stepSounds[stepNum];
        audioSource.Play();
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

    public void AttackSound() {
        // Add sound
        // Play random step sound
        System.Random r = new System.Random();
        int attackNum = r.Next(0, attackSounds.Length);
        audioSource.clip = attackSounds[attackNum];
        audioSource.Play();
    }

    #region Stats
    public void ResetStats() {
        beenAttacked = false;
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
