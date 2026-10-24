# Nexus Mods page: Ember Smithing

Early beta: v0.1.0 passed every automated build check but hasn't been playtested in-game. The page says so up front. Exact upload-form values are in [NEXUS_FIELDS.md](NEXUS_FIELDS.md).

## Page settings

- **Game:** Skyrim Special Edition
- **Mod name:** Ember Smithing
- **Version:** 0.1.0 (early beta)
- **Category:** Weapons and Armour
- **Suggested tags:** Crafting, Smithing, Weapons, Armour, Lore-Friendly, ESL, No SKSE, No Scripts
- **Requirements:** none.
- **Permissions (suggested):** source on GitHub under MIT, so allow modification and conversions with credit. Don't allow re-uploads of the archive itself.
- **File to upload:** `EmberSmithing-0.1.0.zip` (FOMOD), main file, named "Ember Smithing".

## Description (paste into the BBCode editor)

```bbcode
[center][size=6][b]EMBER SMITHING[/b][/size]
[size=3][i]Back into the fire. Heat-temper your blades, engrave your armor.[/i][/size]

[size=3][color=#e0a030][b]EARLY BETA (v0.1.0)[/b]: this version passes every automated build check but hasn't been playtested in-game yet. Please report bugs in the Bugs tab or on GitHub.[/color][/size][/center]

[line]

[size=4][b]Summary[/b][/size]
More to do at the forge, with no scripts at all. Put vanilla weapons back into the fire for three tiers of [b]heat-tempering[/b], each hitting harder than the last, and [b]engrave[/b] vanilla armor and shields with protective runes. Everything is crafted at the vanilla forges and can be improved at the vanilla grindstones and workbenches.

[size=4][b]Heat-tempering[/b][/size]
42 vanilla weapons: Iron, Steel, Orcish, Dwarven, Elven, Glass and Ebony swords, war axes, maces, greatswords, battleaxes and warhammers.
[list]
[*][b]Heat-Tempered[/b]: +10% base damage (at least +1). Smithing 20. The weapon, 1 ingot, 2 firewood.
[*][b]Ember-Forged[/b]: +20% (at least +2). Smithing 45. The Heat-Tempered weapon, 1 ingot, 2 charcoal.
[*][b]Dragonfire[/b]: +30% (at least +3). Smithing 70. The Ember-Forged weapon, 2 ingots, 1 fire salts.
[/list]
Each recipe also needs the material's smithing perk. Weight, speed and reach stay vanilla, and every tier can still be sharpened at a grindstone and enchanted.

[size=4][b]Engraving[/b][/size]
17 vanilla pieces: Iron, Steel, Dwarven, Orcish, Ebony, Hide, Leather, Elven and Glass armor, plus their shields.
[list]
[*][b]Ember-Engraved[/b]: Resist Fire 20%. 1 silver ingot, 1 fire salts.
[*][b]Hearth-Engraved[/b]: Resist Frost 20%. 1 silver ingot, 1 frost salts.
[*][b]Bellows-Engraved[/b]: Stamina regenerates 25% faster. 1 silver ingot, 2 charcoal.
[/list]
Smithing 30 plus the material's perk. Armor rating stays vanilla; engraved pieces count as enchanted (Arcane Blacksmith to improve, can't be disenchanted).

[size=4][b]Clean forge menu[/b][/size]
A recipe only appears while you carry the item it starts from, so the forge doesn't fill up with hundreds of entries you can't use.

[size=4][b]Requirements[/b][/size]
[list]
[*]Skyrim Special Edition or Anniversary Edition (1.5.97 or 1.6.x). Works on Steam under Proton on Linux.
[*]Nothing else. No SKSE, no SkyUI, no DLC, no Creation Club content.
[/list]

[size=4][b]Installation[/b][/size]
Install with [b]Mod Organizer 2[/b] or [b]Vortex[/b] (FOMOD installer) and enable [b]EmberSmithing.esp[/b]. It's ESL-flagged, so it doesn't take a full load-order slot. No scripts: safe to add at any time.

[size=4][b]Compatibility[/b][/size]
[list]
[*]No edits to vanilla records. Everything is new records: weapons, armor, enchantments and recipes. Skyrim.esm is the only master.
[*]Vanilla recipes are untouched, so smithing overhauls keep working for vanilla items. The new items use vanilla stats as their base, so overhauls that change vanilla weapon or armor stats won't apply to them.
[*]Retextures and mesh replacers for the vanilla items apply to the new items too.
[*]Fully compatible with [b]Blackshield Company[/b], [b]Daedric Ledger: Bounties of Oblivion[/b], [b]Contract Hunter[/b] and [b]Steel Oath[/b] (same author).
[/list]

[size=4][b]Uninstalling[/b][/size]
No scripts, so you can remove it any time. Items made with it disappear.

[size=4][b]Known issues / early beta limits[/b][/size]
[list]
[*]Not playtested in-game yet. Found something? Please post it in the Bugs tab.
[*]A recipe uses up one copy of the starting item. If you carry several (say, one improved and one plain), stash the one you want to keep.
[*]Not every vanilla item is covered yet: no daggers, bows, Daedric weapons, helmets, gauntlets or boots in this version.
[*]The tiers look like the vanilla item. The name tells them apart.
[/list]

[size=4][b]Roadmap[/b][/size]
Daggers, bows and Daedric weapons, engravings for helmets, gauntlets and boots, and optional patches for popular smithing overhauls.

[size=4][b]Source[/b][/size]
Open source (MIT): [url=https://github.com/Blackshield-Company/ember-smithing]github.com/Blackshield-Company/ember-smithing[/url]

[size=4][b]Credits[/b][/size]
[list]
[*][url=https://github.com/Mutagen-Modding/Mutagen]Mutagen[/url] by Noggog and contributors, which generates the plugin from code.
[*][url=https://en.uesp.net]UESP[/url] for the vanilla weapon and armor stats.
[*][url=https://github.com/GandaG/fomod-schema]FOMOD schema[/url] by GandaG.
[*]Bethesda Game Studios for Skyrim.
[/list]
```

## Screenshots to capture in-game

Take these at 16:9, with the HUD hidden for scenery shots (`tm` in the console toggles menus).

1. **Hero shot.** A character at a glowing forge holding a Dragonfire greatsword. This goes on the mod tile.
2. **Forge menu.** The Steel category showing a Heat-Tempered and an Ember-Forged recipe with their ingredients.
3. **Tier comparison.** Inventory view of a vanilla and a Dragonfire weapon of the same type, showing damage.
4. **Engraving recipe.** An Ember-Engraved armor recipe at the forge.
5. **Engraved armor.** Inventory preview of an engraved piece showing the enchantment text.
6. **Grindstone.** Improving a Dragonfire weapon.
7. **In use.** Fighting a fire-breathing dragon in Ember-Engraved armor.
