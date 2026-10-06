#!/usr/bin/env bash
# Build Ember Smithing: generate the plugin, package the FOMOD.
# This mod has no scripts: everything is records (weapons, armor, enchantments, recipes).
#
#   ./build.sh                     # everything, archive goes to ./dist
#   DIST_DIR=/some/dir ./build.sh
#
# Needs: bash, python3, .NET SDK 9, xmllint (optional, for FOMOD schema validation).
set -euo pipefail
cd "$(dirname "${BASH_SOURCE[0]}")"

NAME="EmberSmithing"
VERSION="$(sed -n 's/^## \[\([0-9][0-9.]*\)\].*/\1/p' CHANGELOG.md | head -1)"
DIST_DIR="${DIST_DIR:-dist}"
KIT="tools/modkit"
BUILD="build"
STAGE="$BUILD/stage"
export SOURCE_DATE_EPOCH="${SOURCE_DATE_EPOCH:-$(git log -1 --format=%ct 2>/dev/null || echo 315532800)}"

echo "== $NAME $VERSION"
rm -rf "$STAGE" "$BUILD/no-scripts"
mkdir -p "$STAGE/00 Core" "$BUILD/no-scripts" "$DIST_DIR"

echo "== Plugin"
dotnet run --project src/plugin/EmberSmithing.Generator -c Release -- \
  --out "$BUILD/plugin" --ids src/plugin/formids.json --data src/plugin/vanilla-items.json --papyrus "$BUILD/no-scripts"
python3 "$KIT/esp_check.py" "$BUILD/plugin/$NAME.esp"
cp "$BUILD/plugin/$NAME.esp" "$STAGE/00 Core/"

echo "== FOMOD"
ARCHIVE="$DIST_DIR/$NAME-$VERSION.zip"
python3 "$KIT/fomod/pack.py" --fomod fomod --stage "$STAGE" --version "$VERSION" --out "$ARCHIVE"
(cd "$DIST_DIR" && sha256sum "$(basename "$ARCHIVE")" > "$(basename "$ARCHIVE").sha256")
echo "== Done: $ARCHIVE"
