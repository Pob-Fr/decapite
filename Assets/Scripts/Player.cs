using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {

    protected override void init() {
        base.init();
        attackMask = 1; // MASK zombi + dice
    }

    // Update is called once per frame
    private void Update() {
        Move(new Vector2(Input.GetAxisRaw("Horizontal") * Time.deltaTime, Input.GetAxisRaw("Vertical") * Time.deltaTime));
        if (Input.GetButton("Attack"))
            Attack();
    }

}
