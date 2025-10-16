using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;
    private LeaderboardInfo[] leaderboardInfo;

    bool isInitialized = false;
    Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();

        GameManager.instance.OnGameStateChanged += OnGameStateChanged;

    }

    void Start()
    {
        InitializeLeaderboard();
    }

    private void InitializeLeaderboard()
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();
        leaderboardInfo = new LeaderboardInfo[carLapCounterArray.Length];

        Debug.Log($"Encontrados {carLapCounterArray.Length} carros para o leaderboard");

        foreach (Transform child in leaderboardLayoutGroup.transform)
        {
            if (child.gameObject.activeInHierarchy && child.GetComponent<LeaderboardInfo>() != null)
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < carLapCounterArray.Length; i++)
        {
            GameObject leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);
            leaderboardInfoGameObject.SetActive(true);

            leaderboardInfo[i] = leaderboardInfoGameObject.GetComponent<LeaderboardInfo>();

            if (leaderboardInfo[i] != null)
            {
                leaderboardInfo[i].SetPositionText($"{i + 1}.");
                leaderboardInfo[i].SetDriverNameText("Aguardando...");
            }
        }

        Canvas.ForceUpdateCanvases();
        isInitialized = true;
    }

    public void UpdateList(List<CarLapCounter> lapCounters)
    {
        if (leaderboardInfo == null || lapCounters == null) return;

        for (int i = 0; i < lapCounters.Count && i < leaderboardInfo.Length; i++)
        {
            if (leaderboardInfo[i] != null && lapCounters[i] != null)
            {
                leaderboardInfo[i].SetPositionText($"{i + 1}.");
                leaderboardInfo[i].SetDriverNameText(lapCounters[i].gameObject.name);
            }
        }
    }

    void OnGameStateChanged(GameManager gameManager)
    {
        if (GameManager.instance.GetGameStates() == GameStates.raceOver)
        {
            canvas.enabled = true;
        }
    }

    void OnDestroy()
    {
        GameManager.instance.OnGameStateChanged -= OnGameStateChanged;
    }
}