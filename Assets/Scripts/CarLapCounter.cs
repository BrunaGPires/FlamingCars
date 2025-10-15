using UnityEngine;
using System;

public class CarLapCounter : MonoBehaviour
{
    int passedCheckPointNumber = 0;
    float timeAtLastPassedCheckPoint = 0;

    int numberOfPassedCheckPoints = 0;

    int lapsCompleted = 0;
    const int lapsToComplete = 2;

    bool isRaceCompleted = false;

    int carPosition = 0;

    public event Action<CarLapCounter> OnPassCheckpoint;

    public void SetCarPosition(int position)
    {
        carPosition = position;
    }

    public int GetNumberOfCheckpointsPassed()
    {
        return numberOfPassedCheckPoints;
    }

    public float GetTimeAtLastCheckpoint()
    {
        return timeAtLastPassedCheckPoint;
    }

    void OnTriggerEnter2D(Collider2D collider2d)
    {
        if (collider2d.CompareTag("Checkpoint"))
        {
            if (isRaceCompleted) 
            {
                return;
            }

            Checkpoints checkPoint = collider2d.GetComponent<Checkpoints>();

            if (passedCheckPointNumber + 1 == checkPoint.checkPointNumber)
            {
                passedCheckPointNumber = checkPoint.checkPointNumber;

                numberOfPassedCheckPoints++;

                timeAtLastPassedCheckPoint = Time.time;

                if (checkPoint.isFinishLine) 
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;


                    if (lapsCompleted >= lapsToComplete)
                    {
                        isRaceCompleted = true;
                    }
                }

                OnPassCheckpoint?.Invoke(this);
            }
        }
    }
}
