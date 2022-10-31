using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownUIManager : MonoBehaviour
{
    [Header("References")]
    public Canvas WorldCanvas;
    public Canvas ScreenCanvas;

    [Header("Prefabs")]
    public GameObject PopupPrefab;

    private GameObject Popup;

    public void ShowPopup(string text, Transform transform)
    {
        ClearPopup();
        Popup = Instantiate(PopupPrefab, transform);
        Popup.GetComponent<Popup>().SetText(text);
    }

    public void ClearPopup()
    {
        if (Popup != null)
        {
            Destroy(Popup);
            Popup = null;
        }
    }
}
