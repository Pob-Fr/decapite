using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    public AudioClip[] soundsVoice = new AudioClip[5];
    public List<AudioClip> killShouts = new List<AudioClip>();

    //public static GameObject playerPrefab = Resources.Load("Prefabs/Player");

    protected override void Init() {
        base.Init();
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
        if (isAlive && !isAttacking) {
            Move(GetInput());
            if (Input.GetButton("Attack")) {
                Attack();
            }
        }
        Animate();
    }

    private Vector2 GetInput() {
        Vector2 output = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (output.magnitude < 0.35f)
            output = Vector2.zero;
        return output;
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
