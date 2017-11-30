public interface Hitable {
    void GetHit(int damage);
    void GetHit(int damage, Entity hitter);
    void Die();
    void Die(Entity killer);

    float GetBodyWidth();
    float GetBodyThickness();
}
