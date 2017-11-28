using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    public AudioClip[] soundsVoice = new AudioClip[5];
    public List<AudioClip> killShouts = new List<AudioClip>();

    public int playerNum = 1;
    private PlayerControls controls;


    //public static GameObject playerPrefab = Resources.Load("Prefabs/Player");

    protected override void Init() {
        base.Init();
        if (playerNum == 1) controls = PlayerControls.BuildPlayer1Controls();
        else controls = PlayerControls.BuildPlayer2Controls();
        attackMask = (1 << 9) + (1 << 10); // MASK zombi + dice
        StartCoroutine(PlaySpawnSound());
    }

    private IEnumerator PlaySpawnSound() {
        yield return new WaitForSeconds(1.5f);
        audioSource.PlayOneShot(soundSpawn);
    }

    public void PunchLine() {
        int index = Random.Range(0, killShouts.Count - 1);
        audioSource.PlayOneShot(killShouts[index], 1);
        if (index > 2)
            killShouts.Remove(killShouts[index]);
    }

    // Update is called once per frame
    private void Update() {
        controls.UpdateControls();
        if (isAlive) {
            Move(controls.moveInput);
            if (!isAttacking && controls.attackInput) {
                Attack();
            }
        }
        Animate();
    }

    public override void Heal(int heal) {
        base.Heal(heal);
        GameDirector.singleton.UpdatePlayerHealth(currentHealth);
    }

    public override void GetHit(int damage) {
        base.GetHit(damage);
        GameDirector.singleton.UpdatePlayerHealth(currentHealth);
        GameDirector.singleton.ShakeCamera(8);
    }

    public override void Die() {
        base.Die();
        GameDirector.singleton.GameOver();
    }

}
