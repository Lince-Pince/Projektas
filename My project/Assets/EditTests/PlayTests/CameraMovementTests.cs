using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class CameraMovementTests
{
    private GameObject camObject;
    private CameraMovement camMovement;

    [SetUp]
    public void Setup()
    {
        GameManager.GameIsOver = false;
        camObject = new GameObject("TestCamera");
        camMovement = camObject.AddComponent<CameraMovement>();
        camObject.transform.position = Vector3.zero;

        camMovement.simulateInput = true; // [MOCK] Enable input simulation
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(camObject);
    }

    [UnityTest]
    public IEnumerator CameraDoesNotMoveWhenDoMovementIsDisabled()
    {
        // [MOCK] Simulate Escape key press to toggle movement off
        camMovement.simulatedGetKeyDown = true;
        camMovement.CallUpdate();  // [MOCK] Calling Update manually
        camMovement.simulatedGetKeyDown = false;

        // [MOCK] Simulate no input
        camMovement.simulatedGetKey = false;
        camMovement.simulatedMousePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        camMovement.simulatedAxis = 0f;

        Vector3 initialPos = camObject.transform.position;
        yield return null;
        camMovement.CallUpdate();  // [MOCK]
        yield return null;

        Assert.AreEqual(initialPos, camObject.transform.position);
    }

}
