using System;
using System.Collections.Generic;
using System.Text;

namespace ChessPvP
{
    class Pawn : ChessPiece
    {
        bool enPassant;
        public Pawn(string colour)
            :base(colour)
        {
            SetEnPassant(false);
        }
        public Pawn(string colour, bool enPassant)
            : base(colour)
        {
            SetEnPassant(enPassant);
        }

        public void SetEnPassant(bool value)
        {
            enPassant = value;
        }
        public bool GetEnPassant()
        {
            return enPassant;
        }

        public override bool CanMoveTo(ChessPiece[,] piecesBoard, int[] move, int turn)
        {
            bool possible = true;
            bool isWhite = base.PieceIsWhite();

            //Checks if the colur of the piece and the turn match up
            if (!IfTheMovingPieceIsInTheRightColourAndTurn(isWhite, turn))
                return false;

            if (isWhite)
            {
                piecesBoard = ReverseBoard(piecesBoard);
                move = ReverseMove(move);
            }


            //Regular move (no eating)
            if ((move[2] - move[0] == 1) && (move[1] == move[3]))
            {
                //If the position you move to is empty
                if (piecesBoard[move[2], move[3]] != null)
                    possible = false;
            }
            //Start move (no eating)
            else if ((move[2] - move[0] == 2) && (move[1] == move[3]) && (move[0] == 1))
            {
                //If the position you move to is empty
                if ((piecesBoard[move[2], move[3]] != null) || (piecesBoard[move[2] - 1, move[3]] != null))
                    possible = false;
            }
            //Eating
            else if ((move[2] - move[0] == 1) && ((move[1] - move[3] == 1) || (move[3] - move[1] == 1)))
            {
                if ((piecesBoard[move[2], move[3]] == null) || (piecesBoard[move[2], move[3]].PieceIsWhite() == isWhite))
                    possible = false;
            }
            //The move is illegal
            else
                possible = false;

            //reverse it back
            if (isWhite == true)
            {
                piecesBoard = ReverseBoard(piecesBoard);
                move = ReverseMove(move);
            }
            return possible;
        }
        public override string ToString()
        {
            if (enPassant)
                return "EE";
            return base.ToString() + "P";
        }
    }
}
