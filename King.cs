using System;
using System.Collections.Generic;
using System.Text;

namespace ChessPvP
{
    class King : ChessPiece
    {
        bool moved;
        public King(string colour)
            : base(colour)
        {
            moved = false;
        }
        public King(string colour, bool moved)
            : base(colour)
        {
            moved = false;
            this.moved = moved;
        }
        public bool GetMoved()
        {
            return moved;
        }
        public void SetMoved(bool value)
        {
            moved = value;
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

            int rowDifference;
            rowDifference = move[2] - move[0];
            if (rowDifference < 0)
                rowDifference *= (-1);
            int columnDifference;
            columnDifference = move[3] - move[1];
            if (columnDifference < 0)
                columnDifference *= (-1);

            //checks
            if (rowDifference + columnDifference <= 2 && rowDifference + columnDifference > 0)
            {
                if (move[0] == move[2] || move[1] == move[3])
                    if (rowDifference + columnDifference != 1)
                        possible = false;
            }
            else
                possible = false;

            //reverse it back
            if (isWhite)
            {
                piecesBoard = ReverseBoard(piecesBoard);
                move = ReverseMove(move);
            }

            //Castling check
            /*#region Castling check
            if (move[0] == move[2] && (move[0] == 0 || move[0] == 7) && move[1] == 4)
            {
                int start;
                if (move[3] > move[1])
                    start = move[1];
                else
                    start = move[3] - 1;

                if (move[3] - move[1] == 2 || move[1] - move[3] == 2)
                {
                    if (!(board.CheckIfKingOrRookMoved(Program.GetGameBoards(), move[0], move[3])))
                    {
                        if (pieces[move[0], start + 1] == null && pieces[move[0], start + 2] == null)
                        {
                            //If it's a big castling that check all three positions rather than just the two
                            if (move[1] > move[3])
                                if (pieces[move[0], start] != null)
                                    return false;
                            int[] checkPos = new int[2];
                            checkPos[0] = move[0];
                            checkPos[1] = move[1];
                            //Check if there is a check before the move
                            if (board.Check(checkPos))
                                return false;
                            //Check if there is a threat on the position between the King and the Rook
                            checkPos[1] = start + 1;
                            if (board.Check(checkPos))
                                return false;
                            //Check if there is a threat on the position that the King will end up in (the Rook's current position)
                            checkPos[1] = start + 2;
                            if (board.Check(checkPos))
                                return false;
                            //If it's a big castling that check all three positions rather than just the two
                            if (move[1] > move[3])
                            {
                                checkPos[1] = start;
                                if (board.Check(checkPos))
                                    return false;
                            }
                            Console.WriteLine("Castling");
                            return true;
                        }
                    }
                }
            }
            #endregion*/

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

            return possible;
        }
        public override string ToString()
        {
            return base.ToString() + "K";
        }
    }
}
