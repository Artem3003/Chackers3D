using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

namespace Chackers3D.Assets.Scripts
{
    public class PieceMover
    {
        TextMeshProUGUI turnText;
        public CheckersBoard checkersBoard;
        public WinMessage winMessage;
        public List<Piece> forcedPieces;
        public Stack<Move> moveHistory;

        public bool hasKilled;
        public bool isWhite;
        public bool isWhiteTurn;

        public Piece selectedPiece;
        public Vector2 mouseOver;
        public Vector2 startDrag;
        public Vector2 endDrag;

        public Vector3 boardOffset = new Vector3(-4f, 0, -4f);
        public Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

        public PieceMover(bool isWhite)
        {
            isWhiteTurn = true;
            this.isWhite = isWhite;
            forcedPieces = new List<Piece>();
            moveHistory = new Stack<Move>();
            checkersBoard = GameObject.FindGameObjectWithTag("Board").GetComponent<CheckersBoard>();
            winMessage = GameObject.FindGameObjectWithTag("EndMenu").GetComponent<WinMessage>();
        }

        public void MovePiece(Piece p, int x, int y)
        {
            p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
        }

        public void TryMove(int x1, int y1, int x2, int y2)
        {
            forcedPieces = ScanForPossibleMove();
            startDrag = new Vector2(x1, y1);
            endDrag = new Vector2(x2, y2);
            selectedPiece = checkersBoard.pieces[x1, y1];

            if (x2 < 0 || x2 >= checkersBoard.pieces.Length || y2 < 0 || y2 >= checkersBoard.pieces.Length) // out of bounds
            {
                if (selectedPiece != null)
                    MovePiece(selectedPiece, x1, y1);

                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }

            if (selectedPiece != null) // selected Piece
            {
                // if it hasn't moved
                if (endDrag == startDrag)
                {
                    MovePiece(selectedPiece, x1, y1);
                    startDrag = Vector2.zero;
                    selectedPiece = null;
                    return;
                }

                // Check if its valid move
                if (selectedPiece.ValidMove(checkersBoard.pieces, x1, y1, x2, y2))
                {
                    Piece capturedPiece = null;
                    // if this is a jump
                    if (Math.Abs(x2 - x1) == 2)
                    {
                        capturedPiece = checkersBoard.pieces[(x1 + x2) /2, (y1 + y2) / 2];
                        if (capturedPiece != null)
                        {
                            checkersBoard.pieces[(x1 + x2) / 2, (y1 + y2) / 2] = null;
                            capturedPiece.gameObject.SetActive(false);
                            hasKilled = true;
                            capturedPiece.DisapperSound();
                        }
                    }

                    if (forcedPieces.Count != 0 && !hasKilled)
                    {
                        MovePiece(selectedPiece, x1, y1);
                        startDrag = Vector2.zero;
                        selectedPiece = null;
                        return;
                    }

                    // Record the move
                    moveHistory.Push(new Move
                    {
                        start = startDrag,
                        end = endDrag,
                        movedPiece = selectedPiece,
                        capturedPiece = capturedPiece
                    });

                    checkersBoard.pieces[x2, y2] = selectedPiece;
                    checkersBoard.pieces[x1, y1] = null;
                    MovePiece(selectedPiece, x2, y2);

                    selectedPiece.StepSound();

                    EndTurn();
                } 
                else 
                {
                    MovePiece(selectedPiece, x1, y1);
                    startDrag = Vector2.zero;
                    selectedPiece = null;
                    return;
                }
            }
        }

        public void UpdatePieceDrag(Piece piece)
        {
            if (!Camera.main)
            {
                Debug.Log("Enable to find main camera");
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25, LayerMask.GetMask("Board")))
                piece.transform.position = hit.point + Vector3.up;
        }

        public void UpdateMouseOver()
        {
            if (!Camera.main)
            {
                Debug.Log("Enable to find main camera");
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25, LayerMask.GetMask("Board")))
            {
                mouseOver.x = (int)(hit.point.x - boardOffset.x);
                mouseOver.y = (int)(hit.point.z - boardOffset.z);
            } else
            {
                mouseOver.x = -1;
                mouseOver.y = -1;
            }
        }

        public void SelctPiece(int x, int y)
        {
            if (x < 0 || x >= checkersBoard.pieces.Length || y < 0 || y >= checkersBoard.pieces.Length) // out of bounds
                return;

            Piece p = checkersBoard.pieces[x, y];
            if (p != null && p.isWhite == isWhite)
            {
                if (forcedPieces.Count == 0)
                {
                    selectedPiece = p;
                    startDrag = mouseOver;
                } else
                {
                    // look for the piece under our forced pieces list
                    if (forcedPieces.Find(fp => fp == p) == null)
                        return;
                    
                    selectedPiece = p;
                    startDrag = mouseOver;
                }
            }
        }

        private void EndTurn()
        {
            int x = (int)endDrag.x;
            int y = (int)endDrag.y;

            CreateKingPiece();

            selectedPiece = null;
            startDrag = Vector2.zero;

            if (ScanForPossibleMove(selectedPiece, x, y).Count != 0 && hasKilled)
                return;

            isWhiteTurn = !isWhiteTurn;
            isWhite = !isWhite;
            hasKilled = false;

            winMessage.CheckVictory();
        }

        private void CreateKingPiece()
        {
            int y = (int)endDrag.y;

            if (selectedPiece != null)
            {
                if (selectedPiece.isWhite && !selectedPiece.isKing && y == 7)
                {
                    selectedPiece.isKing = true;
                    selectedPiece.transform.Rotate(Vector3.right * 180);
                } 
                else if (!selectedPiece.isWhite && !selectedPiece.isKing && y == 0)
                {
                    selectedPiece.isKing = true;
                    selectedPiece.transform.Rotate(Vector3.right * 180);
                }
            }
        }

        private List<Piece> ScanForPossibleMove(Piece p, int x, int y)
        {
            forcedPieces = new List<Piece>();

            if(checkersBoard.pieces[x, y].isForceToMove(checkersBoard.pieces, x, y))
            {
                forcedPieces.Add(checkersBoard.pieces[x, y]);
            }

            return forcedPieces;
        }
        public List<Piece> ScanForPossibleMove()
        {
            forcedPieces = new List<Piece>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (checkersBoard.pieces[i, j] != null && checkersBoard.pieces[i, j].isWhite == isWhiteTurn)
                    {
                        if(checkersBoard.pieces[i, j].isForceToMove(checkersBoard.pieces, i, j))
                        {
                            forcedPieces.Add(checkersBoard.pieces[i, j]);
                        }
                    }
                }
            }
            return forcedPieces;
        }
    }
}