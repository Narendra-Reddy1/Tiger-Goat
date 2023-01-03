using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButtonsCreator : MonoBehaviour
{
    #region Variables
    [SerializeField] private DebugButton debugButtonPrefab;
    [SerializeField] private Transform leftParent;
    [SerializeField] private Transform middleParent;
    [SerializeField] private Transform rightParent;

    private int NUMBER_OF_CHILD_PARENT_CAN_HOLD = 5;
    #endregion Variables

    #region Unity Methods
    private void Awake()
    {
        if (DebugButtonAttributeHelper.data.Count <= 0)
        {
            DebugButtonAttributeHelper.GetAllMethodsWithDebugButtonAttribute();
        }

        List<DebugMethodData> debugData = DebugButtonAttributeHelper.data;
        foreach (DebugMethodData item in debugData)
        {
            DebugButton button = Instantiate(debugButtonPrefab, GetParentBasedOnChildCount());
            button.Init(item);
        }
    }
    #endregion Unity Methods

    #region Public Methods
    #endregion Public Methods

    #region Private Methods
    private Transform GetParentBasedOnChildCount()
    {
        if (leftParent.childCount < NUMBER_OF_CHILD_PARENT_CAN_HOLD)
            return leftParent;
        else if (middleParent.childCount < NUMBER_OF_CHILD_PARENT_CAN_HOLD)
            return middleParent;
        else
            return rightParent;
    }
    #endregion Private Methods

    #region Callbacks
    #endregion Callbacks

}
