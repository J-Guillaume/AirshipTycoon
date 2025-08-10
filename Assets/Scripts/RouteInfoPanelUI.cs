using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class RouteInfoPanelUI : MonoBehaviour
{
    public TextMeshProUGUI titleText, durationText, combatPowerText;
    public Transform manifestContainer;
    public GameObject cargoEntryPrefab;
    public TextMeshProUGUI shipEffectText;
    public Toggle autoRerunToggle;
    public Button assignRouteButton;
    public Button selectShipButton;

    private RouteData currentRoute;
    private Ship selectedShip;
    private Action<RouteData, Ship, bool> onRouteAssigned;

    public void Open(RouteData route, Action<RouteData, Ship, bool> onAssignCallback)
    {
        gameObject.SetActive(true);
        currentRoute = route;
        selectedShip = null;
        onRouteAssigned = onAssignCallback;

        titleText.text = route.routeTitle;
        durationText.text = $"Base Duration: {FormatDuration(route.BaseDuration)}";
        combatPowerText.text = $"Recommended CP: {route.recommendedCombatPower}";

        // Clear old manifest entries
        foreach (Transform child in manifestContainer)
            Destroy(child.gameObject);

        // Populate manifest
        foreach (var entry in route.cargoManifest)
        {
            var entryUI = Instantiate(cargoEntryPrefab, manifestContainer).GetComponent<CargoItem>();
            entryUI.Initialize(entry);
        }

        shipEffectText.text = "No ship selected";
        assignRouteButton.interactable = false;

        // Hook up buttons
        selectShipButton.onClick.RemoveAllListeners();
        selectShipButton.onClick.AddListener(OpenShipSelection);

        assignRouteButton.onClick.RemoveAllListeners();
        assignRouteButton.onClick.AddListener(AssignRoute);
    }

    private void OpenShipSelection()
    {
        HomePageManager.Instance.OpenShipSelection("Select a ship for this route", OnShipSelected);
    }

    private void OnShipSelected(Ship ship)
    {
        selectedShip = ship;

        // Adjust duration based on speed
        TimeSpan adjusted = TimeSpan.FromTicks((long)(currentRoute.BaseDuration.Ticks / (1 + (ship.speed * 0.01f))));

        durationText.text = $"Base Duration: {FormatDuration(currentRoute.BaseDuration)} ({FormatDuration(adjusted)})";

        int runs = Mathf.Max(1, ship.durability / currentRoute.durabilityCost);
        shipEffectText.text = $"This ship can make {runs} runs ({FormatDuration(adjusted * runs)}) before breaking down.";

        assignRouteButton.interactable = true;
    }

    private void AssignRoute()
    {
        onRouteAssigned?.Invoke(currentRoute, selectedShip, autoRerunToggle.isOn);
        gameObject.SetActive(false);
    }

    private string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalDays >= 1)
            return $"{(int)duration.TotalDays}d {(int)duration.Hours}h {(int)duration.Minutes}m";
        else if (duration.TotalHours >= 1)
            return $"{(int)duration.TotalHours}h {(int)duration.Minutes}m";
        else
            return $"{(int)duration.TotalMinutes}m";
    }

    internal void Open(RouteData route, ShipSelectionPopup shipSelectionPopup)
    {
        throw new NotImplementedException();
    }
}
