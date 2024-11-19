using BepInEx;
using UnityEngine;
using EFT;

namespace AnimationReplacer.Features
{
    [BepInPlugin("com.boogle.animationreplacer.blockfirearmonsprint", "12.11AnimationReplacer", "1.0.0")]
    public class BlockFirearmOnSprint : BaseUnityPlugin
    {
        private Player _player;
        private MovementContext _movementContext;
        private bool _previousSprintState;

        public bool Enabled { get; set; } = true;

        private void Update()
        {
            if (!Enabled) return;

            if (_player == null || _movementContext == null)
            {
                // Attempt to get the player and movement context if they are not yet cached
                _player = GetLocalPlayer();
                if (_player != null)
                {
                    _movementContext = _player.MovementContext;
                }
            }

            if (_movementContext != null)
            {
                bool isSprinting = _movementContext.IsSprintEnabled;

                // Only update BlockFirearms if the sprint state has changed
                if (isSprinting != _previousSprintState)
                {
                    SetBlockFirearms(isSprinting);
                    _previousSprintState = isSprinting;
                }
            }
        }

        private Player GetLocalPlayer()
        {
            // Find and return the local player (finds the first instance in the scene)
            return FindObjectOfType<Player>();
        }

        private void SetBlockFirearms(bool block)
        {
            if (_movementContext != null)
            {
                _movementContext.BlockFirearms = block;
            }
        }
    }
}
