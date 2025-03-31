public class EnemyMovement
{
    public int GetNextWaypointIndex(int currentIndex, int totalWaypoints)
    {
        if (currentIndex >= totalWaypoints - 1)
            return -1;

        return currentIndex + 1;
    }
}