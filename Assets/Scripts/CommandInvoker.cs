using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Chackers3D.Assets.Scripts
{
    public class CommandInvoker
    {
        // stack of cvommand objects to undo
        private static Stack<ICommand> _undoStack = new Stack<ICommand>();

        // execute a command object directly and save to the undo
        public static void  ExecuteCommand(ICommand command)
        {
            command.Execute();
            _undoStack.Push(command);
        }

        public static void UndoCommand()
        {
            if (_undoStack.Count > 0)
            {
                ICommand activeCommand = _undoStack.Pop();
                activeCommand.Undo();
            }
        }
    }
}