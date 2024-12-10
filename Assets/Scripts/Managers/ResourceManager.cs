using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public int Gold { get; private set; }
    public int Gem { get; private set; }

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI gemText;

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

    private void Start()
    {
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        UpdateUI();
    }

    public void AddGem(int amount)
    {
        Gem += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        goldText.text = Gold.ToString();
        gemText.text = Gem.ToString();
    }
}
