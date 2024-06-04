using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Chackers3D.Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;


namespace Chackers3D.Assets.Scripts
{
    public class CheckersBoard : MonoBehaviour
    {
        public PieceMover pieceMover = null;
        public Piece[,] pieces;
        public GameObject whitePiecePrefab;
        public GameObject blackPiecePrefab;
        public  CheckersBoard _instance;
        void Awake() 
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        void Start()
        {
            pieceMover = new PieceMover(true);
            pieces = new Piece[8, 8];
        }

        public void PlayGame()
        {
            GenerateBoard();
        }


        public void GenerateBoard()
        {
            // Generate white team
            for (int y = 0; y < 3; y++)
            {
                bool oddRow = (y % 2 == 0);
                for (int x = 0; x < 8; x += 2)
                    GeneratePiece((oddRow) ? x: x + 1, y); // Generate our Piece
            }

            // Generate black team
            for (int y = 7; y > 4; y--)
            {
                bool oddRow = (y % 2 == 0);
                for (int x = 0; x < 8; x += 2)
                    GeneratePiece((oddRow) ? x: x + 1, y); // Generate our Piece
            }
        }

        private void GeneratePiece(int x, int y)
        {
            bool isPieceWhite = (y > 3) ? false : true;
            GameObject go = Instantiate((isPieceWhite) ? whitePiecePrefab : blackPiecePrefab) as GameObject;
            go.transform.SetParent(transform); // make pieces childrens of the board
            Piece p = go.GetComponent<Piece>();
            pieces[x, y] = p;
            pieceMover.MovePiece(p, x, y);
        }
    }
}