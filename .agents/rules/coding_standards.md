# Ashbinders Coding Standards

## 1. Language & Framework
- **Runtime**: .NET 8.0 / C# 12
- **Engine**: Godot 4.x
- **Nullability**: Nullable reference types enabled (`<Nullable>enable</Nullable>`). Every nullable reference must be marked with `?`.

## 2. Naming Conventions
- **Classes, Enums, Structs, Interfaces**: `PascalCase` (`PlayerController`, `IInteractable`, `DamageType`).
- **Interfaces**: Must start with `I` (`IDamageable`, `ISaveable`).
- **Methods and Properties**: `PascalCase` (`TakeDamage()`, `CurrentHealth`).
- **Private Fields**: `_camelCase` (`_healthComponent`, `_currentEmber`).
- **Local Variables & Parameters**: `camelCase` (`moveSpeed`, `inputVector`).
- **Constants & Static Readonly**: `PascalCase` (`DefaultMaxHealth`, `ActionMoveLeft`).

## 3. Formatting
- 4 spaces indentation (no tabs).
- Braces on new lines (Allman style).
- Always use file-scoped namespaces (`namespace Ashbinders.Core.Save;`).

## 4. Error Handling & Defensive Programming
- Never swallow exceptions silently. Log meaningful diagnostic messages with `GameLogger.LogError()`.
- Use assertions in debug builds to catch impossible states early (`System.Diagnostics.Debug.Assert`).
- Validate external and node inputs at public boundaries.
