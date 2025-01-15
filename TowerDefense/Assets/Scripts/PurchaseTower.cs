using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseTower : MonoBehaviour
{
    [SerializeField] BaseTower towerType;

    private Image _image;
    private Button _button;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<int>("OnMoneyChanged", CheckIfAvailable);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<int>("OnMoneyChanged", CheckIfAvailable);
    }

    private void CheckIfAvailable(int value)
    {
        if (value < towerType.Cost)
        {
            _image.color = Color.gray;
            _button.interactable = false;
        }
        else
        {
            _image.color = Color.white;
            _button.interactable = true;
        }
    }

    public void Purchase()
    {
        EventBus.Publish("TowerSelected", towerType);
    }
}
