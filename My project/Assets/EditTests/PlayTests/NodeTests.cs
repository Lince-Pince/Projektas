using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.EventSystems;

public class NodeTests
{
    private GameObject nodeGO;
    private Node node;
    private GameObject buildManagerGO;
    private BuildManager buildManager;
    private GameObject turretPrefab;
    private GameObject buildEffectPrefab;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        foreach (var es in GameObject.FindObjectsOfType<EventSystem>())
            Object.Destroy(es.gameObject);

        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        turretPrefab = new GameObject("TestTurret");
        buildEffectPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        buildEffectPrefab.name = "TestBuildEffect";

        nodeGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        node = nodeGO.AddComponent<Node>();
        node.positionOffset = Vector3.up;
        node.hoverColor = Color.green;
        node.notEnoughMoneyColor = Color.red;
        nodeGO.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));

        buildManagerGO = new GameObject("BuildManager");
        buildManager = buildManagerGO.AddComponent<BuildManager>();
        BuildManager.instance = buildManager;
        buildManager.buildEffect = buildEffectPrefab;

        TurretBlueprint blueprint = new TurretBlueprint
        {
            prefab = turretPrefab,
            cost = 50
        };
        buildManager.SelectTurretToBuild(blueprint);
        PlayerStats.Money = 100;

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(nodeGO);
        Object.Destroy(buildManagerGO);
        Object.Destroy(buildEffectPrefab);
        yield return null;
    }

    // 🔟 Play Mode Tests

    [UnityTest]
    public IEnumerator Test1_BuildsTurretOnClick()
    {
        node.SendMessage("OnMouseDown");
        yield return null;
        Assert.IsNotNull(node.turret);
    }

    [UnityTest]
    public IEnumerator Test2_DoesNotBuildIfNotEnoughMoney()
    {
        PlayerStats.Money = 10;
        node.SendMessage("OnMouseDown");
        yield return null;
        Assert.IsNull(node.turret);
    }

    [UnityTest]
    public IEnumerator Test3_DoesNotBuildIfTurretAlreadyExists()
    {
        node.turret = new GameObject("ExistingTurret");
        node.SendMessage("OnMouseDown");
        yield return null;
        Assert.AreEqual("ExistingTurret", node.turret.name);
    }

    [UnityTest]
    public IEnumerator Test4_HoverColorAppliesIfEnoughMoney()
    {
        PlayerStats.Money = 100;
        node.SendMessage("OnMouseEnter");
        yield return null;
        Assert.AreEqual(Color.green, nodeGO.GetComponent<Renderer>().material.color);
    }

    [UnityTest]
    public IEnumerator Test5_NotEnoughMoneyColorApplies()
    {
        PlayerStats.Money = 0;
        node.SendMessage("OnMouseEnter");
        yield return null;
        Assert.AreEqual(Color.red, nodeGO.GetComponent<Renderer>().material.color);
    }

    [UnityTest]
    public IEnumerator Test6_ResetsColorOnMouseExit()
    {
        node.SendMessage("OnMouseEnter");
        yield return null;
        node.SendMessage("OnMouseExit");
        yield return null;
        Assert.AreEqual(Color.white, nodeGO.GetComponent<Renderer>().material.color);
    }

    [UnityTest]
    public IEnumerator Test7_IgnoresBuildIfNoTurretSelected()
    {
        buildManager.SelectTurretToBuild(null);
        node.SendMessage("OnMouseDown");
        yield return null;
        Assert.IsNull(node.turret);
    }

    [UnityTest]
    public IEnumerator Test8_StillBuildsIfPointerNotOverUI()
    {
        node.SendMessage("OnMouseDown");
        yield return null;
        Assert.IsNotNull(node.turret);
    }

    [UnityTest]
    public IEnumerator Test9_HoverStillAppliesWhenNotOverUI()
    {
        PlayerStats.Money = 100;
        node.SendMessage("OnMouseEnter");
        yield return null;
        Assert.AreEqual(Color.green, nodeGO.GetComponent<Renderer>().material.color);
    }

    [UnityTest]
    public IEnumerator Test10_GetBuildPositionIncludesOffset()
    {
        Vector3 expected = nodeGO.transform.position + node.positionOffset;
        Assert.AreEqual(expected, node.GetBuildPosition());
        yield return null;
    }

    // 🧪 Unit-Style Tests with Stub, Mock, Spy

    private class StubBuildManager : IBuildManager
    {
        public bool CanBuild => true;
        public bool HasMoney => true;
        public void BuildTurretOn(Node node) { /* do nothing */ }
    }

    [Test]
    public void Stub_ShouldAllowBuild()
    {
        var testNodeGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var testNode = testNodeGO.AddComponent<Node>();
        testNodeGO.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));

        testNode.SetBuildManager(new StubBuildManager());
        testNode.SendMessage("OnMouseDown");

        Assert.Pass("Stub allowed click without error.");
        Object.DestroyImmediate(testNodeGO);
    }

    private class MockBuildManager : IBuildManager
    {
        public bool CanBuild => true;
        public bool HasMoney => true;
        public bool Called = false;
        public void BuildTurretOn(Node node) => Called = true;
    }

    [Test]
    public void Mock_ShouldTrackBuildCall()
    {
        var testNode = new GameObject("MockNode").AddComponent<Node>();
        var mock = new MockBuildManager();
        testNode.SetBuildManager(mock);
        testNode.SendMessage("OnMouseDown");
        Assert.IsTrue(mock.Called, "Expected BuildTurretOn to be called.");
        Object.DestroyImmediate(testNode.gameObject);
    }

    private class SpyBuildManager : IBuildManager
    {
        public bool CanBuild => true;
        public bool HasMoney => true;
        public bool Called = false;
        public void BuildTurretOn(Node node)
        {
            node.turret = new GameObject("SpyTurret");
            Called = true;
        }
    }

    [Test]
    public void Spy_ShouldBuildTurretAndTrackCall()
    {
        var testNode = new GameObject("SpyNode").AddComponent<Node>();
        var spy = new SpyBuildManager();
        testNode.SetBuildManager(spy);
        testNode.SendMessage("OnMouseDown");
        Assert.IsTrue(spy.Called);
        Assert.IsNotNull(testNode.turret);
        Assert.AreEqual("SpyTurret", testNode.turret.name);
        Object.DestroyImmediate(testNode.gameObject);
    }
}
