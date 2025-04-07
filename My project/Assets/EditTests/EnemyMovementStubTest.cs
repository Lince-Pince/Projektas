using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyMovementStubTest
{
    //------------------Stub test-------------------------
    [UnityTest]
    public IEnumerator EnemyPasiekiaPaskutiniWaypointIrSumazinaGyvybes()
    {
        // Sukuriam 3 stub waypoint'us
        var waypoint1 = new GameObject("StubWaypoint1").transform;
        waypoint1.position = new Vector3(0, 0, 0);

        var waypoint2 = new GameObject("StubWaypoint2").transform;
        waypoint2.position = new Vector3(0, 0, 5);

        var waypoint3 = new GameObject("StubWaypoint3").transform;
        waypoint3.position = new Vector3(0, 0, 10);

        Waypoints.points = new Transform[] { waypoint1, waypoint2, waypoint3 };

        // Nustatome PlayerStats.Lives
        PlayerStats.Lives = 5;

        // Sukuriame priešą
        var enemyGO = new GameObject("StubEnemy");
        enemyGO.transform.position = waypoint1.position;
        var enemy = enemyGO.AddComponent<Enemy>();
        enemy.speed = 10f;
        enemy.startSpeed = 10f;

        enemyGO.AddComponent<EnemyMovementt>();

        // Laukiame tol, kol enemy bus sunaikintas (pasieks paskutinį waypoint) arba timeout pasibaigs
        float timeout = 10f; // padidinta timeout
        while (enemyGO != null && timeout > 0f)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }


        // Patikriname, ar PlayerStats.Lives sumažėjo (turėtų būti 4)
        Assert.AreEqual(4, PlayerStats.Lives, "Gyvybės nebuvo sumažintos pasiekus paskutinį waypoint.");

        // Patikriname, ar priešas buvo sunaikintas
        Assert.IsTrue(enemyGO == null || enemyGO.Equals(null), "Priešas nebuvo sunaikintas, kai pasiektas galutinis waypoint'as.");

        // Išvalome sukurtus objektus
        Object.Destroy(waypoint1.gameObject);
        Object.Destroy(waypoint2.gameObject);
        Object.Destroy(waypoint3.gameObject);

        yield return null;
    }
}

