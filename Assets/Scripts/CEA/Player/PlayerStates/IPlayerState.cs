using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void EnterState(Player player);
    public void UpdateState(Player player);
    public void FixedUpdateState(Player player);
    public void CheckNowState(Player player);
}

public enum PlayerStateName
{
    Idle, Interact, Move, SlowMotion,
    Groggy, Invincible, Knockback, Solid
}