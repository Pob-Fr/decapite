using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour, Movable, Hitable {

    public const float verticalMovementSpeedFactor = 0.75f;

    public int maxHealth = 1;
    protected int currentHealth;

    public bool isAlive {
        get { return currentHealth > 0; }
    }

    public float movementSpeedFactor = 5;
    public float attackSpeedFactor = 1;
    private bool isWalking;

    public float attackDelay = 0.2f; // delay before the attack hit
    public float attackRecoverTime = 0.2f; // getting back to idle

    protected int attackMask = 0;
    public int attackDamage = 1;
    public float attackReach = 1;
    public float attackThickness = 0.4f;
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
        sprite = GetComponent<SpriteRenderer>();
        Init();
    }

    protected virtual void Init() {
        currentHealth = maxHealth;
        attackAreaLeftMin = new Vector2(-(bodyWidth / 2f), -attackThickness / 2f);
        attackAreaLeftMax = new Vector2(-(attackReach + bodyWidth / 2f), attackThickness / 2f);
        attackAreaRightMin = new Vector2(bodyWidth / 2f, attackThickness);
        attackAreaRightMax = new Vector2(attackReach + bodyWidth / 2f, -attackThickness / 2f);
        collisionBox.size = new Vector2(bodyWidth, bodyThickness / 2f);
        collisionBox.offset = new Vector2(0, bodyThickness / 2);
    }

    protected void Animate() {
        animatorController.SetBool("isAttacking", isAttacking);
        animatorController.SetBool("isWalking", isWalking);
        sprite.flipX = !isLookinkRight;
    }

    public void Move(Vector2 direction) {
        isWalking = false;
        if (isAlive && !isAttacking) {
            if (direction.magnitude > float.Epsilon) {
                transform.position += new Vector3(direction.x * movementSpeedFactor * Time.deltaTime,
                    direction.y * movementSpeedFactor * verticalMovementSpeedFactor * Time.deltaTime, 0);
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

    public void GetHit(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
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
        if (isAlive && !isAttacking)
            StartCoroutine(DoAttack(attackMask));
    }

    protected virtual IEnumerator DoAttack(int hitmask) {
        if (isAlive) { // anim
            // TODO trigger animation
            Debug.Log("Start attack animation");
            // wait for impact
            isAttacking = true;
            isWalking = false;
            yield return new WaitForSeconds(attackDelay);
        }
        if (isAlive) { // hit
            // pick targets and damage them
            Debug.Log("Attack impact");
            foreach (Hitable h in pickTargets(hitmask)) {
                h.GetHit(attackDamage);
            }
            yield return new WaitForSeconds(attackRecoverTime);
        }
        if (isAlive) { // recover
            // back to idle
            Debug.Log("Attack recovered");
            isAttacking = false;
        }
    }

    protected virtual List<Hitable> pickTargets(int hitmask) {
        List<Hitable> output = new List<Hitable>();
        Collider2D[] colliders;
        if (isLookinkRight) {
            colliders = Physics2D.OverlapAreaAll(transform.position + attackAreaRightMin, transform.position + attackAreaRightMax, hitmask, 0);
        } else {
            colliders = Physics2D.OverlapAreaAll(transform.position + attackAreaLeftMin, transform.position + attackAreaLeftMax, hitmask, 0);
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
            Gizmos.DrawCube(transform.position + new Vector3(bodyWidth, 0, 0)
                , new Vector2(attackReach, attackThickness));
        else
            Gizmos.DrawCube(transform.position - new Vector3(bodyWidth, 0, 0)
                , new Vector2(attackReach, attackThickness));
    }

}
