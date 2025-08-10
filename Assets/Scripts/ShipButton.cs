using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShipSelectButton : MonoBehaviour
{
    public TextMeshProUGUI label;
    private Ship assignedShip;
    private Action<Ship> onClickCallback;
    public Image overlayAssigned;

    public void Initialize(Ship ship, Action<Ship> callback)
    {
        assignedShip = ship;
        onClickCallback = callback;
        label.text = $"{ship.shipName}\nSpeed: {ship.speed}, Cargo: {ship.cargo}";

        if (ship.isAssigned)
        {
            overlayAssigned.gameObject.SetActive(true);
            GetComponent<Button>().interactable = false;
        }
        else
        {
            overlayAssigned.gameObject.SetActive(false);
            GetComponent<Button>().interactable = true;
        }
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => onClickCallback?.Invoke(assignedShip));
    }
}
