using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maggot : ZombieAbstract {


    public static Maggot Spawn(GameObject prefab, Vector3 position, GameObject target) {
        GameObject Maggot = GameObject.Instantiate(prefab);
        Maggot.transform.position = position;
        Maggot m = Maggot.GetComponent<Maggot>();
        m.target = target;
        return m;
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
        ScoreHelper.totalMaggotKills++;
    }

}
