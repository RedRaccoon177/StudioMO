using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void EnterState(PlayerController player);
    public void UpdateState(PlayerController player);
    public void FixedUpdateState(PlayerController player);
    public void CheckNowState(PlayerController player);
}

public enum PlayerStateName
{
    Idle, Interact, Move, Slipstream,
    Groggy, Invincible, Knockback, Solid
}