using System;
using System.Collections.Generic;
using System.Text;

namespace ChessPvP
{
    class Queen : ChessPiece
    {
        public Queen(string colour)
            : base(colour)
        {

        }
        public override bool CanMoveTo(ChessPiece[,] piecesBoard, int[] move, int turn)
        {
            int[] moveCopy = new int[4];
            for (int i = 0; i < 4; i++)
                moveCopy[i] = move[i];
            if (base.CanMoveInStraightLine(piecesBoard, move, turn))
                return true;
            return base.CanMoveInDiagonalLine(piecesBoard, move, turn);
        }
        public override string ToString()
        {
            return base.ToString() + "Q";
        }
    }
}
