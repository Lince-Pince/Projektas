using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyMovementTests
{
    [Test]
    public void GetNextWaypointIndex_MovesToNextWaypoint()
    {
        var movement = new EnemyMovement();
        int result = movement.GetNextWaypointIndex(1, 5);
        Assert.AreEqual(2, result);
    }

    [Test]
    public void GetNextWaypointIndex_LastWaypoint_ReturnsMinusOne()
    {
        var movement = new EnemyMovement();
        int result = movement.GetNextWaypointIndex(4, 5);
        Assert.AreEqual(-1, result);
    }
}