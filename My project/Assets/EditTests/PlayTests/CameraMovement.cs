using UnityEngine;

namespace MyProject.PlayTests
{
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

        // Public method for testing that calls the base Update()
        public void CallUpdate()
        {
            base.Update();
        }
    }
}
