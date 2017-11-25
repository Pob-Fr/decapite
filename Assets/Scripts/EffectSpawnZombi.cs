public class EffectSpawnZombi : Effect
{
    int nbZombi;

    public EffectSpawnZombi(int number)
    {
        this.nbZombi = number;
    }

    public void ChangeNbZombi(int number)
    {
        this.nbZombi = number;
    }

    public void DoSomething()
    {
        //        GameDirector.singleton.SpawnZombi(score);
    }
}
