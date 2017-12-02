using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {

    public PlayerScore score;

    public AudioClip[] soundsVoice = new AudioClip[5];
    public List<AudioClip> killShouts = new List<AudioClip>();

    public PlayerSlot playerSlot = PlayerSlot.Player1;
    private PlayerControls controls;

    protected override void Init() {
        base.Init();
        if (playerSlot == PlayerSlot.Player1) controls = PlayerControls.player1Control;
        else controls = PlayerControls.player2Control;
        attackMask = (1 << 9) + (1 << 10); // MASK zombi + dice
        StartCoroutine(PlaySpawnSound());
    }

    private IEnumerator PlaySpawnSound() {
        yield return new WaitForSeconds(1.5f);
        audioSource.PlayOneShot(soundSpawn);
    }

    public void PunchLine() {
        int index = Random.Range(0, killShouts.Count);
        audioSource.PlayOneShot(killShouts[index], 1);
        if (index > 2)
            killShouts.Remove(killShouts[index]);
    }

    // Update is called once per frame
    private void Update() {
        if (isAlive) {
            Move(controls.GetAxisRaw());
            if (!isAttacking && controls.GetAttack()) {
                Attack();
            }
        }
        Animate();
    }

    public override void Heal(int heal) {
        base.Heal(heal);
        GameDirector.singleton.UpdatePlayerHealth(this);
    }

    public override void GetHit(int damage) {
        if (isAlive) {
            base.GetHit(damage);
            GameDirector.singleton.UpdatePlayerHealth(this);
            GameDirector.singleton.ShakeCamera(8);
        }
    }

    public override void GetHit(int damage, Entity hitter) {
        if (isAlive) {
            base.GetHit(damage, hitter);
            GameDirector.singleton.UpdatePlayerHealth(this);
            GameDirector.singleton.ShakeCamera(8);
        }
    }

    public override void Die() {
        base.Die();
        GameDirector.singleton.OnePlayerDead(gameObject);
    }

    public override void Die(Entity killer) {
        base.Die(killer);
        GameDirector.singleton.OnePlayerDead(gameObject);
    }

}
