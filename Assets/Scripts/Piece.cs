using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isWhite;
    public bool isKing;

    public bool isForceToMove(Piece[,] board, int x, int y)
    {
        // for white team
        if (isWhite || isKing)
        {
            // top left
            if (x >= 2 && y <= 5)
            {
                Piece piece = board[x - 1, y + 1];
                // if there is a piece and it is not the same color as ours
                if (piece != null && piece.isWhite != isWhite)
                {
                    // check if its possible to land after the jump
                    if (board[x - 2, y + 2] == null)
                    {
                        return true;
                    }
                }
            }
            // to right
            if (x <= 5 && y <= 5)
            {
                Piece piece = board[x + 1, y + 1];
                // if there is a piece and it is not the same color as ours
                if (piece != null && piece.isWhite != isWhite)
                {
                    // check if its possible to land after the jump
                    if (board[x + 2, y + 2] == null)
                    {
                        return true;
                    }
                }
            }
        } 
        if (!isWhite || isKing) // for black team
        {
            // top left
            if (x >= 2 && y >= 2)
            {
                Piece piece = board[x - 1, y - 1];
                // if there is a piece and it is not the same color as ours
                if (piece != null && piece.isWhite != isWhite)
                {
                    // check if its possible to land after the jump
                    if (board[x - 2, y - 2] == null)
                    {
                        return true;
                    }
                }
            }
            // to right
            if (x <= 5 && y >= 2)
            {
                Piece piece = board[x + 1, y - 1];
                // if there is a piece and it is not the same color as ours
                if (piece != null && piece.isWhite != isWhite)
                {
                    // check if its possible to land after the jump
                    if (board[x + 2, y - 2] == null)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    public bool ValidMove(Piece[,] board, int x1, int y1, int x2, int y2)
    {
        // if you are moving on top of another piece
        if (board [x2, y2] != null)
            return false;

        int deltaMove = (int)Mathf.Abs(x1 - x2);
        int deltaMoveY = y2 - y1;
        // white team
        if (isWhite || isKing)
        {
            if (deltaMove == 1)
            {
                if (deltaMoveY == 1)
                    return true;
            } 
            else if (deltaMove == 2)
            {
                if (deltaMoveY == 2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) /2];
                    if (p != null && p.isWhite != isWhite) // piece is not the same color
                        return true;
                }
            }
        }
        
        // black team
        if (!isWhite || isKing)
        {
            if (deltaMove == 1)
            {
                if (deltaMoveY == -1)
                    return true;
            } 
            else if (deltaMove == 2)
            {
                if (deltaMoveY == -2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) /2];
                    if (p != null && p.isWhite != isWhite) // piece is not the same color
                        return true;
                }
            }
        }
        return false;
    }
}
