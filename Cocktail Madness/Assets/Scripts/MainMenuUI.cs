using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private List<Scene> scenes = new List<Scene>();

    private void Start()
    {
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            scenes.Add(SceneManager.GetSceneAt(i));
        }
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
