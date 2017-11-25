public class EffectScore : Effect
{
    int score;

    public EffectScore (int score)
    {
        this.score = score;
    }

    public void ChangeScore (int score)
    {
        this.score = score;
    }

    public void DoSomething()
    {
        //        GameDirector.singleton.AddScore(score);
    }
}
