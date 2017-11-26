﻿using UnityEngine;

public class EffectSpawnHorde : Effect
{

    public static AudioClip eventClip = null;

    int nbZombi;

    public EffectSpawnHorde(int number)
    {
        this.nbZombi = number;
    }

    public void ChangeNbZombi(int number)
    {
        this.nbZombi = number;
    }

    public void DoSomething()
    {
        GameDirector.singleton.SpawnZombis(nbZombi);
        GameDirector.singleton.Event("Horde !!!", eventClip);
        GameDirector.singleton.ShakeCamera(10);
    }
}
