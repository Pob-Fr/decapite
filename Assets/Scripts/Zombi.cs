using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombi : Entity {
    public AudioClip soundIdle;


    public static void Spawn(GameObject prefab, Vector3 position, GameObject target) {
        GameObject zombi = GameObject.Instantiate(prefab);
        zombi.transform.position = position;
        Zombi z = zombi.GetComponent<Zombi>();
        z.target = target;
    }

    // public bool attackPlayer;
    // public bool attackDice;

    public GameObject target;

    protected Hitable targetHitable {
        get { return TARGET_HITABLE; }
    }

    protected Hitable TARGET_HITABLE;

    protected override void Init() {
        base.Init();
        attackMask = (1 << 8); // MASK player
        if (target != null) {
            TARGET_HITABLE = target.GetComponent<Entity>();
            if (TARGET_HITABLE == null)
                TARGET_HITABLE = target.GetComponent<Dice>();
            if (TARGET_HITABLE == null)
                Debug.Log("Not a valid target !");
        }
        audioSource.PlayOneShot(soundSpawn);
        StartCoroutine(Enrage());
    }

    private IEnumerator Enrage() {
        if (isAlive) {
            yield return new WaitForSeconds(6f);
            movementSpeedFactor = 2.5f;
            attackSpeedFactor = 0.5f;
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.4f, 0.4f);
        }
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
