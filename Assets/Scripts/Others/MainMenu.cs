using System.Collections;
using DG.Tweening;
using UnityEngine;

// Manager care se ocupa de functionalitatiile meniului principal
public class MainMenu : MonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private GameObject[] mainMenuButtons;
    [SerializeField] private GameObject[] levelButtons;
    [SerializeField] private GameObject settingsMenu;


    private void Start()
    {
        foreach (GameObject button in mainMenuButtons)
        {
            button.transform.localScale = Vector3.zero;
        }

        foreach (GameObject button in levelButtons)
        {
            button.transform.localScale = Vector3.zero;
        }

        StartCoroutine(ShowMainMenuButtons());
        settingsMenu.SetActive(false);
    }
    public void GoToMainMenu()
    {
        background.DOSizeDelta(new(350f, 400f), 2f);
        StartCoroutine(ShowMainMenuButtons());
        settingsMenu.SetActive(false);
    }

    public void OnSettingsButtonClicked()
    {
        foreach (GameObject button in mainMenuButtons)
        {
            button.transform.localScale = Vector3.zero;
        }

        background.DOSizeDelta(new(500f, 500f), 2f).OnComplete(() =>
        {
            settingsMenu.SetActive(true);
        });
    }

    public void ShowLevels()
    {
        StartCoroutine(ShowLevelButtons());
    }

    public void ExitTheGame()
    {
        Application.Quit();
    }

    private IEnumerator ShowMainMenuButtons()
    {
        foreach (GameObject button in mainMenuButtons)
        {
            button.transform.DOScale(1f, 1f);

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ShowLevelButtons()
    {
        foreach (GameObject button in levelButtons)
        {
            button.transform.DOScale(1f, 1f);

            yield return new WaitForSeconds(1f);
        }
    }
}
