﻿using System.Collections;
using UnityEngine;

public abstract class ZombieAbstract : Entity {

    public AudioClip soundIdle;
    public GameObject deathParticleSystem;
    public GameObject hitParticleSystem;

    protected Vector2 roamDirection;

    protected TargetAbstract TARGET_HELPER;

    public GameObject target {
        get { return TARGET_HELPER.currentTarget; }
        set { TARGET_HELPER.currentTarget = value; }
    }

    protected Hitable targetHitable {
        get { return TARGET_HELPER.currentTartegHitable; }
    }

    protected override void Init() {
        base.Init();
        audioSource.volume = AudioOptionController.GetZombiesVolume();
        audioSource.PlayOneShot(soundSpawn);
        StartCoroutine(RepickRoamDirection());
    }

    protected IEnumerator RepickRoamDirection() {
        while (true) {
            roamDirection = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
            yield return new WaitForSeconds(2f);
        }
    }

    public override void GetHit(int damage) {
        if (isAlive) {
            base.GetHit(damage);
            Stun();
        }
        if (isAlive) {
            if (deathParticleSystem != null) PlayHitParticle(null);
        }
    }

    public override void GetHit(int damage, Entity hitter) {
        if (isAlive) {
            base.GetHit(damage, hitter);
            Stun();
        }
        if(isAlive) {
            if (deathParticleSystem != null) PlayHitParticle(hitter);
        }
    }

    public override void Die() {
        base.Die();
        GetComponent<BoxCollider2D>().enabled = false;
        if (deathParticleSystem != null) PlayDeathParticle(null);
        Object.Destroy(GetComponent<Rigidbody2D>());
    }

    public override void Die(Entity killer) {
        base.Die(killer);
        GetComponent<BoxCollider2D>().enabled = false;
        if (deathParticleSystem != null) PlayDeathParticle(killer);
        Object.Destroy(GetComponent<Rigidbody2D>());
    }

    private void Update() {
        if (isAlive && !isSpawning) {
            if (target != null) {
                if (!isAttacking) {
                    Vector2 directionToPlayer = target.transform.position - transform.position;
                    if (Mathf.Abs(directionToPlayer.x) <= attackReach + (bodyWidth + targetHitable.GetBodyWidth()) / 2
                        && Mathf.Abs(directionToPlayer.y) <= (attackThickness + targetHitable.GetBodyThickness()) / 2) {
                        Attack();
                    } else {
                        if (Mathf.Abs(directionToPlayer.x) < Mathf.Abs(directionToPlayer.y)) { // vertical movement only
                            Move(new Vector2(0, Mathf.Sign(directionToPlayer.y)));
                        } else {
                            if (Mathf.Abs(directionToPlayer.y) > 0.5f) { // diagonal movement
                                Move(new Vector2(Mathf.Sign(directionToPlayer.x), Mathf.Sign(directionToPlayer.y)));
                            } else { // horizontal movement
                                Move(new Vector2(Mathf.Sign(directionToPlayer.x), 0));
                            }
                        }
                    }
                }
            } else {
                Move(roamDirection);
            }
        }
        Animate();
    }

    private void PlayDeathParticle(Entity killer = null) {
        Vector3 particlePos = transform.position + new Vector3(0, 1.5f, 0);
        GameObject bloodParticleObj = Instantiate(deathParticleSystem, particlePos, Quaternion.identity);
        ParticleSystem bloodParticleSys = bloodParticleObj.GetComponent<ParticleSystem>();
        var vel = bloodParticleSys.velocityOverLifetime;
        if (killer != null) {
            int direction = 1;
            if (killer.transform.position.x > transform.position.x) direction = -1;
            vel.xMultiplier = direction * Random.Range(3, 8);
        }
        vel.yMultiplier = Random.Range(-1, 2);
    }

    private void PlayHitParticle(Entity hitter = null) {
        Vector3 particlePos = transform.position + new Vector3(0, 1.5f, 0);
        GameObject bloodParticleObj = Instantiate(hitParticleSystem, particlePos, Quaternion.identity);
        ParticleSystem bloodParticleSys = bloodParticleObj.GetComponent<ParticleSystem>();
        var vel = bloodParticleSys.velocityOverLifetime;
        if (hitter != null) {
            int direction = 1;
            if (hitter.transform.position.x > transform.position.x) direction = -1;
            vel.xMultiplier = direction * Random.Range(3, 8);
        }
        vel.yMultiplier = Random.Range(-1, 2);
    }

}
