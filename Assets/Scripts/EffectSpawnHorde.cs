using UnityEngine;

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

    public bool isBonus() {
        return false;
    }

    public bool isMalus() {
        return true;
    }

    public void DoSomething()
    {
        GameDirector.singleton.SpawnHorde(nbZombi);
        GameDirector.singleton.Event("Horde !!!", eventClip);
        GameDirector.singleton.ShakeCamera(10);
    }
}
