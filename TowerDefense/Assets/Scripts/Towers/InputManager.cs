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
            // if(Input.GetMouseButtonDown(0))
            //     print(_hit.collider.name);
            Debug.Log(_hit.transform.name);
            
            if (_hit.transform != null && _pointer == null)
            {
                _pointer = Instantiate(pointer, _hit.transform.position, Quaternion.identity);
            }
            else if (_hit.transform != null)
            {
                _pointer.transform.position = _hit.transform.position;
            }
            
        } else if (_pointer != null)
        {
            //TODO: This does not work:((
            Destroy(_pointer.gameObject);
        }
    }
}
