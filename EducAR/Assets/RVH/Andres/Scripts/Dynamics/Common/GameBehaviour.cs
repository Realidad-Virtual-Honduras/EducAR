using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using MEC;
using UnityEngine.Rendering;

public class GameBehaviour : MonoBehaviour
{
    public static GameBehaviour instance;

    [SerializeField] float waitTime;

    private void Awake()
    {
        instance = this;
    }

    public void GameInitialize()
    {
        LoaderUtility.Initialize();
    }

    public void GameDeinitialize()
    {
        LoaderUtility.Deinitialize();
    }

    public void GameRestart()
    {
        Timing.RunCoroutine(RestartGame());
    }

    private IEnumerator<float> RestartGame()
    {
        GameDeinitialize();
        yield return Timing.WaitForSeconds(waitTime);
        GameInitialize();
    }
}
