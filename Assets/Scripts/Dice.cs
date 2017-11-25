using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour, Hitable
{
    public float GetBodyWidth() {
        return 0;
    }
    public float GetBodyThickness() {
        return 0;
    }

    Transform spriteRender;
    AudioSource audioSource;
    public AudioClip sound1;

    float throwSpeed;

    public int life;
    public Vector2 pos;
    float height = 0;
    public float maxHeight = 1;
    float moveUp = 1;

    // Bounce stats
    int bounce = 0;
    public int maxBounce;

    // Dice specifique informations
    bool isUsed = false;
    float timeToDie = 3f;

    // Use this for initialization
    void Start()
    {
        // Init variables
        spriteRender = this.gameObject.transform.GetChild(0);
        audioSource = GetComponent<AudioSource>();
        bool isUsed = false;
        timeToDie = 3f;
        moveUp = 1;
        throwSpeed = 10f;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        if (bounce > 0 & !isUsed)
            Throw(throwSpeed);
    }

    public void GetHit(int damage)
    {
        life -= damage;
    }

    public void GetHit(int damage, Entity hitter)
    {
        audioSource.PlayOneShot(sound1);
        life -= damage;
        // Check if the dice get hit by the right of the left
        bool toDebug = true;
        float direction;
        direction = toDebug ? direction = 1 : direction = -1;
        throwSpeed = Mathf.Abs(throwSpeed) * direction;
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
        height += (2.5f * moveUp * Time.deltaTime);
        if (height >= (maxHeight - 0.05f))
        {
            height = maxHeight - 0.05f;
            moveUp = -1;
        }
        if (height <= 0)
        {
            height = 0;
            moveUp = 1;
            bounce--;
        }
        spriteRender.transform.position = new Vector2(spriteRender.transform.position.x, height);
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        
    }
    
    public void hitSomething()
    {
        isUsed = true;
        height = 0;
        spriteRender.transform.position = new Vector2(spriteRender.transform.position.x, height);
        Destroy(this.gameObject.GetComponent(typeof(BoxCollider2D)));
        StartCoroutine(Opening());
    }

    IEnumerator Opening()
    {
        // Play animation
        spriteRender.Rotate(0f, 0f, 45f);
        yield return new WaitForSeconds(timeToDie);
        // Faire le bonus/malus à la fin de l'animation
        Die();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Hit at right!"))
            // GetHit(1, true);
        if (GUILayout.Button("Hit at left!"))
            // GetHit(1, false);
        if (GUILayout.Button("Touch something"))
            hitSomething();
    }
}
