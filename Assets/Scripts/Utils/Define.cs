using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    #region Weapon

    public enum ItemStyle
    {
        AR,              // 돌격소총
        DMR,             // 지정사수소총
        SMG,             //기관단총
        SR,              // 저격소총
        SHOTGUN,         //산탄총
        HANDGUN,         // 권총
        MELEE,           // 근접
        THROWABLE,       // 투척무기
        ETC              // 기타
    }

    public enum GunStyles
    {
        nonautomatic, 
        automatic
    }

    #endregion

    #region Other

    public enum E_TeamId
    {
        Player = 0,
        Monster = 1,
    }

    public enum Scene
    {
        Unknown = 0,
        Start = 1,
        Lobby = 2,
        Game = 3,
    }

    public enum Sound
    {
        Bgm = 0,
        Effect = 1,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        
    }

    public enum CursorType
    {
        None,
        Arrow,
        Hand,
        Look,
    }
    #endregion
}
