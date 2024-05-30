using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chackers3D.Assets.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioSource stepSound;
        [SerializeField] AudioSource disapperSound;

        private void Start()
        {
            // Register AudioManager with the Service Locator
            ServiceLocator.Instance.RegisterService(this);     
        }

        public void PlayStepSound()
        {
            stepSound.Play();
        }

        public void PlayDisapperSound()
        {
            disapperSound.Play();
        }
    }
}
