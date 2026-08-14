# ADR-0007: 2.5D Isometric Traversal Mechanics and Layered Elevation Architecture

## Status
Accepted

## Context
Subterranean exploration in *Ashbinders* relies heavily on verticality (deep mining shafts in the Underlevels, vertical climbs in the Furnace Spire) and environmental fluid hazards (flooded canals in the Drowned Districts).

In a 2.5D Isometric projection, flat Cartesian 2D movement without a vertical traversal model creates rigid, flat levels and restricts puzzle design. We require a unified traversal architecture that supports **Jumping** (gap-crossing and elevation hops), **Climbing** (ladders and vertical scaffolding), and **Swimming / Wading** (water volumes and fluid navigation) while maintaining deterministic physics, event-driven decoupling, and decoupled visual skinning.

## Decision

### 1. Player Traversal State Machine (`PlayerState`)
Formalize Kael's lifecycle into an explicit, deterministic Finite State Machine:

```csharp
public enum PlayerState
{
    Grounded,    // Standard 8-way isometric movement, attack, interaction
    Dashing,     // High-speed kinetic dash with i-frames
    Jumping,     // Airborne parabolic arc over low obstacles and chasms
    Climbing,    // Attached to vertical ladder or scaffolding
    Swimming,    // Navigating water volumes with surface ripples and current forces
    Hurt,        // Hitstun and knockback
    Interacting  // Operating heavy ancient devices or dialogue
}
```

### 2. Parabolic Z-Axis Elevation & Ground Shadows
- **Jumping Curve**: Airborne height $Z(t)$ is calculated via a parabolic equation:
  $$Z(t) = 4 \cdot H_{max} \cdot \left(\frac{t}{T_{jump}}\right) \cdot \left(1 - \frac{t}{T_{jump}}\right)$$
- **Visual vs. Physics Decoupling**:
  - The root coordinate $(0, 0)$ and ground shadow remain anchored to the floor plane.
  - The visual node (`$Visuals`) is displaced upward: `Position.Y = -Z(t)`.
  - The drop shadow (`$DropShadow`) scales dynamically: `Scale = 1.0 - (Z(t) / H_max) * 0.35`.
  - During mid-air jump frames, the player's collision mask disables low obstacles (`Layer 7: LowObstacles`), permitting leaping over minecart tracks, pipes, and small chasms.

### 3. Volume-Based Environment Interactions
- **Climbing**: Triggered via `ClimbableVolume` (`Area2D` on `Layer 9: TraversalVolumes`). When inside the volume and pressing `W` / `S`, Kael attaches to the vertical rail, moving along the ladder axis. Reaching the top automatically steps onto the higher elevation ledge ($Z = 1$).
- **Water Volumes & Swimming**: Triggered via `WaterVolume` (`Area2D` on `Layer 8: WaterVolumes`):
  - **Shallow Wading**: Reduces speed by 25% and spawns circular isometric water ripples.
  - **Deep Swimming**: Shifts Kael into `PlayerState.Swimming`, lowers the visual baseline into the water mask, and converts dashes into water surges.

## Consequences
### Positive
- Fully enables vertical puzzle design in the Underlevels and water manipulation in the Drowned Districts.
- Preserves clean 2D physics without the performance overhead or complexity of a full 3D physics server.
- Visuals remain easily skin-swappable by artists in Phase 2.
