using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game")]
    [SerializeField] int requiredDeliveries = 3;

    [Header("UI")]
    [SerializeField] GameObject victoryScreen;

    int currentDeliveries;

    void Awake()
    {
        Instance = this;

        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
        }
    }

    public void AddDelivery()
    {
        currentDeliveries++;

        Debug.Log(
            "Deliveries: " +
            currentDeliveries +
            "/" +
            requiredDeliveries
        );

        if (currentDeliveries >= requiredDeliveries)
        {
            Victory();
        }
    }

    void Victory()
    {
        Debug.Log("VICTORY");

        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}