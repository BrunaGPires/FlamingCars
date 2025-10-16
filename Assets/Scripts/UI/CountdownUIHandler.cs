using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUIHandler : MonoBehaviour
{
    public Text countDownText;

    void Awake()
    {
        countDownText.text = "";
        
    }
    void Start()
    {
        StartCoroutine(CountdownCO());
    }

    IEnumerator CountdownCO()
    {

        yield return new WaitForSeconds(0.3f);

        int counter = 3;

        while (true)
        {
            if (counter != 0)
            {
                countDownText.text = counter.ToString();
            }
            else
            {
                countDownText.text = "GO!";

                GameManager.instance.OnRaceStart();
                break;
            }

            counter--;
            yield return new WaitForSeconds(1.0f);
        }

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}
