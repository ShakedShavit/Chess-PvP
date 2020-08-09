using System;
using System.Collections.Generic;
using System.Text;

namespace ChessPvP
{
    class ChessGame
    {
        //even value - white's turn, uneven value - black's turn 
        int turnCounter;
        List<ChessPiece[,]> gameBoards;
        List<int[]> moves;
        ChessUtilities utilities;

        public void InitiateNewGame()
        {
            utilities = new ChessUtilities();

            turnCounter = 0;

            //A list that holds all of the game boards
            gameBoards = new List<ChessPiece[,]>();
            moves = new List<int[]>();

            
            //Printing first initial board
            gameBoards.Add(utilities.TransformStringArrayToChessPieceArray(CreatingInitialBoard()));
            PrintBoard();


            while (!IsCheckmateOrStalemate())
            {
                #region Draw Check
                if (InsufficientMaterialDraw())
                {
                    Console.WriteLine("Draw - Insufficient Material Rule");
                    break;
                }
                if (ThreefoldRepetitionRule())
                {
                    Console.WriteLine("Draw - Threefold Repetition Rule");
                    break;
                }
                if (FiftyMoveRule(moves))
                {
                    Console.WriteLine("Draw - Fifty Move Rule");
                    break;
                }
                #endregion

                Console.WriteLine();
                Console.WriteLine("Please enter the position of the peice you would like to move, and the position that you would like it to move to and press ENTER");
                string input = Console.ReadLine();

                while (!IsMovePossible(input))
                {
                    Console.WriteLine();
                    Console.WriteLine("That move was illegal, please enter another move and press ENTER");
                    input = Console.ReadLine();
                }
                int[] moveCopy = utilities.TransformTextToPosition(input);
                moves.Add(moveCopy);
                ChessPiece[,] pieceBoardCopy = new ChessPiece[8, 8];
                pieceBoardCopy = CreateChessPieceBoardCopy();

                #region Check if it's a Pawn moved to the last line
                if (moveCopy[2] == 0 || moveCopy[2] == 7)
                {
                    if (pieceBoardCopy[moveCopy[0], moveCopy[1]] is Pawn)
                    {
                        Console.WriteLine("Which chess piece would you like the Pawn to turn to: Pawn? Rook? Knight? Bishop? Queen?");
                        string pieceInput = Console.ReadLine();
                        pieceInput = pieceInput.ToUpper();
                        while (pieceInput != "PAWN" && pieceInput != "ROOK" && pieceInput != "KNIGHT" && pieceInput != "BISHOP" && pieceInput != "QUEEN")
                        {
                            Console.WriteLine("Invalid imput! These are your choices: Pawn, Rook, Knight, Bishop, Queen");
                            pieceInput = Console.ReadLine();
                            pieceInput = pieceInput.ToUpper();
                        }
                        bool isWhite = pieceBoardCopy[moveCopy[0], moveCopy[1]].PieceIsWhite();
                        switch (pieceInput)
                        {
                            case "ROOK":
                                pieceBoardCopy[moveCopy[0], moveCopy[1]] = new Rook(isWhite?"white":"black", true);
                                break;
                            case "KNIGHT":
                                pieceBoardCopy[moveCopy[0], moveCopy[1]] = new Knight(isWhite ? "white" : "black");
                                break;
                            case "BISHOP":
                                pieceBoardCopy[moveCopy[0], moveCopy[1]] = new Bishop(isWhite ? "white" : "black");
                                break;
                            case "QUEEN":
                                pieceBoardCopy[moveCopy[0], moveCopy[1]] = new Queen(isWhite ? "white" : "black");
                                break;
                        }
                    }
                }
                #endregion
                #region Check if it's Castling and moving the Rook
                if (moveCopy[2] == 7 || moveCopy[2] == 0)
                    if (pieceBoardCopy[moveCopy[0], moveCopy[1]] is King)
                        if (moveCopy[3] - moveCopy[1] == 2 || moveCopy[1] - moveCopy[3] == 2)
                        {
                            if (moveCopy[3] > moveCopy[1])
                            {
                                ((Rook)pieceBoardCopy[moveCopy[2], 7]).SetMoved(true);
                                pieceBoardCopy[moveCopy[2], moveCopy[3] - 1] = pieceBoardCopy[moveCopy[2], 7];
                                pieceBoardCopy[moveCopy[2], 7] = null;
                            }
                            else
                            {
                                ((Rook)pieceBoardCopy[moveCopy[2], 0]).SetMoved(true);
                                pieceBoardCopy[moveCopy[2], moveCopy[3] + 1] = pieceBoardCopy[moveCopy[2], 0];
                                pieceBoardCopy[moveCopy[2], 0] = null;
                            }
                        }
                #endregion


                //Checks if there is an eating of a EnPassant Pawn, if so than delete the original Pawn
                if (pieceBoardCopy[moveCopy[0], moveCopy[1]] is Pawn)
                    if (pieceBoardCopy[moveCopy[2], moveCopy[3]] is Pawn)
                        if (((Pawn)(pieceBoardCopy[moveCopy[2], moveCopy[3]])).GetEnPassant())
                            pieceBoardCopy[moves[turnCounter - 1][2], moves[turnCounter - 1][3]] = null;

                //If one the moving piece is King or Rook change their moved value to true
                if (pieceBoardCopy[moveCopy[0], moveCopy[1]] is King)
                    pieceBoardCopy[moveCopy[0], moveCopy[1]] = new King(pieceBoardCopy[moveCopy[0], moveCopy[1]].PieceIsWhite() ? "white" : "black", true);
                else if (pieceBoardCopy[moveCopy[0], moveCopy[1]] is Rook)
                    pieceBoardCopy[moveCopy[0], moveCopy[1]] = new Rook(pieceBoardCopy[moveCopy[0], moveCopy[1]].PieceIsWhite() ? "white" : "black", true);

                //Executing the play and adding the new board to the list
                pieceBoardCopy[moveCopy[2], moveCopy[3]] = pieceBoardCopy[moveCopy[0], moveCopy[1]];
                pieceBoardCopy[moveCopy[0], moveCopy[1]] = null;
                gameBoards.Add(pieceBoardCopy);
                NextTrun();

                #region En Passant
                //If a Pawn moved two steps than create an additional Fake Pawn - EnPassant
                if (IfThereIsNeedToCreateEnPassantPawn(moveCopy))
                {
                    if (moveCopy[2] > moveCopy[0])
                        pieceBoardCopy[moveCopy[2] - 1, moveCopy[1]] = new Pawn(pieceBoardCopy[moveCopy[2], moveCopy[3]].PieceIsWhite()?"white":"black", true);
                    else
                        pieceBoardCopy[moveCopy[0] - 1, moveCopy[1]] = new Pawn(pieceBoardCopy[moveCopy[2], moveCopy[3]].PieceIsWhite() ? "white" : "black", true);
                    gameBoards[GetTurn()] = pieceBoardCopy;
                }
                //Deleting the EnPassant from the preivious board
                if (GetTurn() > 0)
                {
                    ChessPiece[,] previousBoard = new ChessPiece[8, 8];
                    previousBoard = CreateChessPieceBoardCopy(GetTurn() - 1);
                    for (int i = 0; i < 8; i++)
                        for (int j = 0; j < 8; j++)
                        {
                            if (previousBoard[i, j] is Pawn)
                                if (((Pawn)previousBoard[i, j]).GetEnPassant())
                                    previousBoard[i, j] = null;
                        }
                    gameBoards[GetTurn() - 1] = previousBoard;
                }
                #endregion

                PrintBoard();
            }
        }


        public bool IsMovePossible(string input)
        {
            //First check to see if input is valid
            input = input.ToUpper();
            if (!utilities.IsInputValid(input))
                return false;

            //The move the player put in
            int[] move = new int[4];
            move = utilities.TransformTextToPosition(input);
            int[] moveCopy = new int[4];
            moveCopy = utilities.TransformTextToPosition(input);


            ChessPiece[,] gamePieces = new ChessPiece[8, 8];
            gamePieces = GetChessPieceBoard();


            //If the choosen place is empty
            if (gamePieces[move[0], move[1]] == null)
                return false;
            if (gamePieces[move[0], move[1]] is Pawn)
                if (((Pawn)gamePieces[move[0], move[1]]).GetEnPassant())
                    return false;

            //If the move is Castling
            if (IsMoveCastlingPossible(move, gamePieces))
                return true;


            if (gamePieces[move[0], move[1]].CanMoveTo(gamePieces, move, GetTurn()))
            {
                ChessPiece[,] chessPiecesCopy = new ChessPiece[8, 8];
                chessPiecesCopy = CreateChessPieceBoardCopy();

                chessPiecesCopy[moveCopy[2], moveCopy[3]] = chessPiecesCopy[moveCopy[0], moveCopy[1]];
                chessPiecesCopy[moveCopy[0], moveCopy[1]] = null;

                if (!IsCheck(utilities.KingPosition(chessPiecesCopy, GetTurn()), chessPiecesCopy))
                    return true;
                else
                    Console.WriteLine("Check");
            }
            return false;
        }

        public bool IsMoveCastlingPossible(int[] move, ChessPiece[,] gamePieces)
        {
            //If the moving piece is King
            if (gamePieces[move[0], move[1]] is King == false)
                return false;
            //If the row of the moving piece and the location its moving to are the same
            //If the row of the moving piece is either 0 or 7
            //If the column of the moving piece is 4
            if (move[0] != move[2] || (move[0] != 0 && move[0] != 7) || move[1] != 4)
                return false;
            //If the moving piece moves only two squares to the right or the left
            if (move[3] - move[1] != 2 && move[1] - move[3] != 2)
                return false;
            //If the King moved
            if (((King)gamePieces[move[0], move[1]]).GetMoved() == true)
                return false;

            int start;
            int[] checkPos = new int[2];
            checkPos[0] = move[0];

            if (move[3] > move[1])
            {
                start = move[1];
                //If the rook moved
                if (gamePieces[move[0], move[1] + 3] is Rook == false)
                    return false;
                if (((Rook)gamePieces[move[0], move[1] + 3]).GetMoved() == true)
                    return false;
            }
            else
            {
                start = move[3] - 1;
                //If the rook moved
                if (gamePieces[move[0], move[1] - 4] is Rook == false)
                    return false;
                if (((Rook)gamePieces[move[0], move[1] - 4]).GetMoved() == true)
                    return false;
                
                //If it's a big castling than check all three positions rather than just the two
                if (gamePieces[move[0], start] != null)
                    return false;
                //If there is a threat on the position that is unique to the big castling
                checkPos[1] = start;
                if (IsCheck(checkPos, gamePieces))
                    return false;
            }

            if (gamePieces[move[0], start + 1] != null || gamePieces[move[0], start + 2] != null)
                return false;

            checkPos[1] = move[1];
            //If there is a Check before the move
            if (IsCheck(checkPos, gamePieces))
                return false;
            //If there is a threat on the position between the King and the Rook
            checkPos[1] = start + 1;
            if (IsCheck(checkPos, gamePieces))
                return false;
            //If there is a threat on the position that the King will end up in
            checkPos[1] = start + 2;
            if (IsCheck(checkPos, gamePieces))
                return false;
            
            Console.WriteLine("Castling");
            return true;
        }
        public bool IsCheck(int[] threatenedPiecePosition, ChessPiece[,] chessPiecesCopy)
        {
            int[] move = new int[4];
            move[2] = threatenedPiecePosition[0];
            move[3] = threatenedPiecePosition[1];
            NextTrun();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (chessPiecesCopy[i, j] != null)
                    {
                        move[0] = i;
                        move[1] = j;

                        //do not check threat by a king if the move is castling
                        if (chessPiecesCopy[i, j] is King && (i == 7 || i == 0) && (chessPiecesCopy[threatenedPiecePosition[0], threatenedPiecePosition[1]] == null) && (move[3] - move[1] == 2 || move[1] - move[3] == 2))
                            continue;

                        if (chessPiecesCopy[i, j].CanMoveTo(chessPiecesCopy, move, GetTurn()))
                        {
                            PreviousTrun();
                            return true;
                        }
                    }
                }
            }
            PreviousTrun();
            return false;
        }


        public bool IsCheckmateOrStalemate()
        {
            bool isWhiteTurn = utilities.isWhiteTurn(GetTurn());

            ChessPiece[,] piecesBoardCopy = new ChessPiece[8, 8];
            ChessPiece[,] piecesBoardSecondCopy = new ChessPiece[8, 8];
            piecesBoardCopy = CreateChessPieceBoardCopy();
            piecesBoardSecondCopy = CreateChessPieceBoardCopy();


            int[] kingPosition = new int[2];
            kingPosition = utilities.KingPosition(piecesBoardCopy, GetTurn());

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (piecesBoardCopy[i, j] != null)
                        if (piecesBoardCopy[i, j].PieceIsWhite() == isWhiteTurn)
                        {
                            int[] movePos = new int[4];
                            for (int k = 0; k < 8; k++)
                                for (int l = 0; l < 8; l++)
                                {
                                    movePos[0] = i;
                                    movePos[1] = j;
                                    movePos[2] = k;
                                    movePos[3] = l;
                                    if (piecesBoardCopy[i, j].CanMoveTo(piecesBoardCopy, movePos, GetTurn()))
                                    {
                                        if (piecesBoardCopy[i, j] is King)
                                        {
                                            kingPosition[0] = k;
                                            kingPosition[1] = l;
                                        }

                                        piecesBoardSecondCopy[k, l] = piecesBoardSecondCopy[i, j];
                                        piecesBoardSecondCopy[i, j] = null;
                                        if (!IsCheck(kingPosition, piecesBoardSecondCopy))
                                            return false;
                                        else
                                        {
                                            //Restoring the borad for the next check
                                            piecesBoardSecondCopy[i, j] = piecesBoardSecondCopy[k, l];
                                            piecesBoardSecondCopy[k, l] = null;
                                            kingPosition = utilities.KingPosition(piecesBoardCopy, GetTurn());
                                        }
                                    }
                                }
                        }
            WinLoseDrawMessages();
            return true;
        }


        public void NextTrun()
        {
            turnCounter++;
        }
        public void PreviousTrun()
        {
            turnCounter--;
        }
        public int GetTurn()
        {
            return turnCounter;
        }
        public List<ChessPiece[,]> GetGameBoards()
        {
            return gameBoards;
        }
        public ChessPiece[,] GetChessPieceBoard()
        {
            return GetGameBoards()[GetTurn()];
        }
        public ChessPiece[,] GetChessPieceBoard(int turn)
        {
            return GetGameBoards()[turn];
        }
        public ChessPiece[,] CreateChessPieceBoardCopy()
        {
            ChessPiece[,] chessPiecesCopy = new ChessPiece[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    chessPiecesCopy[i, j] = GetChessPieceBoard()[i, j];
            return chessPiecesCopy;
        }
        public ChessPiece[,] CreateChessPieceBoardCopy(int turn)
        {
            ChessPiece[,] chessPiecesCopy = new ChessPiece[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    chessPiecesCopy[i, j] = GetChessPieceBoard(turn)[i, j];
            return chessPiecesCopy;
        }
        public ChessPiece[,] CreateChessPieceBoardCopy(ChessPiece[,] chessPieceBoard)
        {
            ChessPiece[,] chessPiecesCopy = new ChessPiece[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    chessPiecesCopy[i, j] = chessPieceBoard[i, j];
            return chessPiecesCopy;
        }


        //If a pawn moved two squares, than return true so a 'fake' Pawn could be created (in the middle square)
        public bool IfThereIsNeedToCreateEnPassantPawn(int[] move)
        {
            ChessPiece[,] copyPieceBoard = new ChessPiece[8, 8];
            copyPieceBoard = CreateChessPieceBoardCopy();
            if (copyPieceBoard[move[2], move[3]] is Pawn == false)
                return false;
            if (move[2] - move[0] == 2 || move[0] - move[2] == 2)
                return true;
            return false;
        }

        public bool InsufficientMaterialDraw()
        {
            int knightCounter = 0;
            int bishopCounter = 0;
            //Only kings
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (GetChessPieceBoard()[i, j] != null)
                        if ((GetChessPieceBoard()[i, j] is King) == false)
                        {
                            if ((GetChessPieceBoard()[i, j] is Knight) == false)
                            {
                                if ((GetChessPieceBoard()[i, j] is Bishop) == false)
                                    return false;
                                else
                                    bishopCounter++;
                            }
                            else
                                knightCounter++;
                        }
            if (((knightCounter == 1 && bishopCounter == 0) || (knightCounter == 0 && bishopCounter == 1)) || (knightCounter == 0 && bishopCounter == 0))
                return true;

            return false;
        }
        public bool ThreefoldRepetitionRule()
        {
            int currentTurn = GetTurn();
            int sameBoardCounter = 0;
            ChessPiece[,] currentBoard = new ChessPiece[8, 8];
            currentBoard = CreateChessPieceBoardCopy();

            bool boardsAreNotTheSame = true;
            ChessPiece[,] boardCompare = new ChessPiece[8, 8];
            for (int i = currentTurn % 2; i < currentTurn; i += 2)
            {
                boardCompare = CreateChessPieceBoardCopy(i);

                for (int m = 0; m < 8; m++)
                {
                    for (int n = 0; n < 8; n++)
                    {
                        if (boardCompare[m, n] != currentBoard[m, n])
                        {
                            boardsAreNotTheSame = false;
                            break;
                        }
                    }
                }
                if (boardsAreNotTheSame)
                    sameBoardCounter++;

                boardsAreNotTheSame = true;
            }
            if (sameBoardCounter == 2)
                return true;
            return false;
        }
        public bool FiftyMoveRule(List<int[]> moves)
        {
            int maxMoves = 50;
            int currentTurn = GetTurn();
            if (currentTurn >= maxMoves)
            {
                for (int i = currentTurn - maxMoves; i < currentTurn; i++)
                    if (GetChessPieceBoard(i)[moves[i][0], moves[i][1]] is Pawn)
                        return false;
                return true;
            }
            return false;
        }


        public void WinLoseDrawMessages()
        {
            //Win Lose Draw messages
            if (!IsCheck(utilities.KingPosition(GetChessPieceBoard(), GetTurn()), GetChessPieceBoard()))
                Console.WriteLine("Draw - Stalemate Rule");
            else
            {
                if (GetTurn() % 2 == 0)
                    Console.WriteLine("Black's Player Wins!!!");
                else
                    Console.WriteLine("White's Player Wins!!!");
            }
        }
        

        public string[,] CreatingInitialBoard()
        {
            string[,] stringBoard = new string[8, 8];
            stringBoard[0, 0] = "BR";
            stringBoard[0, 1] = "BN";
            stringBoard[0, 2] = "BB";
            stringBoard[0, 3] = "BQ";
            stringBoard[0, 4] = "BK";
            stringBoard[0, 7] = "BR";
            stringBoard[0, 6] = "BN";
            stringBoard[0, 5] = "BB";
            stringBoard[7, 0] = "WR";
            stringBoard[7, 1] = "WN";
            stringBoard[7, 2] = "WB";
            stringBoard[7, 3] = "WQ";
            stringBoard[7, 4] = "WK";
            stringBoard[7, 7] = "WR";
            stringBoard[7, 6] = "WN";
            stringBoard[7, 5] = "WB";
            for (int i = 1; i < 7; i++)
                for (int j = 0; j < stringBoard.GetLength(1); j++)
                {
                    if (i == 1)
                    {
                        stringBoard[i, j] = "BP";
                        continue;
                    }
                    if (i == 6)
                    {
                        stringBoard[i, j] = "WP";
                        continue;
                    }   
                    stringBoard[i, j] = "EE";
                }
            return stringBoard;
        }

        public void PrintBoard()
        {
            ChessPiece[,] chessPiecesBoard = new ChessPiece[8, 8];
            chessPiecesBoard = GetChessPieceBoard();

            for (int i = 65; i < 65 + chessPiecesBoard.GetLength(0); i++)
                Console.Write("  " + Convert.ToChar(i));
            Console.WriteLine();

            for (int i = 0; i < chessPiecesBoard.GetLength(0); i++)
            {
                Console.Write(i + 1);
                for (int j = 0; j < chessPiecesBoard.GetLength(1); j++)
                {
                    if (chessPiecesBoard[i, j] == null)
                        Console.Write(" " + "EE");
                    else
                        Console.Write(" " + chessPiecesBoard[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
