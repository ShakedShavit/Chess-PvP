using System;
using System.Collections.Generic;
using System.Text;

namespace ChessPvP
{
    class ChessUtilities
    {
        public ChessUtilities() { }

        public ChessPiece[,] TransformStringArrayToChessPieceArray(string[,] stringBoard)
        {
            ChessPiece[,] piecesBoard = new ChessPiece[8, 8];
            for (int i = 0; i < stringBoard.GetLength(0); i++)
                for (int j = 0; j < stringBoard.GetLength(1); j++)
                {
                    switch (stringBoard[i, j])
                    {
                        //empty
                        case "EE":
                            piecesBoard[i, j] = null;
                            break;
                        //pawn
                        case "WP":
                            piecesBoard[i, j] = new Pawn("white");
                            break;
                        case "BP":
                            piecesBoard[i, j] = new Pawn("black");
                            break;
                        //rook
                        case "WR":
                            piecesBoard[i, j] = new Rook("white");
                            break;
                        case "BR":
                            piecesBoard[i, j] = new Rook("black");
                            break;
                        //knight
                        case "WN":
                            piecesBoard[i, j] = new Knight("white");
                            break;
                        case "BN":
                            piecesBoard[i, j] = new Knight("black");
                            break;
                        //bishop
                        case "WB":
                            piecesBoard[i, j] = new Bishop("white");
                            break;
                        case "BB":
                            piecesBoard[i, j] = new Bishop("black");
                            break;
                        //queen
                        case "WQ":
                            piecesBoard[i, j] = new Queen("white");
                            break;
                        case "BQ":
                            piecesBoard[i, j] = new Queen("black");
                            break;
                        //king
                        case "WK":
                            piecesBoard[i, j] = new King("white");
                            break;
                        case "BK":
                            piecesBoard[i, j] = new King("black");
                            break;
                    }
                }
            return piecesBoard;
        }

        public bool IsInputValid(string input)
        {
            if (input.Length != 4)
                return false;
            for (int i = 0; i < input.Length; i++)
            {
                //The char is a letter
                if (i % 2 == 0)
                    if (input[i] != 'A' && input[i] != 'B' && input[i] != 'C' && input[i] != 'D' && input[i] != 'E' && input[i] != 'F' && input[i] != 'G' && input[i] != 'H')
                        return false;
                //The char is a number
                if (i % 2 != 0)
                    if (input[i] != '1' && input[i] != '2' && input[i] != '3' && input[i] != '4' && input[i] != '5' && input[i] != '6' && input[i] != '7' && input[i] != '8')
                        return false;
            }
            return true;
        }
        public int[] TransformTextToPosition(string input)
        {
            int[] move = new int[4];
            input = input.ToUpper();
            for (int i = 0; i < input.Length; i++)
            {
                //The char is a letter
                if (i % 2 == 0)
                {
                    switch (input[i])
                    {
                        case 'A':
                            move[i] = 0;
                            break;
                        case 'B':
                            move[i] = 1;
                            break;
                        case 'C':
                            move[i] = 2;
                            break;
                        case 'D':
                            move[i] = 3;
                            break;
                        case 'E':
                            move[i] = 4;
                            break;
                        case 'F':
                            move[i] = 5;
                            break;
                        case 'G':
                            move[i] = 6;
                            break;
                        case 'H':
                            move[i] = 7;
                            break;
                    }
                }
                //The char is a number
                else
                    move[i] = int.Parse(input[i].ToString()) - 1;
            }
            //Reverses the column and the row => (move[row1, column1, row2, colum2)
            int holder = move[0];
            move[0] = move[1];
            move[1] = holder;
            holder = move[2];
            move[2] = move[3];
            move[3] = holder;

            return move;
        }
        
        public bool isWhiteTurn(int turn)
        {
            if (turn % 2 == 0)
                return true;
            return false;
        }

        public int[] KingPosition(ChessPiece[,] chessPiecesBoard, int turn)
        {
            int[] kingPosition = new int[2];

            bool isWhite;
            if (turn % 2 == 0)
                isWhite = true;
            else
                isWhite = false;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (chessPiecesBoard[i, j] != null)
                    {
                        if (chessPiecesBoard[i, j] is King && chessPiecesBoard[i, j].PieceIsWhite() == isWhite)
                        {
                            kingPosition[0] = i;
                            kingPosition[1] = j;
                            return kingPosition;
                        }
                    }
                }
            }
            //Won't happen
            return kingPosition;
        }

    }
}
