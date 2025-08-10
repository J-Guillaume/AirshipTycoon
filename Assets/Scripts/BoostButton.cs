using UnityEngine;
using UnityEngine.UI;
using System;

public class BoostButton : MonoBehaviour
{
    public Button button;
    public event Action OnBoostClicked;

    private void Awake()
    {
        button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        OnBoostClicked?.Invoke();
    }
}
