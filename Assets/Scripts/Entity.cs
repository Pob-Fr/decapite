using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour, Movable, Hitable {

    public int maxHealth;
    protected int currentHealth;

    public float movementSpeedFactor;
    public float attackSpeedFactor;

    public float attackDelay; // delay before the attack hit
    public float attackRecoverTime; // getting back to idle
    
    protected int attackMask;
    public int attackDamage;
    public float attackReach;
    protected bool attacking; // is in attack animation state

    public float deathDuration;

    public float bodyThickness = 0.4f; // deepth size
    public float bodyWidth = 1;

    protected Vector2 attackAreaLeft;
    protected Vector2 attackAreaRight;
    protected bool lookingRight = true;

    protected BoxCollider2D collisionBox;

    void Start() {
        init();
    }

    protected virtual void init() {
        currentHealth = maxHealth;
        attackAreaLeft = new Vector2(-(attackReach + bodyWidth / 2f), bodyThickness);
        attackAreaRight = new Vector2(attackReach + bodyWidth / 2f, bodyThickness);
        collisionBox = GetComponent<BoxCollider2D>();
        collisionBox.size = new Vector2(bodyWidth, bodyThickness);
        collisionBox.offset = new Vector2(0, bodyThickness / 2);
    }

    public void Move(Vector2 direction) {
        transform.position += new Vector3(direction.x * movementSpeedFactor, direction.y * movementSpeedFactor * 0.75f, 0);
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
        if(!attacking)
            DoAttack(attackMask);
    }

    protected virtual IEnumerator DoAttack(int hitmask) {
        // TODO trigger animation
        // wait for impact
        attacking = true;
        yield return new WaitForSeconds(attackDelay);
        // pick targets and damage them
        foreach(Hitable h in pickTargets(hitmask)) {
            h.GetHit(attackDamage);
        }
        yield return new WaitForSeconds(attackRecoverTime);
        // back to idle
        attacking = false;
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
            if (c != collisionBox) {
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
