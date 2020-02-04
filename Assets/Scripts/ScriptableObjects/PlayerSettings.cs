﻿using UnityEngine;

[CreateAssetMenu(fileName = "Default Player", menuName = "Settings/Player", order = 1)]
public class PlayerSettings : ScriptableObject
{
    /// <summary>
    /// Maximum speed in unit/second that the character can moves.
    /// </summary>
    public float moveSpeed = 8;
    /// <summary>
    /// Height in unit that the player can jump
    /// </summary>
    public float jumpHeight = 4;
    /// <summary>
    /// Gravity in unit*unit/second
    /// </summary>
    public float gravity = 40;

    /// <summary>
    /// Max angle in degree the player can walk on
    /// </summary>
    public float maxClimbAngle = 60;
    /// <summary>
    /// Max angle in degree the player can descend
    /// </summary>
    public float maxDescendAngle = 60;

    /// <summary>
    /// Time in second needed for the player to reach max speed on the ground
    /// </summary>
    public float groundAccelerationTime = 0.07f;
    /// <summary>
    ///  Time in second needed for the player to stop themself on the ground
    /// </summary>
    public float groundDecelerationTime = 0.07f;
    /// <summary>
    ///  Time in second needed for the player to reach max speed while in the air
    /// </summary>
    public float airAccelerationTime = 0.14f;
    /// <summary>
    /// Time in second needed for the player to stop themself while the air
    /// </summary>
    public float airDecelerationTime = 0.14f;
}
