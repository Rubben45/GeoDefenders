using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using TMPro;
using System.Collections;


// Acest Manager este responsabil de absolut tot de inseamna UI ( cu foarte mici exceptii specifice ) 
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("MainPanels")]
    [SerializeField] private Button mainButton;
    [SerializeField] private GameObject infoPanel, utilityPanel;
    [SerializeField] private GameObject leftArrow, upArrow;
    [SerializeField] private GameObject infoSign, towerSign;

    [Header("EnemyInfo")]
    [SerializeField] private GameObject physicalArrmor, magicalArrmor;
    [SerializeField] private GameObject currentEnemyInfoPanel;
    [SerializeField] private TextMeshProUGUI currentEnemyHP;
    [SerializeField] private TextMeshProUGUI currentEnemyArrmorStaus;
    [SerializeField] private TextMeshProUGUI currentEnemyMoveSpeed;
    [SerializeField] private TextMeshProUGUI currentEnemyDamage;

    [Header("TowerInfo")]
    [SerializeField] private GameObject physicalDamage, magicalDamage;
    [SerializeField] private GameObject currentTowerInfoPanel;
    [SerializeField] private TextMeshProUGUI currentTowerDamage;
    [SerializeField] private TextMeshProUGUI currentTowerDamageType;
    [SerializeField] private TextMeshProUGUI currentTowerAttackSpeed;
    [SerializeField] private TextMeshProUGUI currentTowerAmmoAmount;

    [SerializeField] private GameObject towerUtilityPanel;
    [SerializeField] private GameObject currentTowerUtilityInfo;

    [Header("UtilityPanel - Building")]
    [SerializeField] private GameObject buildingButtonsPanel;
    [SerializeField] private BuyBuildingButton[] buyBuildingButtons;

    [Header("UtilityPanel - Math")]
    [SerializeField] private GameObject mathUtilityPanel;
    [SerializeField] private GameObject mathEquationPanel;
    [SerializeField] private GameObject mathAnswersPanel;
    [SerializeField] private List<Button> answerButtons;
    [SerializeField] private TextMeshProUGUI equationText;
    [SerializeField] private RectTransform mathEQPanelRectT;

    [SerializeField] private GameObject deadScreen;
    [SerializeField] private GameObject winScreen;

    private MathEquation currentEquation;
    private Tower currentTower;

    private bool isUtilityPanelTransitioning = false;
    private bool isInfoPanelTransitioning = false;
    private bool isTowerInfoPanelTransitioning = false;
    private bool isMathPanelTransitioning = false;
    private bool isEnemyInfoOpen = false;
    private bool isTowerInfoOpen = false;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        infoPanel.SetActive(false);
        utilityPanel.SetActive(false);
        infoSign.SetActive(false);
        towerSign.SetActive(false);
        towerUtilityPanel.SetActive(false);
        currentTowerInfoPanel.SetActive(false);
        currentEnemyInfoPanel.SetActive(false);
        buildingButtonsPanel.SetActive(false);
        mathAnswersPanel.SetActive(false);
        mathEquationPanel.SetActive(false);
        mathUtilityPanel.SetActive(false);
        mathUtilityPanel.transform.localScale = new(0, 1, 1);
        infoPanel.transform.localScale = new(1, 0, 1);
        utilityPanel.transform.localScale = new(0, 1, 1);
        towerUtilityPanel.transform.localScale = new(0, 1, 1);

        mathEQPanelRectT.sizeDelta = new(mathEQPanelRectT.sizeDelta.x, 0f);
    }

    // Regiunea Building se ocupa de UI pentru construire turnurilor 
    #region ---- Building ---- 
    public void OpenUtilityPanel(GameObject olderLocation, Transform towerPlace, Tower.TowerLocation towerLocation)
    {
        if (isUtilityPanelTransitioning) return;

        isUtilityPanelTransitioning = true;

        utilityPanel.SetActive(true);
        leftArrow.SetActive(false);
        utilityPanel.transform.DOScaleX(1f, 1f).OnComplete(() =>
        {
            buildingButtonsPanel.SetActive(true);
            isUtilityPanelTransitioning = false;
        });

        foreach(BuyBuildingButton buildingButton in buyBuildingButtons)
        {
            buildingButton.PlaceToBuildTower = towerPlace;
            buildingButton.CurrentTowerLocation = towerLocation;
            buildingButton.OldBuild = olderLocation;
        }

    }

    public void CloseUtilityPanel()
    {
        if (isUtilityPanelTransitioning) return;

        isUtilityPanelTransitioning = true;

        buildingButtonsPanel.SetActive(false);

        utilityPanel.transform.DOScaleX(0f, 1f).OnComplete(() => {
            utilityPanel.SetActive(false);
            leftArrow.SetActive(true);
            isUtilityPanelTransitioning = false;
        });
    }
    
    public void ShowBuildingInfoPanel(GameObject panelToShow)
    {
        panelToShow.SetActive(true);
    }

    public void HideBuildingInfoPanel(GameObject panelToHide)
    {
        panelToHide.SetActive(false);
    }

    #endregion

    // Regiunea ReloadUI se ocupa de UI pentru partea de a da reload folosind matematica 
    #region ---- ReloadUI ----
    public void OpenMathEquationPanel(MathEquation equation, Tower tower)
    {
        HideTowerUtilityInfo();

        currentEquation = equation;
        currentTower = tower;
        equationText.text = "";
        mathUtilityPanel.SetActive(true);

        mathUtilityPanel.transform.DOScaleX(1f, 1f).OnComplete(() =>
        {     
            mathAnswersPanel.SetActive(true);
            mathEquationPanel.SetActive(true);
            mathEQPanelRectT.DOSizeDelta(new(mathEQPanelRectT.sizeDelta.x, 120f), 1f).OnComplete(() =>
            {
                equationText.text = equation.Equation;
            });
        });

        for (int i = 0; i < answerButtons.Count; i++)
        {
            if (i < equation.PossibleAnswers.Count)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = equation.PossibleAnswers[i];
                int index = i;
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(equation.PossibleAnswers[index]));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnAnswerSelected(string selectedAnswer)
    {
        MathManager.Instance.CheckAnswer(currentEquation, selectedAnswer, currentTower);
        equationText.text = "";
        mathEQPanelRectT.DOSizeDelta(new(mathEQPanelRectT.sizeDelta.x, 0f), 1f).OnComplete(() =>
        {
            mathAnswersPanel.SetActive(false);
            mathUtilityPanel.transform.DOScaleX(0f, 1f).OnComplete(() =>
            {
                HideMathUI();
            });

        });
        
    }

    public void HideMathUI()
    {
        mathEquationPanel.SetActive(false);
        foreach (Button button in answerButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
            ShowTowerUtilityInfo();
        }
    }

    #endregion
    // Regiunea InfoPanel se ocupa de UI pentru partea informatica a jocului precum turnurile si ianmicii 
    #region ---- InfoPanel ----
    private string GetMoveSpeed(float moveSpeed)
    {
        if (moveSpeed <= 2) return "LENT";
        if (moveSpeed < 5 && moveSpeed > 2) return "MED";
        if (moveSpeed >= 5) return "RAPID";

        return "UNDEFINED";
    }
    private string GetArrmorStatus(EnemySO enemyData)
    {
        physicalArrmor.SetActive(true);
        magicalArrmor.SetActive(false);

        if (enemyData.PhysicalResistance == EnemySO.EnemyPhysicalResistance.None && enemyData.MagicResistance == EnemySO.EnemyMagicResistance.None) return "NONE";

        if (enemyData.PhysicalResistance != EnemySO.EnemyPhysicalResistance.None)
        {
            physicalArrmor.SetActive(true);
            magicalArrmor.SetActive(false);

            switch (enemyData.PhysicalResistance)
            {
                case EnemySO.EnemyPhysicalResistance.Low: return "SLAB";
                case EnemySO.EnemyPhysicalResistance.Medium: return "MED";
                case EnemySO.EnemyPhysicalResistance.High: return "MARE";
            }
        }

        if (enemyData.MagicResistance != EnemySO.EnemyMagicResistance.None)
        {
            physicalArrmor.SetActive(false);
            magicalArrmor.SetActive(true);

            switch (enemyData.MagicResistance)
            {
                case EnemySO.EnemyMagicResistance.Low: return "SLAB";
                case EnemySO.EnemyMagicResistance.Medium: return "MED";
                case EnemySO.EnemyMagicResistance.High: return "MARE";
            }
        }

        return "UNKNOWN";
    }
    private string GetDamageType(TowerSO towerData)
    {
        if (towerData.TowerDamageType == TowerSO.DamageType.Physical)
        {
            physicalDamage.SetActive(true);
            magicalDamage.SetActive(false);

            return "FIZIC";
        }

        if (towerData.TowerDamageType == TowerSO.DamageType.Magic)
        {
            physicalDamage.SetActive(false);
            magicalDamage.SetActive(true);

            return "MAGIC";
        }

        return "UNKNOWN";
    }
    private string GetAttackSpeed(float attackSpeed)
    {
        if (attackSpeed < 1) return "LENT";
        if (attackSpeed >= 1 && attackSpeed < 1.5) return "MED";
        if (attackSpeed >= 1.5) return "RAPID";

        return "UNDEFINED";
    }   

    public void ShowEnemyInfo(EnemySO enemyData, float currentEnemyHealthPoints)
    {
        if (isInfoPanelTransitioning) return;

        isInfoPanelTransitioning = true;

        if (isTowerInfoOpen)
        {
            HideTowerInfo();
        }

        isEnemyInfoOpen = true;
        isTowerInfoOpen = false;

        infoPanel.SetActive(true);
        currentTowerInfoPanel.SetActive(false);
        currentEnemyInfoPanel.SetActive(true);

        infoSign.SetActive(true);
        towerSign.SetActive(false);

        upArrow.SetActive(false);

        infoPanel.transform.DOScaleY(1f, 1f);
        StartCoroutine(ResetInfoPanelTransitioning());

        currentEnemyHP.text = currentEnemyHealthPoints.ToString();
        currentEnemyDamage.text = enemyData.EnemyDamage.ToString();
        currentEnemyMoveSpeed.text = GetMoveSpeed(enemyData.EnemyMoveSpeed);
        currentEnemyArrmorStaus.text = GetArrmorStatus(enemyData);
    }

    public void ShowTowerInfo(TowerSO towerData, float currentTowerAmmo)
    {
        if (isInfoPanelTransitioning) return;

        isInfoPanelTransitioning = true;

        if (isEnemyInfoOpen)
        {
            HideEnemyInfo();
        }

        isTowerInfoOpen = true;
        isEnemyInfoOpen = false;

        infoPanel.SetActive(true);
        currentEnemyInfoPanel.SetActive(false);
        currentTowerInfoPanel.SetActive(true);

        infoSign.SetActive(false);
        towerSign.SetActive(true);

        upArrow.SetActive(false);

        infoPanel.transform.DOScaleY(1f, 1f);
        StartCoroutine(ResetInfoPanelTransitioning());

        currentTowerDamage.text = towerData.AttackDamage.ToString();
        currentTowerAmmoAmount.text = currentTowerAmmo.ToString();
        currentTowerDamageType.text = GetDamageType(towerData);
        currentTowerAttackSpeed.text = GetAttackSpeed(towerData.AttackSpeed);
    }

    public void ShowTowerUtilityInfo()
    {
        towerUtilityPanel.SetActive(true);
        currentTowerInfoPanel.SetActive(true);

        leftArrow.SetActive(false);

        towerUtilityPanel.transform.DOScaleX(1f, 1f);
    }

    public void HideTowerUtilityInfo()
    {
        currentTowerInfoPanel.SetActive(false);
        towerUtilityPanel.transform.DOScaleX(0f, 1f).OnComplete(() =>
        {
            towerUtilityPanel.SetActive(false);
            leftArrow.SetActive(true);
        });
    }

    public void HideEnemyInfo()
    {
        if (!isTowerInfoOpen)
        {
            infoPanel.transform.DOScaleY(0f, 1f).OnComplete(() =>
            {
                infoPanel.SetActive(false);
                upArrow.SetActive(true);
            });
        }

        currentEnemyInfoPanel.SetActive(false);
        isEnemyInfoOpen = false;
        infoSign.SetActive(false);
    }

    public void HideTowerInfo()
    {
        if (!isEnemyInfoOpen)
        {
            infoPanel.transform.DOScaleY(0f, 1f).OnComplete(() =>
            {
                infoPanel.SetActive(false);
                upArrow.SetActive(true);
            });
        }

        currentTowerInfoPanel.SetActive(false);
        isTowerInfoOpen = false;
        towerSign.SetActive(false);
    }

    #endregion

    public void ShowDeadScreen()
    {
        deadScreen.SetActive(true);
        deadScreen.transform.DOScale(1f, 2f);
    }
    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
        winScreen.transform.DOScale(1f, 2f);
    }


    private IEnumerator ResetInfoPanelTransitioning()
    {
        yield return new WaitForSeconds(1);
        isInfoPanelTransitioning = false;
    }
}
