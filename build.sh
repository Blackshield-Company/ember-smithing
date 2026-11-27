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

# Lowercase the Data folders the game opens by lowercase name: scripts, seq, sound/voice
# (and the rest of sound/). On Linux (Proton) the lookup prefers an exact-case folder, so a
# mod manager that deploys our "Scripts" next to another mod's "scripts" leaves ours unread.
for opt in "$STAGE"/*/; do
  if [[ -d "${opt}Scripts" ]]; then mv "${opt}Scripts" "${opt}scripts"; fi
  if [[ -d "${opt}SEQ" ]]; then mv "${opt}SEQ" "${opt}seq"; fi
  if [[ -d "${opt}Sound" ]]; then mv "${opt}Sound" "${opt}sound"; fi
  if [[ -d "${opt}sound/Voice" ]]; then mv "${opt}sound/Voice" "${opt}sound/voice"; fi
done

echo "== FOMOD"
ARCHIVE="$DIST_DIR/$NAME-$VERSION.zip"
python3 "$KIT/fomod/pack.py" --fomod fomod --stage "$STAGE" --version "$VERSION" --out "$ARCHIVE"
(cd "$DIST_DIR" && sha256sum "$(basename "$ARCHIVE")" > "$(basename "$ARCHIVE").sha256")
echo "== Done: $ARCHIVE"
