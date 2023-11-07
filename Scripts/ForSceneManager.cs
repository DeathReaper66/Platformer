using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForSceneManager : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

   public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SceneToLoad()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
