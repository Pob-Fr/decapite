﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : ZombieAbstract {


    public static Zombie Spawn(GameObject prefab, Vector3 position, GameObject target = null) {
        GameObject zombie = GameObject.Instantiate(prefab);
        zombie.transform.position = position;
        Zombie z = zombie.GetComponent<Zombie>();
        z.TARGET_HELPER = new TargetPlayer();
        z.target = target;
        return z;
    }

    protected override void Init() {
        base.Init();
        attackMask = (1 << 8); // MASK player
        if (GameDirector.singleton.numberPlayers > 1) StartCoroutine(AutoChangeTarget());
    }

    public override void Die(Entity killer) {
        base.Die(killer);
        if (killer is Player) {
            Player p = (Player)killer;
            p.score.totalZombieKills += 1;
        }
    }

    public IEnumerator Enrage(float rageDelay) {
        if (isAlive) {
            yield return new WaitForSeconds(rageDelay);
            // do enrage effect cue
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.7f, 0.7f);
            yield return new WaitForSeconds(0.5f);
            movementSpeedFactor = 2.5f;
            attackSpeedFactor = 0.5f;
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.4f, 0.4f);
        }
    }

    private IEnumerator AutoChangeTarget() {
        GameDirector gd = GameDirector.singleton;
        while (gd.numberPlayers > 0) {
            yield return new WaitForSeconds(0.5f);
            if (target == null || target.Equals(null)) {
                target = gd.GetRandomPlayerToChase();
            }
        }
    }

}
