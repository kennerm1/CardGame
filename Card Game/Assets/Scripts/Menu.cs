using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainScreen;

    public Button settingsButton;
    public Button backButton;

    [SerializeField] string _nextLevelName;

    public void onStartGameButton()
    {
        SceneManager.LoadScene(_nextLevelName);
    }

    public void onSettingsButton()
    {
        settingsMenu.SetActive(true);
    }

    public void onBackButton()
    {
        settingsMenu.SetActive(false);
    }
}
