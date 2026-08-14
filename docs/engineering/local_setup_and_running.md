# Local Development and Execution Guide

This document outlines the step-by-step procedure to set up, build, test, and run the **Ashbinders** game client and development tools locally.

---

## 1. System Prerequisites

1. **.NET 8.0 SDK**:
   - Download and install [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
   - Verify installation:
     ```bash
     dotnet --version
     ```
     Expected output: `8.0.x` or higher.

2. **Godot Engine 4.x (C# / .NET Edition)**:
   - Download **Godot Engine 4.3+ (.NET version)** from [godotengine.org](https://godotengine.org/download).
   - Standard Godot builds without C# support will not compile or run this project.

3. **Python 3.x** (for automation and asset validation scripts):
   - Verify installation:
     ```bash
     python3 --version
     ```

---

## 2. Initial Setup and Compilation

### Clone and Verify Working Directory
```bash
git clone https://github.com/your-org/ashbinders.git
cd ashbinders
```

### Build the C# Game Solution
Run the build script from the repository root:
```bash
./scripts/build.sh
```
Or execute MSBuild directly:
```bash
dotnet build game/Ashbinders.csproj -c Debug
```

---

## 3. Running Automated Tests

Execute the automated unit and integration tests covering the core architecture (`EventBus`, `StateMachine`, `SaveManager`):

```bash
./scripts/run_tests.sh
```

Expected output:
```text
=== Running Ashbinders Automated Test Suite ===
=================================================
    Ashbinders Core Systems Unit Test Runner     
=================================================

  [PASS] EventBus: Publish & Subscribe
  [PASS] EventBus: Unsubscribe
  [PASS] StateMachine: Transitions & Lifecycle
  [PASS] SaveManager: Serialization & Deserialization Fidelity

=================================================
Results: 4 Passed, 0 Failed
=================================================
```

---

## 4. Running Asset and Resource Validation

To check that all textures, audio clips, and data resources adhere to strict domain placement and `snake_case` naming rules:

```bash
python3 scripts/validate_assets.py
```

Expected output:
```text
--- Running Ashbinders Asset & Resource Validation ---
[OK] All assets and resources passed validation.
```

---

## 5. Running the Game in Godot

### Step 1: Open Godot (.NET Edition)
1. Launch the Godot 4 .NET executable.
2. In the Project Manager, click **Import**.
3. Browse to the `game/` folder inside this repository and select `project.godot`.
4. Click **Import & Edit**.

### Step 2: Build C# Assemblies inside Godot
- In the top-right corner of the Godot editor, click the **Build** hammer icon (or press `Alt + B`).
- Ensure the build completes with 0 errors.

### Step 3: Run the Main Scene
- Press **F5** (Play Project) or click the **Play** button in the top-right corner.
- Godot will load the configured starter scene: `res://world/regions/underlevels/gym_underlevels.tscn`.

### Step 4: Controls & Input Mappings
| Action | Keyboard | Gamepad |
| :--- | :--- | :--- |
| **Move Up / Down / Left / Right** | `W`, `S`, `A`, `D` | Left Stick / D-Pad |
| **Attack (Ashbinder Chain)** | `Left Mouse Button` / `J` | `X` / Square |
| **Dash** | `Space` / `K` | `A` / Cross / Left Trigger |
| **Interact / Extract Ember** | `E` | `Y` / Triangle |
| **Socket / Unsocket Ember** | `F` | `B` / Circle |

---

## 6. IDE Integration

### VSCode
1. Install extensions:
   - **C# Dev Kit** (`ms-dotnettools.csdevkit`)
   - **godot-tools** (`geequlim.godot-tools`)
2. Set C# solution root to `game/Ashbinders.csproj`.

### JetBrains Rider
1. Open the project root or `game/Ashbinders.csproj`.
2. Install the **Godot Support** plugin.
3. Set the Godot executable path under **Settings -> Languages & Frameworks -> Godot Engine**.
