using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryGame : MonoBehaviour
{
    public static VictoryGame Instance { get; protected set; }


    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void VictoryPlayer()
    {
        gameObject.SetActive(true);
        Controller.Instance.enabled = false;
        // GameSystem.Instance.GameOVerRun();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
