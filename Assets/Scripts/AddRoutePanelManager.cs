using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoutePanelManager : MonoBehaviour
{
    public GameObject panel;
    public Transform routeCardContainer;
    public GameObject routeOptionCardPrefab;
    public ShipSelectionPopup shipSelectionPopup;

    public RouteInfoPanelUI detailedRouteInfoUI;

    [Header("World Settings")]
    public int currentWorld = 1;
    private List<RouteData> currentRouteOptions = new();

    public void Open()
    {
        gameObject.SetActive(true);
        GenerateRouteOptions();
    }

    private void ClearOptions()
    {
        foreach (Transform child in routeCardContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void GenerateRouteOptions()
    {
        // Clear old cards
        ClearOptions();
        currentRouteOptions.Clear();

        for (int i = 0; i < 3; i++)
        {
            RouteData route = RouteGeneratorClass.GenerateRandomRoute(currentWorld);
            currentRouteOptions.Add(route);

            GameObject cardGO = Instantiate(routeOptionCardPrefab, routeCardContainer);
            RouteOptionCardUI cardUI = cardGO.GetComponent<RouteOptionCardUI>();
            cardUI.Initialize(route, this);
        }
    }

    private void ShowRouteDetails(RouteData route)
    {
        detailedRouteInfoUI.Open(route, shipSelectionPopup);
    }

    public void RerollRoutes()
    {
        // Spend gems, then regenerate
        GenerateRouteOptions();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        ClearOptions();
    }
}
