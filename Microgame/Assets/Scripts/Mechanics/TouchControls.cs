using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class TouchControls : MonoBehaviour
    {
        private PlayerController player;

        void Start()
        {
            player = FindObjectOfType<PlayerController>();
        }

        public void LeftArrow()
        {
            if(player.controlEnabled && player.touchControlEnabled)
                player.Move(-1);
        }
        public void RightArrow()
        {
            if (player.controlEnabled && player.touchControlEnabled)
                player.Move(1);
        }
        public void UnPressArrow()
        {
            if (player.controlEnabled && player.touchControlEnabled)
                player.Move(0);
        }
        public void Jump(bool up)
        {
            if (player.controlEnabled && player.touchControlEnabled)
                player.JumpTouch(up);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}