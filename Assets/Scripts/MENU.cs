using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolveGames
{
    public class MENU : MonoBehaviour
    {
        public PlayerManager player;

        [Header("MENU")]
        [SerializeField] GameObject MenuPanel;
        [SerializeField] Animator ani;

        private void Update()
        {
            if (player.inputHandler.m_ESC_Input)
            {
                player.inputHandler.m_ESC_Input = false;

                if (MenuPanel.activeInHierarchy)
                {
                    MenuPanel.SetActive(false);
                    player.canMove = true;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Time.timeScale = 1.0f;
                    ani.SetBool("START", false);
                }
                else
                {
                    MenuPanel.SetActive(true);
                    player.canMove = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0.0f;
                    ani.SetBool("START", true);
                }
            }
        }
    }
}

   
