using UnityEngine;

public class AreaOfDamageVisual : MonoBehaviour
{
    private float _duration = 2f;

    public void AssignDuration(float value)
    {
        _duration = value - value/2;
    }
    
    private void Start()
    {
        Destroy(gameObject, _duration);
    }
}