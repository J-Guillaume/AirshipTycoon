using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetPageManager : MonoBehaviour
{
    public Transform shipListContainer;
    public GameObject shipCardPrefab;
    public ShipPanelUI detailsPanel;

    private List<Ship> allShips;

    public void OpenFleetPage()
    {
        gameObject.SetActive(true);
        LoadFleet();
    }

    private void LoadFleet()
    {
        foreach (Transform child in shipListContainer)
            Destroy(child.gameObject);

        allShips = FleetManager.Instance.GetAllShips();

        foreach (var ship in allShips)
        {
            var cardGO = Instantiate(shipCardPrefab, shipListContainer);
            cardGO.GetComponent<ShipSelectButton>().Initialize(ship, ShowShipDetails);
        }
    }

    private void ShowShipDetails(Ship ship)
    {
        detailsPanel.gameObject.SetActive(true);
    detailsPanel.ShowShip(ship, FleetManager.Instance.GetAllShips());
    }

    public void CloseFleetPage()
    {
        gameObject.SetActive(false);
    }
}
