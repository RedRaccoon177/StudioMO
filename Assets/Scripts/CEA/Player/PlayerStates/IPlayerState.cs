using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void EnterState(Player_Test player);
    public void UpdateState(Player_Test player);
    public void FixedUpdateState(Player_Test player);
    public void CheckNowState(Player_Test player);
}

public enum PlayerStateName
{
    Idle, Interact, Move, SlowMotion,
    Groggy, Invincible, Knockback, Solid
}