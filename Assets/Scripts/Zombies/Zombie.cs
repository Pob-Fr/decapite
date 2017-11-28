using System.Collections;
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
    }

    public override void Die() {
        base.Die();
        ScoreHelper.totalZombieKills++;
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
