using System.Collections;
using UnityEngine;

public abstract class ZombieAbstract : Entity {

    public AudioClip soundIdle;

    protected Vector2 roamDirection;

    protected TargetAbstract TARGET_HELPER;

    public GameObject target {
        get { return TARGET_HELPER.currentTarget; }
        set { TARGET_HELPER.currentTarget = value; }
    }

    protected Hitable targetHitable {
        get { return TARGET_HELPER.currentTartegHitable; }
    }

    protected override void Init() {
        base.Init();
        audioSource.PlayOneShot(soundSpawn);
        StartCoroutine(RepickRoamDirection());
    }

    protected IEnumerator RepickRoamDirection() {
        while (true) {
            roamDirection = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
            yield return new WaitForSeconds(2f);
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
                            Move(new Vector2(0, Mathf.Sign(directionToPlayer.y) ));
                        } else {
                            if (Mathf.Abs(directionToPlayer.y) > 0.5f) { // diagonal movement
                                Move(new Vector2(Mathf.Sign(directionToPlayer.x) , Mathf.Sign(directionToPlayer.y) ));
                            } else { // horizontal movement
                                Move(new Vector2(Mathf.Sign(directionToPlayer.x) , 0));
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
