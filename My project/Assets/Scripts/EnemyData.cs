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
        this.speed = startSpeed;
    }

    public virtual bool TakeDamage(float amount)
    {
        health -= amount;
        return health <= 0;
    }

    public virtual void Slow(float percentage)
    {
        speed = startSpeed * (1f - percentage);
    }
}
