using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PositionHandler : MonoBehaviour
{
    LeaderboardUIHandler leaderboardUIHandler;

    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();

    private void Awake()
    {
        
    }

    void Start()
    {
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();
        carLapCounters = carLapCounterArray.ToList<CarLapCounter>();

        foreach (CarLapCounter lapCounter in carLapCounters)
        {
            lapCounter.OnPassCheckpoint += OnPassCheckpoint;
        }

        leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();

        if (leaderboardUIHandler != null) 
        {
            leaderboardUIHandler.UpdateList(carLapCounters);
        }
    }

    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
        carLapCounters = carLapCounters.OrderByDescending(s => s.GetNumberOfCheckpointsPassed()).ThenBy(s => s.GetTimeAtLastCheckpoint()).ToList();

        int carPosition = carLapCounters.IndexOf(carLapCounter) + 1;

        carLapCounter.SetCarPosition(carPosition);

        if (leaderboardUIHandler != null)
            leaderboardUIHandler.UpdateList(carLapCounters);
    }
}