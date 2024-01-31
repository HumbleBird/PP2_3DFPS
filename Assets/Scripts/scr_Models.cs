using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class scr_Models
{
    #region Player

    [Serializable]
    public class PlayerSettingsModel
    {
        [Header("View Settings")]
        public float ViewXSensitivity = 10;
        public float ViewYSensitivity = 10;

        public bool ViewXInverted;
        public bool ViewYInverted;
    }

    #endregion
}
