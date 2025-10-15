using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;
    private LeaderboardInfo[] leaderboardInfo;
    private CarLapCounter[] carLapCounterArray;

    void Start()
    {
        InitializeLeaderboard();
    }

    private void InitializeLeaderboard()
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        if (leaderboardLayoutGroup == null)
        {
            Debug.LogError("VerticalLayoutGroup não encontrado!");
            return;
        }

        if (leaderboardItemPrefab == null)
        {
            Debug.LogError("LeaderboardItemPrefab não está atribuído no Inspector!");
            return;
        }

        carLapCounterArray = FindObjectsOfType<CarLapCounter>();
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
}