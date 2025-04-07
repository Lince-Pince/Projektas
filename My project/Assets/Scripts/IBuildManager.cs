public interface IBuildManager
{
    bool CanBuild { get; }
    bool HasMoney { get; }
    void BuildTurretOn(Node node);
}
