using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public GameObject pausePanel;
    public Button resumeButton;
    public Button saveButton;
    public Button menuButton;
    public TextMeshProUGUI slotButtonLabel;

    void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        saveButton.onClick.AddListener(Save);
        menuButton.onClick.AddListener(MenuClick);

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
        GameManager.Instance.SaveAllData();
        UpdateSlotButton(true);
    }

    private void MenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
