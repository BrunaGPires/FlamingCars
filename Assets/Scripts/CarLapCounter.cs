using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class CarLapCounter : MonoBehaviour
{
    public Text carPositionText;

    int passedCheckPointNumber = 0;
    float timeAtLastPassedCheckPoint = 0;

    int numberOfPassedCheckPoints = 0;

    int lapsCompleted = 0;
    const int lapsToComplete = 2;

    bool isRaceCompleted = false;

    int carPosition = 0;

    bool isHideRoutineRunning = false;
    float hideUIDelayTime;

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

    IEnumerator ShowPositionCO(float delayUntilHidePosition)
    {
        hideUIDelayTime = delayUntilHidePosition;

        carPositionText.text = carPosition.ToString();

        carPositionText.gameObject.SetActive(true);

        if (!isHideRoutineRunning)
        {
            isHideRoutineRunning = true;

            yield return new WaitForSeconds(hideUIDelayTime);

            carPositionText.gameObject.SetActive(false);

            isHideRoutineRunning = false;
        }

        
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

                if (isRaceCompleted)
                {
                    StartCoroutine(ShowPositionCO(100));

                    if (CompareTag("Car"))
                    {
                        GameManager.instance.OnRaceCompleted();
                        GetComponent<CarInputHandler>().enabled = false;
                    }
                }
                else if (checkPoint.isFinishLine)
                {
                    StartCoroutine(ShowPositionCO(1.5f));
                }
            }
        }
    }
}
