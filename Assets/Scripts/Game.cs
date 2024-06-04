using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chackers3D.Assets.Scripts
{
    public class Game : MonoBehaviour
    {        
        [SerializeField] private bool isWhite;
        public MoveCommand moveCommand;
        public CheckersBoard checkersBoard;
        public PieceMover pieceMover;
        private void Start()
        {
            pieceMover = new PieceMover(isWhite);
            moveCommand = new MoveCommand(pieceMover);   
            checkersBoard = GameObject.FindGameObjectWithTag("Board").GetComponent<CheckersBoard>();
        }

        void Update()
        {
            pieceMover.UpdateMouseOver();
            CommandInvoker.ExecuteCommand(moveCommand);

            if (Input.GetKeyDown(KeyCode.Z)) // Undo with 'Z' key
            {
                CommandInvoker.UndoCommand();
            }

            if (Input.GetKeyDown(KeyCode.Escape)) // Close game with 'Escape' key
            {
                CloseGame();
            }
        }

        public void RestartBoard()
        {
            // Destroy all existing pieces on the board
            foreach (Transform child in checkersBoard.transform)
            {
                GameObject.Destroy(child.gameObject);
            }


            checkersBoard.pieces = new Piece[8, 8];
            pieceMover.isWhiteTurn = true;
            pieceMover.isWhite = true;
            checkersBoard.GenerateBoard();
        }
        private void CloseGame()
        {
            Application.Quit();
        }
    }
}