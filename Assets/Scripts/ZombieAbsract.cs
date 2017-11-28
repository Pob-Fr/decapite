using System.Collections;
using UnityEngine;

public abstract class ZombieAbstract : Entity {

    public AudioClip soundIdle;
    public GameObject target;
    protected Vector2 roamDirection;


    protected Hitable targetHitable {
        get { return TARGET_HITABLE; }
    }

    protected Hitable TARGET_HITABLE;

    protected override void Init() {
        base.Init();
        audioSource.PlayOneShot(soundSpawn);
        StartCoroutine(repickRoamDirection());
    }

    protected IEnumerator repickRoamDirection() {
        while (true) {
            roamDirection = new Vector2(Random.Range(-1, 1) * .2f, Random.Range(-1, 1)*.2f);
            yield return new WaitForSeconds(3f);
        }
    }

    public override void Die() {
        base.Die();
        GetComponent<BoxCollider2D>().enabled = false;
        Object.Destroy(GetComponent<Rigidbody2D>());
    }

    private void Update() {
        if (isAlive && !isSpawning) {
            if (target != null) {
                if (!isAttacking) {
                    Vector2 directionToPlayer = target.transform.position - transform.position;
                    if (Mathf.Abs(directionToPlayer.x) <= attackReach + (bodyWidth + targetHitable.GetBodyWidth()) / 2
                        && Mathf.Abs(directionToPlayer.y) <= (attackThickness + targetHitable.GetBodyThickness()) / 2) {
                        Attack();
                    } else {
                        if (Mathf.Abs(directionToPlayer.x) < Mathf.Abs(directionToPlayer.y)) { // vertical movement only
                            Move(new Vector2(0, Mathf.Sign(directionToPlayer.y) * 1));
                        } else {
                            if (Mathf.Abs(directionToPlayer.y) > 0.5f) { // diagonal movement
                                Move(new Vector2(Mathf.Sign(directionToPlayer.x) * 1, Mathf.Sign(directionToPlayer.y) * 1));
                            } else { // horizontal movement
                                Move(new Vector2(Mathf.Sign(directionToPlayer.x) * 1, 0));
                            }
                        }
                    }
                }
            } else {
                Move(roamDirection);
            }
        }
        Animate();
    }

}
