public interface Hitable {
    void GetHit(int damage);
    void GetHit(int damage, bool hitter);
    void Die();
}
