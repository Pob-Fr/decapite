using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public AudioClip[] soundsVoice = new AudioClip[5];

    //public static GameObject playerPrefab = Resources.Load("Prefabs/Player");

    protected override void Init() {
        base.Init();
        attackMask = (1 << 9) + (1 << 10); // MASK zombi + dice
    }

    // Update is called once per frame
    private void Update() {
        if (isAlive && !isAttacking) {
            Move(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            if (Input.GetButton("Attack"))
            {
                Attack();
            }
        }
        Animate();
    }

    public override void GetHit(int damage) {
        base.GetHit(damage);
        GameDirector.singleton.UpdatePlayerHealth(currentHealth);
        GameDirector.singleton.ShakeCamera(2);
    }

    public override void Die() {
        base.Die();
        GameDirector.singleton.GameOver();
    }

}
