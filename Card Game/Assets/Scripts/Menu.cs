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

    [SerializeField] string nextLevelName;

    public void OnStartGameButton()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    public void OnSettingsButton()
    {
        settingsMenu.SetActive(true);
    }

    public void OnBackButton()
    {
        settingsMenu.SetActive(false);
    }
}
