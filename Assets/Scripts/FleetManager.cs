using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FleetManager : MonoBehaviour
{
    public List<Ship> fleet = new List<Ship>();
    private Ship currentShip;
    private List<Ship> fleetList;
    private int currentIndex;

    // UI References
    public Text shipNameText;
    public Text statsText;
    public Image shipImage;
    public Button prevButton;
    public Button nextButton;
    public Button speedUpgradeBtn;
    public Button combatUpgradeBtn;
    public Button cargoUpgradeBtn;
    public Button durabilityUpgradeBtn;

    public static FleetManager Instance { get; private set; }

    public List<Ship> GetUnassignedShips()
    {
        return fleet.FindAll(ship => !ship.isAssigned);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // prevents duplicate managers
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject); // Optional if you want it to persist across scenes
    }

    public Ship GetCurrentShip()
    {
        return fleet[currentIndex];
    }

    public List<Ship> GetAllShips()
    {
        return fleet;
    }
}
