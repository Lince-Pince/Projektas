using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private bool doMovement = true;

    public float panSpeed = 30f;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 5f;
    public float minY = 10f;
    public float maxY = 80f;

    // Deklaruojame Update() kaip protected virtual, kad jį būtų galima perrašyti ir iškviesti iš paveldėtų klasių.
    protected virtual void Update()
    {
        if (GameManager.GameIsOver)
        {
            this.enabled = false;
            return;
        }

        if (GetKeyDown(KeyCode.Escape))
            doMovement = !doMovement;

        if (!doMovement)
            return;

        if (GetKey("w") || GetMousePosition().y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (GetKey("s") || GetMousePosition().y <= panBorderThickness)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        if (GetKey("d") || GetMousePosition().x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (GetKey("a") || GetMousePosition().x <= panBorderThickness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        float scroll = GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    // Virtualūs metodai, kuriuos galima perrašyti testavimo tikslais
    protected virtual bool GetKey(string key)
    {
        return Input.GetKey(key);
    }

    protected virtual bool GetKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    protected virtual Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    protected virtual float GetAxis(string axisName)
    {
        return Input.GetAxis(axisName);
    }
}
