# Gameplay Specification: Combat & The Ashbinder Chain

## 1. Combat Philosophy
Combat in Ashbinders is tactical, kinetic, and directly tied to the Ember system. Rather than carrying generic swords or bows, Kael wields the ancient **Ashbinder Chain** — a modular, mid-range chain-whip/kusarigama weapon.

---

## 2. Modular Weapon Heads

The Ashbinder Chain maintains consistent reach physics, but swapping its head alters movesets, speeds, and damage profiles:

| Weapon Head | Range | Attack Speed | Damage Profile | Special Property |
| :--- | :--- | :--- | :--- | :--- |
| **Blade Head** | Medium (72 px) | Balanced (0.35s) | Medium (15 dmg) | Clean horizontal slash; starter head |
| **Hammer Head** | Short (54 px) | Slow (0.65s) | Heavy (32 dmg) | High poise damage; shatters armor/stone |
| **Twin Sickles** | Short (48 px) | Fast (0.20s) | Light (8 dmg × 2) | Multi-hit combo; rapid repositioning |
| **Spear Tip** | Long (110 px) | Medium (0.45s) | Piercing (22 dmg) | Long linear thrust; pierces through line |

---

## 3. Hitbox & Damage Framework
- **Active Frames**: Hitbox components activate during defined animation frame intervals.
- **Hitstop (Impact Freeze)**: Every hit applies 0.05s of hitstop to amplify impact feel.
- **Knockback**: Calculated based on the attacker's facing vector and weapon poise stat.
