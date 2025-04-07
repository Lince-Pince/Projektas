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
        GameManager.GameIsOver = false; // arba suteikite tinkamą reikšmę
        camObject = new GameObject("TestCamera");
        camMovement = camObject.AddComponent<CameraMovement>();
        camObject.transform.position = Vector3.zero;
    }


    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(camObject);
    }


    [UnityTest]
    public IEnumerator CameraDoesNotMoveWhenDoMovementIsDisabled()
    {
        // Simuliuojame paspaudimą Escape, kad išjungtume judėjimą
        camMovement.simulatedGetKeyDown = true;
        camMovement.CallUpdate();  // Vietoj Update() kvietimo
        camMovement.simulatedGetKeyDown = false;

        // Nustatome, kad jokie kiti veiksmai neįvyktų
        camMovement.simulatedGetKey = false;
        camMovement.simulatedMousePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        camMovement.simulatedAxis = 0f;

        Vector3 initialPos = camObject.transform.position;
        yield return null;
        camMovement.CallUpdate();
        yield return null;
        Assert.AreEqual(initialPos, camObject.transform.position);
    }

   
}
