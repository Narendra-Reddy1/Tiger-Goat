using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScriptlableObject : ScriptableObject
{
    [TextArea(5, 1)]
    public string description;
}
