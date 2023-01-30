using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGame : MonoBehaviour
{

    public GameObject weapon;
    public GameObject key;

    private void Awake()
    {
        gameObject.SetActive(true);
        weapon.SetActive(false);
        key.SetActive(false);
        Time.timeScale = 0;
        Controller.Instance.enabled = false;

    }

    public void Play()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        weapon.SetActive(true);
        key.SetActive(true);
        Controller.Instance.enabled = true;

    }
}
