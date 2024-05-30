using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chackers3D.Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {

        private void Start()
        {
            // Register AudioManager with the Service Locator
            ServiceLocator.Instance.RegisterService(this);     
        }
    }
}
