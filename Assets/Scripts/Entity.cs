using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour, Movable, Hitable {
    public AudioSource audioSource;
    public AudioClip soundAttack;
    public AudioClip soundDamaged;
    public AudioClip soundDie;
    public AudioClip[] soundsBatHit = new AudioClip[0];

    public const float verticalMovementSpeedFactor = 0.75f;

    public bool isSpawning = false;
    public float spawnDuration = 1f;

    public int maxHealth = 1;
    public int currentHealth;

    public bool isAlive {
        get { return currentHealth > 0; }
    }

    public float movementSpeed = 5;
    public float movementSpeedFactor {
        set {
            MOVEMENT_SPEED_FACTOR = value;
            animatorController.SetFloat("walkDuration", 1f / MOVEMENT_SPEED_FACTOR);
        }
        get { return MOVEMENT_SPEED_FACTOR; }
    }
    private float MOVEMENT_SPEED_FACTOR = 1;
    private bool isWalking;

    protected int attackMask = 0;
    public int attackDamage = 1;
    public float attackReach = 1;
    public float attackThickness = 0.4f;
    public float attackDelay = 0.2f; // delay before the attack hit
    public float attackRecoverTime = 0.2f; // getting back to idle
    public float attackSpeedFactor {
        set {
            ATTACK_SPEED_FACTOR = value;
            animatorController.SetFloat("attackDuration", 1f / (ATTACK_SPEED_FACTOR * (attackDelay + attackRecoverTime)));
        }
        get { return ATTACK_SPEED_FACTOR; }
    }
    private float ATTACK_SPEED_FACTOR = 1;
    protected bool isAttacking = false; // is in attack animation state

    public float deathDuration = 0.5f;

    public float bodyThickness = 0.4f; // deepth size
    public float bodyWidth = 1;

    protected Vector3 attackAreaLeftMin;
    protected Vector3 attackAreaLeftMax;
    protected Vector3 attackAreaRightMin;
    protected Vector3 attackAreaRightMax;

    protected bool isLookinkRight = true;

    protected BoxCollider2D collisionBox;
    protected Animator animatorController;
    protected SpriteRenderer sprite;

    void Start() {
        collisionBox = GetComponent<BoxCollider2D>();
        animatorController = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        Init();
        Spawn();
    }

    protected virtual void Init() {
        currentHealth = maxHealth;
        attackAreaLeftMin = new Vector2(-(bodyWidth / 2f), -attackThickness / 2f);
        attackAreaLeftMax = new Vector2(-(attackReach + bodyWidth / 2f), attackThickness / 2f);
        attackAreaRightMin = new Vector2(bodyWidth / 2f, -attackThickness / 2);
        attackAreaRightMax = new Vector2(attackReach + bodyWidth / 2f, attackThickness / 2f);
        collisionBox.size = new Vector2(bodyWidth, bodyThickness);
        collisionBox.offset = new Vector2(0, 0);
        movementSpeedFactor = 1;
        attackSpeedFactor = 1;
        animatorController.SetFloat("deathDuration", 1f / deathDuration);
    }

    public void Spawn() {
        StartCoroutine(DoSpawn());
    }

    public IEnumerator DoSpawn() {
        if (isSpawning) {
            animatorController.SetFloat("spawnDuration", 1f / spawnDuration);
            animatorController.SetBool("isSpawning", true);
            yield return new WaitForSeconds(deathDuration);
            animatorController.SetBool("isSpawning", false);
            isSpawning = false;
        }
    }

    protected void Animate() {
        animatorController.SetBool("isAttacking", isAttacking);
        animatorController.SetBool("isWalking", isWalking);
        animatorController.SetBool("isDead", !isAlive);
        sprite.flipX = !isLookinkRight;
    }

    public void Move(Vector2 direction) {
        isWalking = false;
        if (isAlive && !isAttacking) {
            if (direction.magnitude > float.Epsilon) {
                transform.position += new Vector3(direction.x * movementSpeed * movementSpeedFactor * Time.deltaTime,
                    direction.y * movementSpeed * movementSpeedFactor * Time.deltaTime, 0);
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
                if (direction.x > float.Epsilon) {
                    isLookinkRight = true;
                } else if (direction.x < -float.Epsilon) {
                    isLookinkRight = false;
                }
                isWalking = true;
            }
        }
    }

    public virtual void GetHit(int damage) {
        if (isAlive) {
            audioSource.PlayOneShot(soundDamaged);
            if (soundsBatHit.Length != 0) {
                int soundToPlay = Random.Range(0, 1);
                audioSource.PlayOneShot(soundsBatHit[soundToPlay]);
            }
            currentHealth -= damage;
            if (currentHealth <= 0)
                Die();
        }
    }

    public void GetHit(int damage, Entity hitter) {
        GetHit(damage);
    }

    public float GetBodyWidth() {
        return bodyWidth;
    }

    public float GetBodyThickness() {
        return bodyThickness;
    }

    public virtual void Attack() {
        if (isAlive && !isAttacking) {
            StopAllCoroutines();
            StartCoroutine(DoAttack(attackMask));
        }
    }

    protected virtual IEnumerator DoAttack(int hitmask) {
        if (isAlive) { // anim
                       // wait for impact
            isAttacking = true;
            isWalking = false;
            yield return new WaitForSeconds(attackDelay * attackSpeedFactor);
        }
        if (isAlive) { // hit

            // pick targets and damage them
            audioSource.PlayOneShot(soundAttack);
            foreach (Hitable h in pickTargets(hitmask)) {
                h.GetHit(attackDamage, this);
            }
            yield return new WaitForSeconds(attackRecoverTime * attackSpeedFactor);
        }
        if (isAlive) { // recover
                       // back to idle
            isAttacking = false;
        }
    }

    protected virtual List<Hitable> pickTargets(int hitmask) {
        List<Hitable> output = new List<Hitable>();
        Collider2D[] colliders;
        if (isLookinkRight) {
            colliders = Physics2D.OverlapAreaAll(transform.position + attackAreaRightMin, transform.position + attackAreaRightMax, hitmask);
        } else {
            colliders = Physics2D.OverlapAreaAll(transform.position + attackAreaLeftMin, transform.position + attackAreaLeftMax, hitmask);
        }
        foreach (Collider2D c in colliders) {
            if (c.gameObject != gameObject) {
                Hitable h = null;
                h = c.GetComponent<Entity>();
                if (h != null) {
                    output.Add(h);
                    continue;
                }
                h = c.GetComponent<Dice>();
                if (h != null) {
                    output.Add(h);
                    continue;
                }
            }
        }
        return output;
    }

    public virtual void Die() {
        audioSource.PlayOneShot(soundDie);
        isAttacking = false;
        isWalking = false;
        StopAllCoroutines();
        StartCoroutine(Remove());
    }

    protected virtual IEnumerator Remove() {
        yield return new WaitForSeconds(deathDuration);
        GameObject.Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        // draw foots
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position + new Vector3(0, 0, 0), new Vector2(bodyWidth, bodyThickness));
        // draw attack area
        Gizmos.color = Color.yellow;
        if (isLookinkRight)
            Gizmos.DrawCube(transform.position + new Vector3((bodyWidth + attackReach) / 2f, 0, 0)
                , new Vector2(attackReach, attackThickness));
        else
            Gizmos.DrawCube(transform.position - new Vector3((bodyWidth + attackReach) / 2f, 0, 0)
                , new Vector2(attackReach, attackThickness));
    }

}
