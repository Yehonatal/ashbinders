# Git & Pull Request Workflow

## 1. Branching Strategy
- **`main`**: Always clean, buildable, tested, and deployable.
- **`feature/<name>`**: For new gameplay features, systems, and content (e.g. `feature/player-controller`, `feature/motion-ember`).
- **`fix/<name>`**: For bug fixes (e.g. `fix/save-file-corruption`, `fix/hitbox-overlap`).
- **`refactor/<name>`**: For architectural cleanups without feature changes.

## 2. Pull Request Standards
Even in a 2-engineer team, every PR must:
1. Pass all automated CI checks (build, test, lint, asset validation).
2. Include a concise summary of what was changed and how it was verified.
3. Be reviewed and approved before merging into `main`.
4. Squash-and-merge or rebase to keep history clean.
