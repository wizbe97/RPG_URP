using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    [Header("PAUSE")]
    public GameObject pausePanel;
    public Button resumeButton;
    public Button saveButton;
    public Button menuButton;
    public TextMeshProUGUI slotButtonLabel;

    [Header("CONFIRM")]
    public GameObject confirmPanel;
    public Button backButton;
    public Button saveAndQuitButton;
    public Button menuConfirmButton;

    void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        saveButton.onClick.AddListener(Save);
        menuButton.onClick.AddListener(MenuClick);
        menuConfirmButton.onClick.AddListener(MenuConfirmClick);
        backButton.onClick.AddListener(ConfirmBackClick);
        saveAndQuitButton.onClick.AddListener(SaveAndQuit);

        UpdateSlotButton(false);
    }

    private void UpdateSlotButton(bool isSaved)
    {
        slotButtonLabel.text = isSaved ? "SAVED" : "SAVE";
        saveButton.interactable = !isSaved;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        UpdateSlotButton(false);
        pausePanel.SetActive(true);
    }

    private void Save()
    {
        GameManager.Instance.SaveAllData(isLocal: false);
        UpdateSlotButton(true);
    }

    private void MenuClick()
    {
        if (saveButton.interactable)
        {
            pausePanel.SetActive(false);
            confirmPanel.SetActive(true);
            return;
        }

        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    private void MenuConfirmClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    private void ConfirmBackClick()
    {
        confirmPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    private void SaveAndQuit()
    {
        GameManager.Instance.SaveAllData(isLocal: false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
