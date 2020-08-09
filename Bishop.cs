using System;
using System.Collections.Generic;
using System.Text;

namespace ChessPvP
{
    class Bishop : ChessPiece
    {
        public Bishop(string colour)
            : base(colour)
        {

        }
        public override bool CanMoveTo(ChessPiece[,] piecesBoard, int[] move, int turn)
        {
            return base.CanMoveInDiagonalLine(piecesBoard, move, turn);
        }
        public override string ToString()
        {
            return base.ToString() + "B";
        }
    }
}
