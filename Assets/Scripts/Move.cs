using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Chackers3D.Assets.Scripts
{
    public struct Move
    {
        public Vector2 start;
        public Vector2 end;
        public Piece movedPiece;
        public Piece capturedPiece;
    }
}