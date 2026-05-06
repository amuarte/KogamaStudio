using UnityEngine;

namespace KogamaStudio.Camera;

public static class Freecam
{
    public static bool IsEnabled { get; private set; }

    public static float Speed = 20f;
    public static float Sensitivity = 2f;
    public static bool RequireRightClick = true;

    private static float _yaw;
    private static float _pitch;
    private static Vector3 _position;
    private static bool _initialized;

    public static void Enable() { IsEnabled = true; _initialized = false; }
    public static void Disable() { IsEnabled = false; _initialized = false; }

    public static void LateUpdate()
    {
        if (!IsEnabled) return;

        var mgr = MVGameControllerBase.MainCameraManager;
        if (mgr == null) return;

        var pt = mgr.ProtectedTransform;
        if (pt == null) return;

        if (!_initialized)
        {
            _position = pt.position;
            var euler = pt.rotation.eulerAngles;
            _yaw = euler.y;
            _pitch = euler.x > 180f ? euler.x - 360f : euler.x;
            _initialized = true;
            return;
        }

        bool canRotate = !RequireRightClick || Input.GetMouseButton(1);
        if (canRotate)
        {
            _yaw += Input.GetAxis("Mouse X") * Sensitivity;
            _pitch -= Input.GetAxis("Mouse Y") * Sensitivity;
            _pitch = Mathf.Clamp(_pitch, -89f, 89f);
        }

        var rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        float speed = Speed * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 3f : 1f);

        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move += rotation * Vector3.forward;
        if (Input.GetKey(KeyCode.S)) move -= rotation * Vector3.forward;
        if (Input.GetKey(KeyCode.D)) move += rotation * Vector3.right;
        if (Input.GetKey(KeyCode.A)) move -= rotation * Vector3.right;
        if (Input.GetKey(KeyCode.E)) move += Vector3.up;
        if (Input.GetKey(KeyCode.Q)) move -= Vector3.up;

        if (move.sqrMagnitude > 0f)
            _position += move.normalized * speed;

        pt.position = _position;
        pt.rotation = rotation;
    }
}