using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShipPanelUI : MonoBehaviour
{
    [Header("UI References")]
    public Text shipNameText;
    public Image shipImage;
    public TextMeshProUGUI statsText;
    public Button upgradeSpeedButton;
    public Button upgradeCargoButton;
    public Button upgradeCombatButton;
    public Button upgradeDurabilityButton;

    public List<Ship> fleet = new List<Ship>();
    private Ship currentShip;
    private List<Ship> fleetList;
    private int currentIndex;


    public void ShowShip(Ship ship, List<Ship> allShips)
    {
        currentShip = ship;

        // Update image and text
        shipImage.sprite = ship.shipSprite;
        statsText.text = $"Name: {ship.shipName}\n" +
                         $"Speed: {ship.speed}\n" +
                         $"Cargo: {ship.cargo}\n" +
                         $"Combat Power: {ship.combatPower}\n" +
                         $"Durability: {ship.durability}/{ship.maxDurability}";
    }

    private void Awake()
    {
        upgradeSpeedButton.onClick.AddListener(() => UpgradeAndRefresh(() => currentShip.UpgradeSpeed()));
        upgradeCargoButton.onClick.AddListener(() => UpgradeAndRefresh(() => currentShip.UpgradeCargo()));
        upgradeCombatButton.onClick.AddListener(() => UpgradeAndRefresh(() => currentShip.UpgradeCombat()));
        upgradeDurabilityButton.onClick.AddListener(() => UpgradeAndRefresh(() => currentShip.UpgradeDurability()));
    }

    private void UpgradeAndRefresh(System.Action upgradeAction)
    {
        upgradeAction?.Invoke();
        ShowShip(currentShip, fleetList);
    }

    void Upgrade(string attribute)
    {
        Ship current = fleet[currentIndex];

        switch (attribute)
        {
            case "speed": current.UpgradeSpeed(); break;
            case "combat": current.UpgradeCombat(); break;
            case "cargo": current.UpgradeCargo(); break;
            case "durability": current.UpgradeDurability(); break;
        }

    }

    public void Open(Ship ship, List<Ship> fleet)
    {
        gameObject.SetActive(true);
        currentShip = ship;
        fleetList = fleet;
        currentIndex = fleetList.IndexOf(ship);
        UpdateUI();
    }

    public void UpdateUI()
    {
        Ship current = fleet[currentIndex];
        if (current == null) return;
        shipNameText.text = current.shipName;
        statsText.text = $"Speed: {current.speed} | Combat: {current.combatPower}\nCargo: {current.cargo} | Durability: {current.durability}/{current.maxDurability}";

        // You can swap the ship sprite later based on index/name
        // shipImage.sprite = ...
    }

    public void ShowPreviousShip()
    {
        if (fleetList == null || fleetList.Count == 0) return;
        currentIndex = (currentIndex - 1 + fleetList.Count) % fleetList.Count;
        ShowShip(fleetList[currentIndex], fleetList);
    }

    public void ShowNextShip()
    {
        if (fleetList == null || fleetList.Count == 0) return;
        currentIndex = (currentIndex + 1) % fleetList.Count;
        ShowShip(fleetList[currentIndex], fleetList);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
