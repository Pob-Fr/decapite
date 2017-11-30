using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maggot : ZombieAbstract {


    public static Maggot Spawn(GameObject prefab, Vector3 position, GameObject target = null) {
        GameObject Maggot = GameObject.Instantiate(prefab);
        Maggot.transform.position = position;
        Maggot m = Maggot.GetComponent<Maggot>();
        m.TARGET_HELPER = new TargetPlayer();
        m.target = target;
        return m;
    }

    protected override void Init() {
        base.Init();
        attackMask = (1 << 8); // MASK player
    }

    public override void Die(Entity killer) {
        base.Die(killer);
        if (killer is Player) {
            Player p = (Player)killer;
            p.score.totalMaggotKills += 1;
        }
    }

}
