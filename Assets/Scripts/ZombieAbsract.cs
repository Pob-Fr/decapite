using System.Collections;
using UnityEngine;

public abstract class ZombieAbstract : Entity {

    public AudioClip soundIdle;
    public GameObject target;

    protected Hitable targetHitable {
        get { return TARGET_HITABLE; }
    }

    protected Hitable TARGET_HITABLE;

    protected override void Init() {
        base.Init();
        audioSource.PlayOneShot(soundSpawn);
    }

    public override void Die() {
        base.Die();
    }

    private void Update() {
        if (isAlive && target != null && !isSpawning) {
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
        }
        Animate();
    }

}
