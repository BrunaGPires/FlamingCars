using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject selectPanel;
    public Button firstTrackBT;
    public Button secondTrackBT;

    void Start()
    {
        selectPanel.SetActive(false);

        firstTrackBT.interactable = true;
        secondTrackBT.interactable = true;
    }

    public void OpenSelectPanel()
    {
        selectPanel.SetActive(true);
    }

    public void GoBackMenu()
    {
        selectPanel.SetActive(false);
    }

    public void PlayFirstTrack()
    {
        SceneManager.LoadScene("SecondTrack");
    }

    public void PlaySecondTrack()
    {
        SceneManager.LoadScene("ThirdTrack");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
