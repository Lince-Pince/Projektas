using UnityEngine;
using System.Collections.Generic;

public class WaypointPath
{
    public List<string> waypointNames;

    public WaypointPath(IEnumerable<string> names)
    {
        waypointNames = new List<string>(names);
    }

    public int GetWaypointCount()
    {
        return waypointNames.Count;
    }

    public string GetWaypointAt(int index)
    {
        if (index < 0 || index >= waypointNames.Count)
            throw new System.IndexOutOfRangeException("Invalid waypoint index");
        return waypointNames[index];
    }
}
