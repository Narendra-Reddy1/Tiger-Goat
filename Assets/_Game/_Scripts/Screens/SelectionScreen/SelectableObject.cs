using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectableObject : MonoBehaviour, IPointerDownHandler
{
    #region Variables
    public UnityEvent OnObjectSelected;
    public UnityEvent OnObjectDeselected;
    [SerializeField] private UnityEngine.UI.Image selectionOverlay;
    private bool isObjectSelected = false;
    #endregion Variables

    #region Unity Methods
    private void OnEnable()
    {
        OnObjectSelected.AddListener(OnSelectedThisObject);
        OnObjectDeselected.AddListener(OnDeselectThisOnbject);
    }
    private void OnDisable()
    {
        OnObjectSelected.RemoveListener(OnSelectedThisObject);
        OnObjectDeselected.RemoveListener(OnDeselectThisOnbject);

    }
    #endregion Unity Methods

    #region Public Methods
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null) return;
        OnObjectSelected?.Invoke();
    }

    public void Select()
    {
        OnObjectSelected?.Invoke();
    }
    public void Deselect()
    {
        OnObjectDeselected?.Invoke();
    }
    #endregion Public Methods

    #region Private Methods
    private void OnSelectedThisObject()
    {
        selectionOverlay.gameObject.SetActive(true);
        isObjectSelected = true;
        Debug.Log($"Selected: {gameObject.name}");
    }
    private void OnDeselectThisOnbject()
    {
        selectionOverlay.gameObject.SetActive(false);
        isObjectSelected = false;
        Debug.Log($"DeSelected: {gameObject.name}");
    }
    #endregion Private Methods

    #region Callbacks

    #endregion Callbacks

}
