using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class ShipSelectionPopup : MonoBehaviour
{
    public GameObject shipSelectButtonPrefab;
    public Transform shipButtonContainer;
    public GameObject popupPanel;
    public TextMeshProUGUI titleText;

    private Action<Ship> onShipSelected;

    public void Open(string title, List<Ship> unassignedShips, Action<Ship> callback)
    {
        popupPanel.SetActive(true);
        titleText.text = title;
        onShipSelected = callback;

        // Clear old buttons
        foreach (Transform child in shipButtonContainer)
            Destroy(child.gameObject);

        // Spawn a button for each ship
        foreach (Ship ship in unassignedShips)
        {
            GameObject buttonGO = Instantiate(shipSelectButtonPrefab, shipButtonContainer);
            ShipSelectButton button = buttonGO.GetComponent<ShipSelectButton>();
            button.Initialize(ship, HandleShipSelected);
        }
    }

    public void Close()
    {
        popupPanel.SetActive(false);
    }

    private void HandleShipSelected(Ship ship)
    {
        onShipSelected?.Invoke(ship);
        Close();
    }
}
