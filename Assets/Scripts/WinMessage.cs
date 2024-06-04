using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Chackers3D.Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace Chackers3D.Assets.Scripts
{
    public class WinMessage : MonoBehaviour
    {
        [SerializeField] private Canvas endMenu;
        [SerializeField] private TextMeshProUGUI winnerText;
        
        private void Awake() 
        {
            endMenu = GameObject.FindGameObjectWithTag("EndMenu").GetComponent<Canvas>();
            endMenu.enabled = false;
        }
        public void CheckVictory()
        {
            var pieces = FindObjectsOfType<Piece>();

            bool hasWhite = false, hasBlack = false;
            for (int i = 0; i < pieces.Length; i++)
            {
                if (pieces[i].isWhite)
                    hasWhite = true;
                else 
                    hasBlack = true;
            }

            if (!hasWhite)
                Victory(false);
            if (!hasBlack)
                Victory(true);
        }

        private void Victory(bool isWhite)
        {
            endMenu.enabled = true;
            if (isWhite)
                winnerText.SetText("White Team Won!");
            else 
                winnerText.SetText("Black Team Won!");
        }
    }
}