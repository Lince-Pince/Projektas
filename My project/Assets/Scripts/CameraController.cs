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

    protected virtual void Update()
    {
        if (GameManager.GameIsOver)
        {
            this.enabled = false;
            return;
        }

        if (Input.GetKeyDown("escape"))
            doMovement = !doMovement;

        if (!doMovement)
            return;

        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
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
