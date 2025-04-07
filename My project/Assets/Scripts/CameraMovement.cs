using UnityEngine;

public class CameraMovement : CameraController
{

    public bool simulatedGetKey;
    public bool simulatedGetKeyDown;
    public Vector3 simulatedMousePosition;
    public float simulatedAxis;

    protected override bool GetKey(string key)
    {
        return simulatedGetKey;
    }

    protected override bool GetKeyDown(KeyCode key)
    {
        return simulatedGetKeyDown;
    }

    protected override Vector3 GetMousePosition()
    {
        return simulatedMousePosition;
    }

    protected override float GetAxis(string axisName)
    {
        return simulatedAxis;
    }

    // Viešas metodas, skirtas testams, kuris iškviečia tėvinės klasės Update()
    public void CallUpdate()
    {
        base.Update();
    }
}
