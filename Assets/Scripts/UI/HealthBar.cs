using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance;
    public HitPoints hitPoints;
    [HideInInspector] public Player character;
    public Image meterImage;
    public Text hpText;
    float maxHitPoints;
    void Start()
    {
        maxHitPoints = character.maxHitPoints;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (character != null)
        {
            meterImage.fillAmount = hitPoints.value / maxHitPoints;
            hpText.text = "HP:" + (meterImage.fillAmount * 100);
        }
    }
}
