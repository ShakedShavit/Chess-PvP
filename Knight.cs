using System;
using System.Collections.Generic;
using System.Text;

namespace ChessPvP
{
    class Knight : ChessPiece
    {
        public Knight(string colour)
            : base(colour)
        {

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

            if (move[2] != move[0] && move[1] != move[3])
            {
                int rowDifference;
                rowDifference = move[2] - move[0];
                if (rowDifference < 0)
                    rowDifference *= (-1);
                int columnDifference;
                columnDifference = move[3] - move[1];
                if (columnDifference < 0)
                    columnDifference *= (-1);

                //checks
                if (rowDifference + columnDifference != 3)
                    possible = false;
                if (piecesBoard[move[2], move[3]] != null)
                {
                    if (piecesBoard[move[2], move[3]] is Pawn)
                    {
                        if (((Pawn)piecesBoard[move[2], move[3]]).GetEnPassant() == false)
                            if (piecesBoard[move[2], move[3]].PieceIsWhite() == isWhite)
                                possible = false;
                    }
                    else
                    {
                        if (piecesBoard[move[2], move[3]].PieceIsWhite() == isWhite)
                            possible = false;
                    }

                }
            }
            else
                possible = false;

            //reverse it back
            if (isWhite)
            {
                piecesBoard = ReverseBoard(piecesBoard);
                move = ReverseMove(move);
            }
            return possible;
        }
        public override string ToString()
        {
            return base.ToString() + "N";
        }
    }
}
