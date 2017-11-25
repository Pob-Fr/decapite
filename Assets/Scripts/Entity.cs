using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour, Movable, Hitable {

    int currentHealth;
    public int maxHealth;

    public float movementSpeedFactor;
    public float attackSpeedFactor;

    public float attackDelay; // delay before the attack hit
    public float attackRecoverTime; // getting back to idle

    public int attackDamage;

    public float deathDuration;

    public float bodyThickness; // deepth size

    private void Start() {
        currentHealth = maxHealth;
    }

    public void Move(Vector2 direction) {
        transform.position += new Vector3(direction.x * movementSpeedFactor, direction.y * movementSpeedFactor, 0 );
    }

    public void GetHit(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    public virtual void Attack() {
        StartCoroutine(DoAttack());
    }

    public virtual IEnumerator DoAttack() {
        yield return new WaitForSeconds(attackDelay);
        // pick targets
        // do damage
        yield return new WaitForSeconds(attackRecoverTime);
        // back to idle
    }

    public virtual void Die() {
        StartCoroutine(Remove());
    }

    public virtual IEnumerator Remove() {
        yield return new WaitForSeconds(deathDuration);
        GameObject.Destroy(gameObject);
    }
}
