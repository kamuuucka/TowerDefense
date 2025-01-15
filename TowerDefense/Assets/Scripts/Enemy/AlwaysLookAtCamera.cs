using UnityEngine;

/// <summary>
/// Makes sure that the transform always is pointed at the camera
/// </summary>
public class AlwaysLookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }
}
