using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        LogCutterWorkstation.ResetStaticData();
        AnvilWorkstation.ResetStaticData();
        BaseWorkstation.ResetStaticData();
        TrashWorkstation.ResetStaticData();
    }
}
