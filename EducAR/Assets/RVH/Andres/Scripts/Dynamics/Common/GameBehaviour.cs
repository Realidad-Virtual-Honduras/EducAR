using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using MEC;

public class GameBehaviour : MonoBehaviour
{
    [SerializeField] float waitTime;

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
