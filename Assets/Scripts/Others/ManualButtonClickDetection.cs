using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class ManualButtonClickDetection : MonoBehaviour
{
    [SerializeField] private List<Button> allButtons = new List<Button>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    pointerId = -1,
                };
                pointerData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                foreach (RaycastResult result in results)
                {
                    Button clickedButton = result.gameObject.GetComponent<Button>();
                    if (clickedButton != null && allButtons.Contains(clickedButton))
                    {
                        clickedButton.onClick.Invoke();
                    }
                }
            }
        }
    }
}
