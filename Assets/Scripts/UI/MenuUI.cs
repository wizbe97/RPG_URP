using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("MENU ITEMS")]
    public GameObject menuPanel;
    public Button newGameButton;
    public Button loadGameButton;

    [Header("NEW GAME SLOT")]
    public GameObject chooseSlotPanel;
    public Button chooseSlotBackButton;
    public Button[] chooseSlotButtons;
    public TextMeshProUGUI[] chooseSlotLabels;


    [Header("LOAD GAME ITEMS")]
    public Button loadGameBackButton;
    public GameObject loadGamePanel;
    public Button[] loadSlotButtons;
    public Button[] removeSlotButtons;

    void Start()
    {
        loadGameBackButton.onClick.AddListener(LoadGameBackClick);
        chooseSlotBackButton.onClick.AddListener(ChooseSlotBackClick);
        newGameButton.onClick.AddListener(StartNewGame);
        loadGameButton.onClick.AddListener(LoadGameClick);

        for (int i = 0; i < loadSlotButtons.Length; i++)
        {
            int slot = i + 1;
            loadSlotButtons[i].onClick.AddListener(() => OnSlotButtonClicked(slot));
            removeSlotButtons[i].onClick.AddListener(() => OnRemoveSlotClicked(slot));
        }

        for (int i = 0; i < chooseSlotButtons.Length; i++)
        {
            int slot = i + 1;
            chooseSlotButtons[i].onClick.AddListener(() => OnChooseSlotClicked(slot));
        }

        UpdateSlotButtons();
        UpdateChooseSlotButtons();
    }

    private void UpdateSlotButtons()
    {
        for (int i = 0; i < loadSlotButtons.Length; i++)
        {
            int slot = i + 1;
            if (SaveManager.Instance.IsDataSaved(slot))
            {
                removeSlotButtons[i].gameObject.SetActive(true);
                loadSlotButtons[i].interactable = true;
                loadGameButton.interactable = true;
            }
            else
            {
                removeSlotButtons[i].gameObject.SetActive(false);
                loadSlotButtons[i].interactable = false;
            }
        }
    }
    private void UpdateChooseSlotButtons()
    {
        for (int i = 0; i < chooseSlotButtons.Length; i++)
        {
            int slot = i + 1;
            if (SaveManager.Instance.IsDataSaved(slot))
            {
                chooseSlotLabels[i].text = "Override";
            }
            else
            {
                chooseSlotLabels[i].text = "EMPTY";
            }
        }
    }

    private void OnSlotButtonClicked(int slot)
    {
        if (SaveManager.Instance.IsDataSaved(slot))
        {
            Load(slot);
        }
        UpdateSlotButtons();
    }
    private void OnChooseSlotClicked(int slot)
    {
        OverrideSlot(slot);
    }

    private void OnRemoveSlotClicked(int slot)
    {
        GameManager.Instance.RemoveSlot(slot);
        UpdateSlotButtons();
    }

    private void Load(int slot)
    {
        GameManager.Instance.currentSlot = slot;
        GameManager.Instance.StartGame();
    }

    private void OverrideSlot(int slot)
    {
        GameManager.Instance.RemoveSlot(slot);
        GameManager.Instance.currentSlot = slot;
        GameManager.Instance.StartGame();
    }

    private void StartNewGame()
    {
        chooseSlotPanel.SetActive(true);
        menuPanel.SetActive(false);
        UpdateChooseSlotButtons();
    }

    private void LoadGameBackClick()
    {
        menuPanel.SetActive(true);
        loadGamePanel.SetActive(false);
    }

    private void ChooseSlotBackClick()
    {
        menuPanel.SetActive(true);
        chooseSlotPanel.SetActive(false);
    }

    private void LoadGameClick()
    {
        menuPanel.SetActive(false);
        loadGamePanel.SetActive(true);
        UpdateSlotButtons();
    }
}
