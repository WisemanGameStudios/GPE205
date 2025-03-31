using UnityEngine;

public class AutoUpright : MonoBehaviour
{
    public float uprightThreshold = 0.3f;
    public float correctionSpeed = 2f;

    void FixedUpdate()
    {
        Vector3 upDirection = transform.up;
        if (Vector3.Dot(upDirection, Vector3.up) < uprightThreshold)
        {
            Quaternion uprightRotation = Quaternion.FromToRotation(upDirection, Vector3.up) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, uprightRotation, Time.deltaTime * correctionSpeed);
        }
    }
}
