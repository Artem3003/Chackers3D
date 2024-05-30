using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Chackers3D.Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Chackers3D.Assets.Scripts
{
    public class MoveCommand : ICommand 
    {
        public PieceMover pieceMover;

        public MoveCommand(PieceMover pieceMover)
        {
            this.pieceMover = pieceMover; 
        }

        public void Execute()
        {
            if ((pieceMover.isWhite) ? pieceMover.isWhiteTurn : !pieceMover.isWhiteTurn)
            {
                int x = (int)pieceMover.mouseOver.x;
                int y = (int)pieceMover.mouseOver.y;

                if (pieceMover.selectedPiece != null)
                    pieceMover.UpdatePieceDrag(pieceMover.selectedPiece);

                if (Input.GetMouseButtonDown(0))
                    pieceMover.SelctPiece(x, y);

                if (Input.GetMouseButtonUp(0))
                    pieceMover.TryMove((int)pieceMover.startDrag.x, (int)pieceMover.startDrag.y, x, y);
            }
        }

        public void Undo()
        {
            if (pieceMover.moveHistory.Count > 0)
            {
                Move lastMove = pieceMover.moveHistory.Pop();

                // Check if the piece was crowned during the move and revert it
                if (lastMove.movedPiece.isWhite && lastMove.movedPiece.isKing && (int)pieceMover.endDrag.y == 7)
                {
                    lastMove.movedPiece.isKing = false;
                    lastMove.movedPiece.transform.Rotate(Vector3.right * 180);
                } 
                else if (!lastMove.movedPiece.isWhite && lastMove.movedPiece.isKing && (int)pieceMover.endDrag.y == 0)
                {
                    lastMove.movedPiece.isKing = false;
                    lastMove.movedPiece.transform.Rotate(Vector3.right * 180);
                }
                // Move the piece back to its original position
                pieceMover.checkersBoard.pieces[(int)lastMove.start.x, (int)lastMove.start.y] = lastMove.movedPiece;
                pieceMover.checkersBoard.pieces[(int)lastMove.end.x, (int)lastMove.end.y] = null;
                pieceMover.MovePiece(lastMove.movedPiece, (int)lastMove.start.x, (int)lastMove.start.y);

                if (lastMove.capturedPiece != null)
                {
                    lastMove.capturedPiece.gameObject.SetActive(true);
                    int captureX = (int)(lastMove.start.x + lastMove.end.x) / 2;
                    int captureY = (int)(lastMove.start.y + lastMove.end.y) / 2;
                    pieceMover.checkersBoard.pieces[captureX, captureY] = lastMove.capturedPiece;
                }

                // Change turn
                if (!pieceMover.hasKilled)
                {
                    pieceMover.isWhiteTurn = !pieceMover.isWhiteTurn;
                    pieceMover.isWhite = !pieceMover.isWhite;     
                }

                // Reset the hasKilled flag
                pieceMover.hasKilled = lastMove.capturedPiece != null;
            }
        }

        
    }
}