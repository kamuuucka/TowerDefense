using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    
    
    private Ray _ray;
    private RaycastHit _hit;
    private GameObject _pointer;

    void Update()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(_ray, out _hit))
        {
            if (_hit.transform != null && _pointer == null)
            {
                _pointer = Instantiate(pointer, _hit.transform.position, Quaternion.identity);
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
            
        } else if (_pointer != null)
        {
            Destroy(_pointer.gameObject);
        }
    }
}