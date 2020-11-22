using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {

    #region Variables
    public int factionID;
    private bool disabled;
    SelectionManager selectionManager;
    #endregion

    #region Initialization
    public void Start() {
        selectionManager = SelectionManager.GetSingleton();
        disabled = false;
    }
    #endregion

    #region OnPointer Events
    public void OnPointerClick(PointerEventData eventData) {
        if (disabled) {
            return;
        }
        selectionManager.Confirm();
        disabled = true;
        gameObject.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (disabled) {
            return;
        }

        selectionManager.currentFactionInt = factionID;
        selectionManager.LoadFaction();
        selectionManager.UpdateFactionImage();
    }
    #endregion

}
