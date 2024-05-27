using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Chackers3D.Assets.Scripts
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}