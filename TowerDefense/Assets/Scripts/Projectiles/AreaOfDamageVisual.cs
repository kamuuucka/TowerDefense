using UnityEngine;

public class AreaOfDamageVisual : MonoBehaviour
{
    private float _duration = 2f;

    public void AssignDuration(float value)
    {
        _duration = value* 0.8f;
    }
    
    private void Start()
    {
        Destroy(gameObject, _duration);
    }
}