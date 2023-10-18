using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float minDistance = 1.0f; // 最近的相机距离
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float pitchSpeed = 2;
    public float yawSpeed = 2;
    public LayerMask collisionMask; // 用于相机碰撞检测的层
    private float pitch = 0;
    private float yaw = 0;

    private void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * yawSpeed;
        pitch -= Input.GetAxis("Mouse Y") * pitchSpeed;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        transform.eulerAngles = targetRotation;

        // 防止相机穿墙
        float adjustedDistance = CheckCameraCollision(distance);
        transform.position = target.position - transform.forward * adjustedDistance;
    }

    private float CheckCameraCollision(float targetDistance)
    {
        if (Physics.Raycast(target.position, -transform.forward, out RaycastHit hit, distance, collisionMask))
        {
            return Mathf.Clamp(hit.distance, minDistance, distance);
        }
        return targetDistance;
    }
}
