using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject btn_Continue;

    [Header("Persistent Objects")]
    [SerializeField] private GameObject[] persistentObjects = null;

    public bool activeContinue = true;

    [Header("Directories")]
    public string menusDirections = "RVH/Andres/Scenes/Menus/";
    public string classesDirections = "RVH/Andres/Scenes/Levels/";

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;

        ActiveContinue(false);

        foreach (GameObject obj in persistentObjects)
            Object.DontDestroyOnLoad(obj);

        LoadScene(menusDirections +"BgMenu");
    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ActiveContinue(bool active)
    {
        activeContinue = active;
        btn_Continue.SetActive(active);
    }
}
