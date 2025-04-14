using UnityEngine;

public interface IBuildManager
{
    bool CanBuild { get; }
    bool HasMoney { get; }
    void SelectNode(Node node);
    TurretBlueprint GetTurretToBuild();
    //GameObject buildEffect { get; }
}
