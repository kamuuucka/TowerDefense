using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Manages the mouse input on the screen.
/// </summary>
public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject pointerPrefab;
    
    private Ray _ray;
    private RaycastHit _hit;
    private GameObject _pointer;
    private bool _isBuildMode = true;

    private void OnEnable()
    {
        EventBus.Subscribe<bool>("ModeSwitch", OnModeSwitch);
    }
    
    private void OnDisable()
    {
        EventBus.Unsubscribe<bool>("ModeSwitch", OnModeSwitch);
    }
    
    private void OnModeSwitch(bool isBuild)
    {
        _isBuildMode = isBuild;
    }

    private void Update()
    {
        if (!_isBuildMode)
        {
            if (_pointer != null) Destroy(_pointer.gameObject);
            return;
        }

        if (Camera.main != null) _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(_ray, out _hit))
        {
            Debug.Log(_hit.transform.gameObject.name);
            if (_hit.transform != null && _pointer == null)
            {
                _pointer = Instantiate(pointerPrefab, _hit.transform.position, Quaternion.identity);
            }
            else if (_hit.transform != null)
            {
                _pointer.transform.position = _hit.transform.position;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_hit.transform.childCount == 0)
                {
                    EventBus.Publish("TowerPlaced", _hit.transform);
                }
            }
            
        } 
        else if (_pointer != null)
        {
            Destroy(_pointer.gameObject);
        }

    }
}
