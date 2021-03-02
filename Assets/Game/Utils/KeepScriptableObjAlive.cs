using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepScriptableObjAlive : MonoSingleton<KeepScriptableObjAlive>
{
    [SerializeField]
    private List<Object> _keepSOs;
}
