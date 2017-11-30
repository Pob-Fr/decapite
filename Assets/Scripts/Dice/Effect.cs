public abstract class Effect {

    protected Dice dice;

    public Effect(Dice dice) {
        this.dice = dice;
    }

    public abstract void DoSomething(Player lastAttacker);
    public virtual bool isBonus() {
        return false;
    }
    public virtual bool isMalus() {
        return false;
    }
}
