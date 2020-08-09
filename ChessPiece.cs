using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ChessPvP
{
    class ChessPiece
    {
        bool isWhite;

        public ChessPiece(string colour)
        {
            SetColour(colour);
        }
        public ChessPiece() { }

        public bool PieceIsWhite()
        {
            return isWhite;
        }
        public void SetColour(string value)
        {
            if (value == "white")
                isWhite = true;
            else
                isWhite = false;
        }

        
        public virtual bool CanMoveTo(ChessPiece[,] piecesBoard, int[] move, int turn)
        {
            return false;
        }

        public bool CanMoveInStraightLine(ChessPiece[,] piecesBoard, int[] move, int turn)
        {
            bool possible = true;
            bool isWhite = PieceIsWhite();

            //Checks if the colur of the piece and the turn match up
            if (!IfTheMovingPieceIsInTheRightColourAndTurn(isWhite, turn))
                return false;

            if (isWhite)
            {
                piecesBoard = ReverseBoard(piecesBoard);
                move = ReverseMove(move);
            }

            if (move[0] == move[2] && move[1] != move[3])
            {
                int start;
                int end;
                if (move[3] > move[1])
                {
                    start = move[1];
                    end = move[3];
                }
                else
                {
                    start = move[3];
                    end = move[1];
                }

                //checks if the column is empty
                for (int i = start + 1; i < end; i++)
                {
                    if (piecesBoard[move[0], i] != null)
                    {
                        possible = false;
                        break;
                    }
                }
                //checks if the position you move to is empty or has an enemy piece
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
            else if (move[1] == move[3] && move[0] != move[2])
            {
                int start;
                int end;
                if (move[2] > move[0])
                {
                    start = move[0];
                    end = move[2];
                }
                else
                {
                    start = move[2];
                    end = move[0];
                }

                //checks if the column is empty
                for (int i = start + 1; i < end; i++)
                {
                    if (piecesBoard[i, move[1]] != null)
                    {
                        possible = false;
                        break;
                    }
                }
                //checks if the position you move to is empty or has an enemy piece
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
            if (isWhite == true)
            {
                piecesBoard = ReverseBoard(piecesBoard);
                move = ReverseMove(move);
            }
            return possible;
        }
        public bool CanMoveInDiagonalLine(ChessPiece[,] piecesBoard, int[] move, int turn)
        {
            bool possible = true;
            bool isWhite = PieceIsWhite();

            //Checks if the colur of the piece and the turn match up
            if (!IfTheMovingPieceIsInTheRightColourAndTurn(isWhite, turn))
                return false;

            if (isWhite)
            {
                piecesBoard = ReverseBoard(piecesBoard);
                move = ReverseMove(move);
            }

            if (move[0] - move[2] == move[1] - move[3])
            {
                int difference;
                int startRow;
                int startColumn;
                if (move[2] > move[0])
                {
                    difference = move[2] - move[0];
                    startRow = move[0];
                    startColumn = move[1];
                }
                else
                {
                    difference = move[0] - move[2];
                    startRow = move[2];
                    startColumn = move[3];
                }

                //checks if the diagonal line is empty
                for (int i = 1; i < difference; i++)
                {
                    if (piecesBoard[startRow + i, startColumn + i] != null && piecesBoard[startRow + i, startColumn + i] is Pawn == false)
                    {
                        possible = false;
                        break;
                    }
                    if (piecesBoard[startRow + i, startColumn + i] is Pawn)
                    {
                        if (!((Pawn)piecesBoard[startRow + i, startColumn + i]).GetEnPassant())
                        {
                            possible = false;
                            break;
                        }
                    }
                }
                //checks if the position you move to is empty or has an enemy piece
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
            else if (move[0] - move[2] == move[3] - move[1])
            {
                int difference;
                int startRow;
                int startColumn;
                if (move[2] > move[0])
                {
                    difference = move[2] - move[0];
                    startRow = move[0];
                    startColumn = move[1];
                }
                else
                {
                    difference = move[0] - move[2];
                    startRow = move[2];
                    startColumn = move[3];
                }

                //checks if the diagonal line is empty
                for (int i = 1; i < difference; i++)
                {
                    if (piecesBoard[startRow + i, startColumn - i] != null && piecesBoard[startRow + i, startColumn - i] is Pawn == false)
                    {
                        possible = false;
                        break;
                    }
                    if (piecesBoard[startRow + i, startColumn - i] is Pawn)
                    {
                        if (!((Pawn)piecesBoard[startRow + i, startColumn - i]).GetEnPassant())
                        {
                            possible = false;
                            break;
                        }
                    }
                }
                //checks if the position you move to is empty or has an enemy piece
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
            if (isWhite == true)
            {
                piecesBoard = ReverseBoard(piecesBoard);
                move = ReverseMove(move);
            }
            return possible;
        }


        public bool IfTheMovingPieceIsInTheRightColourAndTurn(bool isWhite, int turn)
        {
            //Checks if the colur of the piece and the turn match up
            if (!isWhite)
            {
                //the piece that was choosen is black but it's the white's turn
                if (turn % 2 == 0)
                    return false;
            }
            else
            {
                //the piece that was choosen is white but it's the black's turn
                if (turn % 2 == 1)
                    return false;
            }
            return true;
        }

        public ChessPiece[,] ReverseBoard(ChessPiece[,] piecesBoard)
        {
            ChessPiece[,] copyPiecesBoard = new ChessPiece[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    copyPiecesBoard[i, j] = piecesBoard[i, j];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    piecesBoard[i, j] = copyPiecesBoard[7 - i, 7 - j];
            return piecesBoard;
        }
        public int[] ReverseMove(int[] move)
        {
            for (int i = 0; i < move.Length; i++)
                move[i] = 7 - move[i];

            return move;
        }

        public override string ToString()
        {
            return isWhite?"W":"B";
        }
    }
}
