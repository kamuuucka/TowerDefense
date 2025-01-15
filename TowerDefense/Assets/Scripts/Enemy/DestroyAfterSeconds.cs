using UnityEngine;

/// <summary>
/// Destroys the object after specific amount of seconds.
/// </summary>
public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float time;
    private void Start()
    {
        Destroy(gameObject, time);
    }
    
}
