using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour, Movable, Hitable {

    public const float verticalMovementSpeedFactor= 0.75f;

    public int maxHealth = 1;
    protected int currentHealth;

    public bool isAlive {
        get { return currentHealth > 0; }
    }

    public float movementSpeedFactor = 5;
    public float attackSpeedFactor = 1;

    public float attackDelay = 0.2f; // delay before the attack hit
    public float attackRecoverTime = 0.2f; // getting back to idle

    protected int attackMask = 0;
    public int attackDamage = 1;
    public float attackReach = 1;
    protected bool isAttacking = false; // is in attack animation state

    public float deathDuration = 0.5f;

    public float bodyThickness = 0.4f; // deepth size
    public float bodyWidth = 1;

    protected Vector2 attackAreaLeft;
    protected Vector2 attackAreaRight;
    protected bool lookingRight = true;

    protected BoxCollider2D collisionBox;

    void Start() {
        Init();
    }

    protected virtual void Init() {
        currentHealth = maxHealth;
        attackAreaLeft = new Vector2(-(attackReach + bodyWidth / 2f), bodyThickness);
        attackAreaRight = new Vector2(attackReach + bodyWidth / 2f, bodyThickness);
        collisionBox = GetComponent<BoxCollider2D>();
        collisionBox.size = new Vector2(bodyWidth, bodyThickness);
        collisionBox.offset = new Vector2(0, bodyThickness / 2);
    }

    public void Move(Vector2 direction) {
        if (isAlive && !isAttacking) {
            transform.position += new Vector3(direction.x * movementSpeedFactor * Time.deltaTime,
                direction.y * movementSpeedFactor * verticalMovementSpeedFactor * Time.deltaTime, 0);
            lookingRight = direction.x < 0;
        }
    }

    public void GetHit(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    public void GetHit(int damage, Entity hitter) {
        GetHit(damage);
    }


    public virtual void Attack() {
        if (isAlive && !isAttacking)
            StartCoroutine(DoAttack(attackMask));
    }

    protected virtual IEnumerator DoAttack(int hitmask) {
        // TODO trigger animation
        Debug.Log("Start attack animation");
        // wait for impact
        isAttacking = true;
        yield return new WaitForSeconds(attackDelay);
        // pick targets and damage them
        Debug.Log("Attack impact");
        foreach (Hitable h in pickTargets(hitmask)) {
            h.GetHit(attackDamage);
        }
        yield return new WaitForSeconds(attackRecoverTime);
        // back to idle
        Debug.Log("Attack recovered");
        isAttacking = false;
    }

    protected virtual List<Hitable> pickTargets(int hitmask) {
        List<Hitable> output = new List<Hitable>();
        Collider2D[] colliders;
        if (lookingRight) {
            colliders = Physics2D.OverlapAreaAll(transform.position, attackAreaRight, hitmask, 0);
        } else {
            colliders = Physics2D.OverlapAreaAll(transform.position, attackAreaLeft, hitmask, 0);
        }
        foreach (Collider2D c in colliders) {
            if (c.gameObject != gameObject) {
                Hitable h = null;
                h = c.GetComponent<Entity>();
                if (h != null) {
                    output.Add(h);
                    continue;
                }
                //h = c.GetComponent<Dice>();
                //if (h != null) {
                //    output.Add(h);
                //    continue;
                //}
            }
        }
        return output;
    }

    public virtual void Die() {
        StartCoroutine(Remove());
    }

    protected virtual IEnumerator Remove() {
        yield return new WaitForSeconds(deathDuration);
        GameObject.Destroy(gameObject);
    }
}
