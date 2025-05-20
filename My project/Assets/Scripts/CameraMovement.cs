using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private bool doMovement = true;

    public float panSpeed = 30f;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 5f;
    public float minY = 10f;
    public float maxY = 80f;

    // [MOCK] Test input simulation flags and values
    public bool simulateInput = false;
    public bool simulatedGetKeyDown = false;
    public bool simulatedGetKey = false;
    public Vector3 simulatedMousePosition = Vector3.zero;
    public float simulatedAxis = 0f;

    // [MOCK] Allow test scripts to call Update()
    public void CallUpdate()
    {
        Update();
    }

    protected virtual void Update()
    {
        if (GameManager.GameIsOver)
        {
            this.enabled = false;
            return;
        }

        if (GetKeyDown("escape"))
            doMovement = !doMovement;

        if (!doMovement)
            return;

        if (GetKey("w") || GetKey("up"))
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (GetKey("s") || GetKey("down"))
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        if (GetKey("d") || GetKey("right"))
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (GetKey("a") || GetKey("left"))
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        float scroll = GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    // [MOCK] Overridable input methods return simulated values when testing
    protected virtual bool GetKey(string key)
    {
        return simulateInput ? simulatedGetKey : Input.GetKey(key);  // [MOCK]
    }

    protected virtual bool GetKeyDown(string key)
    {
        return simulateInput && key == "escape" ? simulatedGetKeyDown : Input.GetKeyDown(key);  // [MOCK]
    }

    protected virtual float GetAxis(string axisName)
    {
        return (simulateInput && axisName == "Mouse ScrollWheel") ? simulatedAxis : Input.GetAxis(axisName);  // [MOCK]
    }
}
