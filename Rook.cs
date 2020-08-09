using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChessPvP
{
    class Rook : ChessPiece
    {
        bool moved;
        public Rook(string colour)
            : base(colour)
        {
            moved = false;
        }
        public Rook(string colour, bool moved)
            : base(colour)
        {
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
            return base.CanMoveInStraightLine(piecesBoard, move, turn);
        }
        public override string ToString()
        {
            return base.ToString() + "R";
        }
    }
}
