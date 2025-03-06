using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ReloadButton : MonoBehaviour
{
    [SerializeField] private MathEquationsDatabase equationsDatabase;
    [SerializeField] private Button reloadButton;
    public static ReloadButton Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void SetTowerToReload(Tower towerToSetReloadFor)
    {
        reloadButton.onClick.RemoveAllListeners();
        reloadButton.onClick.AddListener(() => RequestReload(towerToSetReloadFor));
    }

    private void RequestReload(Tower towerToRequestReload)
    {
        MathEquation equation = GetRandomMathEquation();
        UIManager.Instance.OpenMathEquationPanel(equation, towerToRequestReload);
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
