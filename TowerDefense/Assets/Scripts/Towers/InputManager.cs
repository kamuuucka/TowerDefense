using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    
    
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

    void Update()
    {
        //TODO: When the tower is placed, can't place around them.
        //TODO: Player can buy few towers at once. Make them click the button every time.
        if (!_isBuildMode)
        {
            if (_pointer != null) Destroy(_pointer.gameObject);
            return;
        }
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
            
        } 
        else if (_pointer != null)
        {
            Destroy(_pointer.gameObject);
        }

    }
}
