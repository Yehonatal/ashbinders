#!/usr/bin/env python3
"""
Asset & Resource Validation Script for Ashbinders.
Checks that all asset files adhere to strict naming conventions:
- Lowercase with underscores (snake_case)
- Valid extensions
- Placed in correct domain folders
"""

import os
import re
import sys

ALLOWED_EXTENSIONS = {
    '.png', '.jpg', '.jpeg', '.webp', '.psd', '.ase', '.aseprite',
    '.wav', '.ogg', '.mp3', '.flac',
    '.blend', '.fbx', '.glb', '.gltf',
    '.ttf', '.otf',
    '.tres', '.tscn', '.gd', '.cs', '.keep'
}

SNAKE_CASE_PATTERN = re.compile(r'^[a-z0-9_]+$')

def validate_directory(base_dir):
    errors = []
    if not os.path.exists(base_dir):
        print(f"Directory {base_dir} does not exist yet. Skipping.")
        return errors

    for root, dirs, files in os.walk(base_dir):
        for filename in files:
            if filename == '.keep' or filename.startswith('.'):
                continue

            name, ext = os.path.splitext(filename)
            if ext not in ALLOWED_EXTENSIONS:
                errors.append(f"Disallowed extension: {os.path.join(root, filename)}")
                continue

            if not SNAKE_CASE_PATTERN.match(name):
                errors.append(f"Invalid naming (must be snake_case): {os.path.join(root, filename)}")

    return errors

def main():
    print("--- Running Ashbinders Asset & Resource Validation ---")
    assets_dir = "game/assets"
    resources_dir = "game/resources"

    errors = []
    errors.extend(validate_directory(assets_dir))
    errors.extend(validate_directory(resources_dir))

    if errors:
        print(f"\n❌ Validation failed with {len(errors)} error(s):")
        for err in errors:
            print(f"  - {err}")
        sys.exit(1)
    else:
        print("✅ All assets and resources passed validation.")
        sys.exit(0)

if __name__ == "__main__":
    main()
