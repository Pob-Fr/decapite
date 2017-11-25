public interface Hitable {
    void GetHit(int damage);
    void GetHit(int damage, Entity hitter);
    void Die();

    float GetBodyWidth();
    float GetBodyThickness();
}
