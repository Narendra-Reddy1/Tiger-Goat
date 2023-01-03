using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugButton : MonoBehaviour
{
    #region Variables
    [SerializeField]private TextMeshProUGUI textMeshProUGUI;
    private DebugMethodData methodData;
    #endregion Variables


    #region Public Methods
    public void Init(DebugMethodData data)
    {
        methodData = data;
        textMeshProUGUI.text = data.displayName;
    }

    public void OnClickButton()
    {
        if(methodData!=null)
        {
            DebugButtonAttributeHelper.CallMethod(methodData);
        }
    }
    #endregion Public Methods



}
