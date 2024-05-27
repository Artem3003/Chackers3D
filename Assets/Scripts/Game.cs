using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chackers3D.Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private bool isWhite;
        public MoveCommand moveCommand;
        public PieceMover pieceMover;
        void Start()
        {
            pieceMover = new PieceMover(isWhite);
            moveCommand = new MoveCommand(pieceMover);
        }

        void Update()
        {
            pieceMover.UpdateMouseOver();
            CommandInvoker.ExecuteCommand(moveCommand);

            if (Input.GetKeyDown(KeyCode.Z)) // Undo with 'Z' key
            {
                CommandInvoker.UndoCommand();
            }
        }

        // private void CheckVictory()
        // {
        //     var pieces = FindObjectsOfType<Piece>();

        //     bool hasWhite = false, hasBlack = false;
        //     for (int i = 0; i < pieces.Length; i++)
        //     {
        //         if (pieces[i].isWhite)
        //             hasWhite = true;
        //         else 
        //             hasBlack = true;
        //     }

        //     if (!hasWhite)
        //         Victory(false);
        //     if (!hasBlack)
        //         Victory(true);
        // }

        // private void Victory(bool isWhite)
        // {
        //     if (isWhite)
        //         Debug.Log("White team has won!");
        //     else 
        //         Debug.Log("Black team has won");
        // }
    }
}