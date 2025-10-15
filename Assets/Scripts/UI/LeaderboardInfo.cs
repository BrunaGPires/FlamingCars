using UnityEngine;
using TMPro;

public class LeaderboardInfo : MonoBehaviour
{
    public TMP_Text positionText;
    public TMP_Text driverNameText;

    void Start()
    {
        if (positionText == null)
            positionText = transform.Find("DriverPositionText")?.GetComponent<TMP_Text>();

        if (driverNameText == null)
            driverNameText = transform.Find("DriverNameText")?.GetComponent<TMP_Text>();
    }

    public void SetPositionText(string newPosition)
    {
        if (positionText != null)
            positionText.text = newPosition;
    }

    public void SetDriverNameText(string newDriverName)
    {
        if (driverNameText != null)
            driverNameText.text = newDriverName;
    }
}