using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenButton : MonoBehaviour
{
    [SerializeField] private string menuScene;
    public void MenuButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(menuScene);
    }
}
