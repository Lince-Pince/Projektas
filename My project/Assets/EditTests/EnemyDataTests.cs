using NUnit.Framework;

public class EnemyDataTests
{
    // UNIT TEST: Enemy dies when damage equals health
    [Test]
    public void TakeDamage_KillsEnemy_WhenHealthDepleted()
    {
        var enemy = new EnemyData(50f, 100, 10f);
        bool isDead = enemy.TakeDamage(50f);
        Assert.IsTrue(isDead, "Enemy should be dead when damage equals health.");
    }

    // UNIT TEST: Health is reduced properly without death
    [Test]
    public void TakeDamage_ReducesHealthCorrectly()
    {
        var enemy = new EnemyData(100f, 100, 10f);
        bool isDead = enemy.TakeDamage(30f);
        Assert.IsFalse(isDead, "Enemy should still be alive.");
        Assert.AreEqual(70f, enemy.health, 0.001f);
    }

    // UNIT TEST: Slow applies correct reduction to speed
    [Test]
    public void Slow_SetsCorrectSpeed()
    {
        var enemy = new EnemyData(100f, 100, 20f);
        enemy.Slow(0.25f); // 25% slow
        Assert.AreEqual(15f, enemy.speed, 0.001f);
    }

    // UNIT TEST: Full slow sets speed to 0
    [Test]
    public void Slow_ToZero_SetsSpeedToZero()
    {
        var enemy = new EnemyData(100f, 100, 20f);
        enemy.Slow(1f); // 100% slow
        Assert.AreEqual(0f, enemy.speed);
    }

    // STUB TEST: TakeDamage always returns false
    private class StubEnemy : EnemyData
    {
        public StubEnemy() : base(999f, 0, 1f) { }

        public override bool TakeDamage(float amount) => false;
    }

    [Test]
    public void Stub_TakeDamageAlwaysFalse()
    {
        var stubEnemy = new StubEnemy();
        bool result = stubEnemy.TakeDamage(999f);
        Assert.IsFalse(result, "Stub always returns false for TakeDamage.");
    }

    // MOCK TEST: Tracks the last damage value
    private class MockEnemy : EnemyData
    {
        public float lastDamageTaken = -1;

        public MockEnemy() : base(100f, 0, 1f) { }

        public override bool TakeDamage(float amount)
        {
            lastDamageTaken = amount;
            return base.TakeDamage(amount);
        }
    }

    [Test]
    public void Mock_TracksLastDamageTaken()
    {
        var mockEnemy = new MockEnemy();
        mockEnemy.TakeDamage(42f); // simulate damage
        Assert.AreEqual(42f, mockEnemy.lastDamageTaken);
    }

    // SPY TEST: Captures if and how Slow was called
    private class SpyEnemy : EnemyData
    {
        public bool slowCalled = false;
        public float lastSlowPercent = -1;

        public SpyEnemy() : base(100f, 0, 20f) { }

        public override void Slow(float percentage)
        {
            slowCalled = true;
            lastSlowPercent = percentage;
            base.Slow(percentage);
        }
    }

    [Test]
    public void Spy_VerifiesSlowWasCalled()
    {
        var spy = new SpyEnemy();
        spy.Slow(0.5f); // Apply slow

        Assert.IsTrue(spy.slowCalled, "Slow should be called.");
        Assert.AreEqual(0.5f, spy.lastSlowPercent, 0.001f);
        Assert.AreEqual(10f, spy.speed, 0.001f); // 50% of 20 = 10
    }
}
