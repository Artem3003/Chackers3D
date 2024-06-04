using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chackers3D.Assets.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource stepSound;
        [SerializeField] private AudioSource disapperSound;

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
