using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este Script nos permitira control sobre las escenas del juego, se podría crear un script Scenemanager y controlar las escenas desde acá
/// </summary>
public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }

    public GameObject[] StartPrefabs;
    public float TargetMissedPenalty = 1.0f;
    public AudioSource BGMPlayer;
    public AudioClip EndGameSound;
    public AudioClip GameOverSound;

    public int TargetCount => targetCount;
    public int DestroyedTarget => targetDestroyed;

    int targetCount;
    int targetDestroyed;

    void Awake()
    {
        Instance = this;
        foreach (var prefab in StartPrefabs)
        {
            Instantiate(prefab);
        }

        PoolSystem.Create();
    }

    public void FinishRun()
    {
        BGMPlayer.clip = EndGameSound;
        BGMPlayer.loop = false;
        BGMPlayer.Play();

        Controller.Instance.DisplayCursor(true);
        Controller.Instance.CanPause = false;
        //   FinalScoreUI.Instance.Display();
    }


    public void GameOVerRun()
    {
        BGMPlayer.clip = GameOverSound;
        BGMPlayer.loop = false;
        BGMPlayer.Play();

        Controller.Instance.DisplayCursor(true);
        Controller.Instance.CanPause = false;
    }

 
    public void TargetDestroyed(int score)
    {
        targetDestroyed += 1;
   
    }
}
