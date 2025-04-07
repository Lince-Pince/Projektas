public class EnemyData
{
    public float health;
    public int worth;
    public float startSpeed;
    public float speed;

    public EnemyData(float health, int worth, float startSpeed)
    {
        this.health = health;
        this.worth = worth;
        this.startSpeed = startSpeed;
        this.speed = startSpeed; // Fixing original constructor mistake too
    }

    // Marked virtual to allow overriding in tests (for mocking/stubbing/spying)
    public virtual bool TakeDamage(float amount)
    {
        health -= amount;
        return health <= 0;
    }

    // Marked virtual to allow override in tests
    public virtual void Slow(float percentage)
    {
        speed = startSpeed * (1f - percentage);
    }
}
