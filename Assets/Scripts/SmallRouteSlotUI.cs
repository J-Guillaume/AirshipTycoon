using UnityEngine;

public class SmallRouteSlotUI : MonoBehaviour
{
    public ProgressBarUI progressBarUI;
    public BoostButton boostButton;
    private bool initialized = false;
    private Ship assignedShip;
    public GameObject collectRewardButton;
    public bool IsActive => initialized;
    private bool autoRerun = false;
    [SerializeField] private int routeReward = 250;
    private float routeDuration;



    public void Initialize(Ship ship, float duration, bool rerunEnabled, int reward)
    {
        assignedShip = ship;
        assignedShip.isAssigned = true;

        autoRerun = rerunEnabled;
        routeReward = reward;
        routeDuration = duration;

        progressBarUI.Initialize(ship.shipSprite, duration, rerunEnabled);
        boostButton.OnBoostClicked += ApplyBoost;

        initialized = true;
        gameObject.SetActive(true);
        collectRewardButton.SetActive(false);
    }


    public void Tick()
    {
        if (!initialized) return;

        progressBarUI.UpdateTimer();

        if (progressBarUI.IsRouteComplete())
        {
            OnRouteComplete();
        }
    }

    public void ApplyBoost()
    {
        progressBarUI.ApplyBoost();
    }

    private void OnRouteComplete()
    {
        assignedShip.durability--;

        if (autoRerun && assignedShip.durability > 0)
        {
            GameManager.Instance.AddCoins(routeReward);
            progressBarUI.Initialize(assignedShip.shipSprite, routeDuration, autoRerun);
        }
        else
        {
            assignedShip.isAssigned = false;

            if (autoRerun)
                GameManager.Instance.AddCoins(routeReward);
            else
                collectRewardButton.SetActive(true);
        }
    }    
}
