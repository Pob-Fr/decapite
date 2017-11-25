using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombi : Entity {

    //public static GameObject zombiPrefab = Resources.Load("Prefabs/Zombi");

    public static void Spawn(Vector2 position, GameObject target) {
        // GameObject zombi = GameObject.Instantiate(zombiPrefab);
        // zombi.GetComponent<Zombi>().target = target;
    }

    // public bool attackPlayer;
    // public bool attackDice;

    public GameObject target;

    protected override void Init() {
        base.Init();
        attackMask = 1; // MASK player
    }

    private void Update() {
        if (isAlive && target != null) {
            if (!isAttacking) {
                Vector2 directionToPlayer = target.transform.position - transform.position;
                if (directionToPlayer.magnitude <= bodyWidth / 2 + attackReach) {
                    Attack();
                } else {
                    if (Mathf.Abs(directionToPlayer.x) < Mathf.Abs(directionToPlayer.y)) { // vertical movement only
                        Move(new Vector2(0, Mathf.Sign(directionToPlayer.y) * 1));
                    } else {
                        if (Mathf.Abs(directionToPlayer.y) > 1) { // diagonal movement
                            Move(new Vector2(Mathf.Sign(directionToPlayer.x) * 1, Mathf.Sign(directionToPlayer.y) * 1));
                        } else { // horizontal movement
                            Move(new Vector2(Mathf.Sign(directionToPlayer.x) * 1, 0));
                        }
                    }
                }
            }
        }
    }

}
