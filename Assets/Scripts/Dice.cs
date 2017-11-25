using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour, Hitable
{
    Transform spriteRender;
    AudioSource audioSource;
    public List<AudioClip> sounds;
    Animator animator;

    float throwSpeed;
    float throwSpeedMax;

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

    // Use this for initialization
    void Start()
    {
        // Init variables
        spriteRender = this.gameObject.transform.GetChild(0);
        audioSource = GetComponent<AudioSource>();
        animator = spriteRender.GetComponent<Animator>();

        // Init the content of the dice
        diceContent.AddEffectHolder(new EffectHolder(new EffectScore(100), 25));
        diceContent.AddEffectHolder(new EffectHolder(new EffectSpawnZombi(1), 50));
        diceContent.AddEffectHolder(new EffectHolder(new EffectScore(200), 25));

        isUsed = false;
        timeToDie = 3f;
        throwSpeedMax = 30f;
        speedUp = defaultSpeedUp;
    }

    // Update is called once per frame
    void Update()
    {
        if (bounce > 0 && !isUsed)
        {
            animator.SetBool("isRolling", true);
            Throw(throwSpeed);
        }
        else
        {
            animator.SetBool("isRolling", false);
        }
    }

    public void GetHit(int damage)
    {
        life -= damage;
    }

    public void GetHit(int damage, Entity hitter)
    {
        audioSource.PlayOneShot(sounds[0]);
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
    }

    public void Die()
    {
        // Must wait to be immobile before die and do his stuff.
        Destroy(gameObject);
    }
    
    public void Throw(float speed)
    {
        // Bounce effect
        speedUp += fakeGrav;
        spriteRender.transform.Translate(Vector2.up * speedUp * Time.deltaTime);
        if (spriteRender.transform.position.y <= 0 && Mathf.Sign(speedUp) < 0)
        {
            transform.position = new Vector2(spriteRender.transform.position.x, transform.position.y);
            audioSource.PlayOneShot(sounds[1]);
            bounce--;
            speedUp = defaultSpeedUp / Mathf.Pow(2, maxBounce - bounce);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    
    public void Resolve()
    {
        isUsed = true;
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
        if (GUILayout.Button("Touch something"))
            Resolve();
    }
}
