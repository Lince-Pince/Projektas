using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyMovementTests
{
    [Test]
    public void TakeDamage_HealthDropsToZero()
    {
        var enemy = new EnemyData(50f, 100, 10f);
        bool isDead = enemy.TakeDamage(50f);
        Assert.IsTrue(isDead);
    }

    [Test]
    public void TakeDamage_HealthAboveZero()
    {
        var enemy = new EnemyData(100f, 100, 10f);
        bool isDead = enemy.TakeDamage(30f);
        Assert.IsFalse(isDead);
        Assert.AreEqual(70f, enemy.health);
    }

    [Test]
    public void Slow_ChangesSpeedCorrectly()
    {
        var enemy = new EnemyData(100f, 100, 10f);
        enemy.Slow(0.5f);
        Assert.AreEqual(5f, enemy.speed);
    }

    [Test]
    public void Slow_DoesNotChangeSpeed()
    {
        var enemy = new EnemyData(100f, 100, 8f);
        enemy.Slow(0f);
        Assert.AreEqual(8f, enemy.speed);
    }

    [Test]
    public void Slow_SetsSpeedToZero()
    {
        var enemy = new EnemyData(100f, 100, 8f);
        enemy.Slow(1f);
        Assert.AreEqual(0f, enemy.speed);
    }

    [Test]
    public void WaypointPath_CorrectlyReturnsCount()
    {
        var path = new WaypointPath(new[] { "A", "B", "C" });
        Assert.AreEqual(3, path.GetWaypointCount());
    }

    [Test]
    public void WaypointPath_ReturnsCorrectWaypoint()
    {
        var path = new WaypointPath(new[] { "Start", "Mid", "End" });
        Assert.AreEqual("Mid", path.GetWaypointAt(1));
    }

    [Test]
    public void WaypointPath_ThrowsExceptionOnNegativeIndex()
    {
        var path = new WaypointPath(new[] { "A", "B" });
        Assert.Throws<System.IndexOutOfRangeException>(() => path.GetWaypointAt(-1));
    }

    [Test]
    public void WaypointPath_ThrowsExceptionOnOutOfBoundsIndex()
    {
        var path = new WaypointPath(new[] { "A", "B" });
        Assert.Throws<System.IndexOutOfRangeException>(() => path.GetWaypointAt(2));
    }

    [Test]
    public void WaypointPath_CanHandleEmptyList()
    {
        var path = new WaypointPath(new string[] { });
        Assert.AreEqual(0, path.GetWaypointCount());
    }
}