using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour, Hitable
{

    public static void Spawn(GameObject prefab, Vector3 position)
    {
        GameObject dice = GameObject.Instantiate(prefab);
        dice.transform.position = position;
        //dice.GetComponent<Dice>();
    }

    Transform spriteRender;
    AudioSource audioSource;
    public List<AudioClip> sounds;
    new Animator animator;
    
    public int life;

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

    // Use this for initialization
    void Start()
    {
        // Init variables
        spriteRender = this.gameObject.transform.GetChild(0);
        audioSource = GetComponent<AudioSource>();
        animator = spriteRender.GetComponent<Animator>();

        crushAreaMin = new Vector2(-bodyWidth / 2, -bodyThickness / 2);
        crushAreaMax = new Vector2(bodyWidth / 2, bodyThickness / 2);

        // Init the content of the dice
        diceContent.AddEffectHolder(new EffectHolder(new EffectScore(100), 25));
        diceContent.AddEffectHolder(new EffectHolder(new EffectSpawnZombi(1), 50));
        diceContent.AddEffectHolder(new EffectHolder(new EffectSpawnHorde(5), 10000));
        diceContent.AddEffectHolder(new EffectHolder(new EffectScore(200), 25));

        isUsed = false;
        timeToDie = 3f;
        throwSpeedMax = 30f;
        speedUp = defaultSpeedUp;

        BoxCollider2D collisionBox = GetComponent<BoxCollider2D>();
        collisionBox.size = new Vector2(bodyWidth, bodyThickness);
        collisionBox.offset = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (bounce > 0)
        {
            Throw(throwSpeed);
        }
        else
        {
            animator.SetBool("isRolling", false);
        }

        if (bounce <= 0 && isUsed)
            Resolve();
    }

    void CrushZombis()
    {
        Collider2D[] colliders;
        colliders = Physics2D.OverlapAreaAll(transform.position + crushAreaMin, transform.position + crushAreaMax, 1<<9);
        foreach (Collider2D c in colliders)
        {
            if (c.gameObject != gameObject)
            {
                Hitable h = null;
                h = c.GetComponent<Entity>();
                if (h != null)
                {
                    h.GetHit(100);
                    continue;
                }
            }
        }
    }

    public void GetHit(int damage)
    {
        life -= damage;
    }

    public void GetHit(int damage, Entity hitter)
    {
        audioSource.PlayOneShot(sounds[0]);
        animator.SetBool("isRolling", true);
        isUsed = true;
        throwSpeed = throwSpeedMax;
        life -= damage;
        // Check if the dice get hit by the right of the left
        float direction;
        if (hitter.transform.position.x <= transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
        throwSpeed = Mathf.Abs(throwSpeed) * direction;
        speedUp = defaultSpeedUp;
        bounce = maxBounce;
        GameDirector.singleton.ShakeCamera(6);
    }

    public void Die()
    {
        // Must wait to be immobile before die and do his stuff.
        Destroy(gameObject);
    }
    
    public void Throw(float speed)
    {
        CrushZombis();
        // Bounce effect
        speedUp += fakeGrav;
        spriteRender.transform.Translate(Vector2.up * speedUp * Time.deltaTime);
        if (spriteRender.transform.localPosition.y <= 0 && (Mathf.Sign(speedUp) < 0))
        {
            spriteRender.transform.localPosition = new Vector2(0, 0);
            audioSource.PlayOneShot(sounds[1]);
            Debug.Log("BOuing !");
            bounce--;
            speedUp = defaultSpeedUp / 1.2f;
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    
    public void Resolve()
    {
        Destroy(this.gameObject.GetComponent(typeof(BoxCollider2D)));
        StartCoroutine(Opening());
    }

    IEnumerator Opening()
    {
        // Play animation
        animator.SetBool("isOpening", true);
        yield return new WaitForSeconds(timeToDie);
        // Run the effect
        Effect effect = diceContent.RandomEffect();
        effect.DoSomething();
        Die();
    }


    public float GetBodyWidth()
    {
        return 0;
    }
    public float GetBodyThickness()
    {
        return 0;
    }

    
    void OnGUI()
    {
        //if (GUILayout.Button("Hit at right!"))
        // GetHit(1, true);
        //if (GUILayout.Button("Hit at left!"))
        // GetHit(1, false);
        //if (GUILayout.Button("Get hit"))
        //    GetHit(1, whoHit);
        //if (GUILayout.Button("Touch something"))
        //    Resolve();
    }
}
