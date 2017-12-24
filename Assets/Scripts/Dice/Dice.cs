using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour, Hitable {

    public static Dice Spawn(GameObject prefab, Vector3 position) {
        GameObject dice = GameObject.Instantiate(prefab);
        dice.transform.position = position;
        Dice d = dice.GetComponent<Dice>();
        return d;
    }

    public Transform spriteRender;
    public AudioSource audioSource;
    public Animator animator;

    public List<AudioClip> sounds;

    public AudioClip clipBonus;
    public AudioClip clipMalus;

    public int life;

    private Player lastAttacker = null;

    // Bounce stats
    int bounce = 0;
    public int maxBounce;
    float speedUp;
    public float defaultSpeedUp = 20f;
    float fakeGrav = -2f;

    // Dice specifique informations
    bool isUsed = false;
    float timeToDie = 3f;
    public DiceContent diceContent = new DiceContent();
    float throwSpeed;
    float throwSpeedMax;

    // Hit zombies
    protected Vector3 crushAreaMin;
    protected Vector3 crushAreaMax;
    public float bodyThickness = 0.4f;
    public float bodyWidth = 1f;
    private int kills = 0;
    public GameObject killScoreTextPrefab;
    private readonly int killScore = 2;


    // Use this for initialization
    void Start() {
        // Init variables

        crushAreaMin = new Vector2(-bodyWidth / 2, -bodyThickness / 2);
        crushAreaMax = new Vector2(bodyWidth / 2, bodyThickness / 2);

        // Init the content of the dice
        diceContent.AddEffectHolder(new EffectHolder(new EffectScore(this, 100), 50));
        diceContent.AddEffectHolder(new EffectHolder(new EffectSpawnZombie(this), 40));
        diceContent.AddEffectHolder(new EffectHolder(new EffectSpawnHorde(this, 2), 30));
        diceContent.AddEffectHolder(new EffectHolder(new EffectSpawnMaggot(this), 50));
        diceContent.AddEffectHolder(new EffectHolder(new EffectSpawnHorde(this, 3), 20));
        diceContent.AddEffectHolder(new EffectHolder(new EffectHealth(this, 1), 30));
        diceContent.AddEffectHolder(new EffectHolder(new EffectZombieIncreaseSpawn(this, 1), 50));
        diceContent.AddEffectHolder(new EffectHolder(new EffectZombieRageFaster(this, 1f), 10));
        diceContent.AddEffectHolder(new EffectHolder(new EffectScore(this, 200), 30));

        isUsed = false;
        timeToDie = 3f;
        throwSpeedMax = 30f;
        speedUp = defaultSpeedUp;

        BoxCollider2D collisionBox = GetComponent<BoxCollider2D>();
        collisionBox.size = new Vector2(bodyWidth, bodyThickness);
        collisionBox.offset = new Vector2(0, 0);

        GameDirector.singleton.StartTrackingDice(this);
    }

    // Update is called once per frame
    void Update() {
        if (bounce > 0) {
            Throw(throwSpeed);
        } else {
            animator.SetBool("isRolling", false);
        }

        if (bounce <= 0 && isUsed)
            Resolve();
    }

    void CrushZombis() {
        Collider2D[] colliders;
        colliders = Physics2D.OverlapAreaAll(transform.position + crushAreaMin, transform.position + crushAreaMax, 1 << 9);
        foreach (Collider2D c in colliders) {
            if (c.gameObject != gameObject) {
                Entity e = null;
                e = c.GetComponent<Entity>();
                if (e != null && e.isAlive) {
                    e.GetHit(100);
                    ++kills;
                    GainKillScore(e);
                    continue;
                }
            }
        }
    }

    private void GainKillScore(Entity e)
    {
        if (lastAttacker == null) return;
        int finalKillScore = killScore * kills;
        GameDirector.singleton.AddScore(lastAttacker, finalKillScore);
        //Instantiate the text mesh printing the score
        Vector3 textPos = new Vector3(e.transform.position.x, e.transform.position.y + 1.5f, e.transform.position.z - 1f);
        GameObject textObj = Instantiate(killScoreTextPrefab, textPos, Quaternion.identity, e.transform);
        //Set correct size and value
        TextMesh textMesh = textObj.GetComponent<TextMesh>();
        textMesh.fontSize = (int)(textMesh.fontSize * (1 + 0.1f * (kills - 1))); //Make the text 10% bigger for each kill
        textMesh.text = '+' + finalKillScore.ToString();
    }

    public void GetHit(int damage) {
        life -= damage;
    }

    public void GetHit(int damage, Entity hitter) {
        if (hitter is Player)
            lastAttacker = (Player)hitter;
        audioSource.PlayOneShot(sounds[0]);
        animator.SetBool("isRolling", true);
        isUsed = true;
        throwSpeed = throwSpeedMax;
        life -= damage;
        // Check if the dice get hit by the right of the left
        float direction;
        if (hitter.transform.position.x <= transform.position.x) {
            direction = 1;
        } else {
            direction = -1;
        }
        throwSpeed = Mathf.Abs(throwSpeed) * direction;
        speedUp = defaultSpeedUp;
        bounce = maxBounce;
        GameDirector.singleton.ShakeCamera(6);
    }

    public void Die() {
        // Must wait to be immobile before die and do his stuff.
        Destroy(gameObject);
    }

    public virtual void Die(Entity killer) {
        Die();
    }

    public void Throw(float speed) {
        CrushZombis();
        // Bounce effect
        speedUp += fakeGrav;
        spriteRender.transform.Translate(Vector2.up * speedUp * Time.deltaTime);
        if (spriteRender.transform.localPosition.y <= 0 && (Mathf.Sign(speedUp) < 0)) {
            spriteRender.transform.localPosition = new Vector2(0, 0);
            audioSource.PlayOneShot(sounds[1]);
            bounce--;
            speedUp = defaultSpeedUp / 1.2f;
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        if (transform.position.x > 16.5f || transform.position.x < -16.5f) {
            this.throwSpeed *= -1;
            transform.Translate(Vector2.right * throwSpeed * 2 * Time.deltaTime);
            audioSource.PlayOneShot(sounds[1]);
        }
    }

    public void Resolve() {
        Destroy(this.gameObject.GetComponent(typeof(BoxCollider2D)));
        StartCoroutine(Opening());
    }

    IEnumerator Opening() {
        // Play animation
        animator.SetBool("isOpening", true);
        GameDirector.singleton.StopTrackingDice(this);
        if (lastAttacker is Player) {
            Player p = (Player)lastAttacker;
            if (kills > p.score.bestDiceStreak) {
                p.score.bestDiceStreak = kills;
                if (kills > 5)
                    GameDirector.singleton.PlayerPunchLine(p);
            }
        }
        yield return new WaitForSeconds(timeToDie);
        // Run the effect
        Effect effect = diceContent.RandomEffect();
        effect.DoSomething(lastAttacker);
        if (effect.isBonus())
            audioSource.PlayOneShot(clipBonus);
        else if (effect.isMalus())
            audioSource.PlayOneShot(clipMalus);
        yield return new WaitForEndOfFrame();
        Die();
    }


    public float GetBodyWidth() {
        return 0;
    }
    public float GetBodyThickness() {
        return 0;
    }
}
