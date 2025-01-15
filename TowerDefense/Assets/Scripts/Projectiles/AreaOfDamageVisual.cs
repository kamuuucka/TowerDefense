using UnityEngine;

/// <summary>
/// Provides a visual for the AreaOfDamageAttack
/// </summary>
public class AreaOfDamageVisual : MonoBehaviour
{
    private float _duration = 2f;
    
    private void Start()
    {
        Destroy(gameObject, _duration);
    }
    
    /// <summary>
    /// Assigns duration of the whole attack cycle. 80% of it is the actual attack, and 20% is the waiting time in between.
    /// </summary>
    /// <param name="value">Value of the whole attack cycle.</param>
    public void AssignDuration(float value)
    {
        _duration = value * 0.8f;
    }
}
