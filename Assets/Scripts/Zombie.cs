﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : ZombieAbstract {


    public static Zombie Spawn(GameObject prefab, Vector3 position, GameObject target) {
        GameObject zombie = GameObject.Instantiate(prefab);
        zombie.transform.position = position;
        Zombie z = zombie.GetComponent<Zombie>();
        z.target = target;
        return z;
    }

    // public bool attackPlayer;
    // public bool attackDice;

    protected new Hitable targetHitable {
        get { return TARGET_HITABLE; }
    }

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
    }

    public override void Die() {
        base.Die();
        GameDirector.singleton.zombieKills++;
    }

    public IEnumerator Enrage(float rageDelay) {
        if (isAlive) {
            yield return new WaitForSeconds(rageDelay);
            movementSpeedFactor = 2.5f;
            attackSpeedFactor = 0.5f;
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.4f, 0.4f);
        }
    }

}
