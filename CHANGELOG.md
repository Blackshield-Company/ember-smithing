# Changelog

All notable changes to this project are documented here. Versions follow [Semantic Versioning](https://semver.org/).

## [0.1.0] - 2026-10-06

Early beta. First public release. It passes every automated check in the build (plugin validation, vanilla reference check, round trip, FOMOD schema) but has not been playtested in-game yet. Please report problems on the Nexus page or on GitHub.

### Added
- Heat-tempering at the forge for 42 vanilla weapons (Iron, Steel, Orcish, Dwarven, Elven, Glass and Ebony swords, war axes, maces, greatswords, battleaxes and warhammers), in three tiers:
  - Heat-Tempered: +10% base damage (at least +1). Smithing 20. Weapon + 1 ingot + 2 firewood.
  - Ember-Forged: +20% (at least +2). Smithing 45. Heat-Tempered weapon + 1 ingot + 2 charcoal.
  - Dragonfire: +30% (at least +3). Smithing 70. Ember-Forged weapon + 2 ingots + 1 fire salts.
- Engraving at the forge for 17 vanilla armor pieces (Iron, Steel, Dwarven, Orcish, Ebony, Hide, Leather, Elven and Glass cuirasses, and their shields where vanilla has one):
  - Ember-Engraved: Resist Fire 20%. Piece + 1 silver ingot + 1 fire salts.
  - Hearth-Engraved: Resist Frost 20%. Piece + 1 silver ingot + 1 frost salts.
  - Bellows-Engraved: Stamina regenerates 25% faster. Piece + 1 silver ingot + 2 charcoal.
  - Smithing 30. Engraved pieces can't be disenchanted.
- The usual material perk is required for each recipe (Steel Smithing for steel, and so on), and a recipe only appears while you carry the item it starts from.
- Every new item can be improved at grindstones and workbenches with its usual material, following the vanilla rules (enchanted items need Arcane Blacksmith).
- No scripts. ESL-flagged plugin, 537 new records, Skyrim.esm only.
