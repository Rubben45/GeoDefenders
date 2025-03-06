using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class TowerUtilityManager : MonoBehaviour
{
    public static TowerUtilityManager Instance { get; private set; }

    [SerializeField] private MathEquationsDatabase equationsDatabase;
    [SerializeField] private Button reloadButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Color normalColor, notAvailableColor;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private GameObject maxLevelText;

    private bool isUpgrading = false;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private bool IsMaxLevel(TowerSO upgradeTowerSO)
    {
        return upgradeTowerSO == null;
    }

    public void SetTowerUtilityPanel(Tower towerToSet, IInteractable currentTower, TowerSO currentTowerSO, TowerSO upgradeTowerSO)
    {
        if (currentTower.CurrentAmmoAmount < currentTowerSO.TowerAmmo)
        {
            reloadButton.image.color = normalColor;
            reloadButton.interactable = true;
        }
        else
        {
            reloadButton.interactable = false;
            reloadButton.image.color = notAvailableColor;
        }

        if (IsMaxLevel(upgradeTowerSO))
        {
            upgradeButton.gameObject.SetActive(false);
            maxLevelText.SetActive(true);
        }
        else
        {
            upgradeButton.gameObject.SetActive(true);
            maxLevelText.SetActive(false);

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => UpgradeTower(towerToSet, upgradeTowerSO));
        }

        if (upgradeTowerSO != null)
        {
            upgradeCost.text = upgradeTowerSO.TowerPrice.ToString();
        }

        reloadButton.onClick.RemoveAllListeners();
        reloadButton.onClick.AddListener(() => RequestReload(towerToSet));

    }

    private void RequestReload(Tower towerToRequestReload)
    {
        SFXManager.Instance.PlaySoundEffect("Click");
        MathEquation equation = GetRandomMathEquation();
        UIManager.Instance.OpenMathEquationPanel(equation, towerToRequestReload);
    }

    private void UpgradeTower(Tower tower, TowerSO upgradeTowerSO)
    {
        GameObject lastTower = tower.gameObject;

        if (upgradeTowerSO.TowerPrice <= EconomyManager.Instance.CurrentCoins)
        {
            SFXManager.Instance.PlaySoundEffect("Click");
            BuildingManager.Instance.UpgradeTower(upgradeTowerSO, tower.transform, tower.CurrentTowerLocation, lastTower);
        }
    }

    private MathEquation GetRandomMathEquation()
    {
        if (equationsDatabase.equations.Count == 0)
        {
            Debug.LogError("No equations in the database!");
            return null;
        }

        int index = Random.Range(0, equationsDatabase.equations.Count);
        return equationsDatabase.equations[index];
    }
}
