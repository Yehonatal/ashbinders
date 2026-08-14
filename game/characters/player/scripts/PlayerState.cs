namespace Ashbinders.Characters.Player;

public enum PlayerState
{
    Grounded,    // Normal 8-way movement, attack, interaction
    Dashing,     // High-speed invulnerable kinetic dash
    Jumping,     // Airborne parabolic leap over gaps/obstacles
    Climbing,    // Attached to vertical ladder or scaffolding
    Swimming,    // Navigating flooded canals and basins
    Hurt,        // Knockback hitstun
    Interacting  // In dialogue or operating a heavy ancient machine
}
