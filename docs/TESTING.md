# In-game test checklist (v0.1.0, early beta)

v0.1.0 ships as an early beta. It passed every automated build check but hasn't been played in-game yet. This checklist is for players and testers who want to confirm everything works, and for the next round of fixes.

Open the console with `~`. `player.additem` needs a full FormID, which depends on your load order. ESL-flagged plugins show up as `FE` + a 3-digit slot (`xxx`) + the record's own 3 digits. Find the slot with:

```
help "Heat-Tempered Steel Sword" 4      -> e.g. WEAP: (FE016809) 'Heat-Tempered Steel Sword'  (slot 016)
```

Local IDs for every item are in [ITEMS.md](ITEMS.md). Vanilla items you'll want for testing:

```
player.additem 00013989 1     (Steel Sword)
player.additem 00013952 1     (Steel Armor)
player.additem 00013955 1     (Steel Shield)
player.additem 0005ACE5 10    (Steel Ingot)
player.additem 0006F993 10    (Firewood)
player.additem 00033760 10    (Charcoal)
player.additem 0003AD5E 5     (Fire Salts)
player.additem 0003AD5F 5     (Frost Salts)
player.additem 0005ACE3 5     (Silver Ingot)
player.setav smithing 100
player.addperk 000CB40D       (Steel Smithing)
player.addperk 0005218E       (Arcane Blacksmith)
```

There are no scripts, so there is no Papyrus log to check. If something is wrong, note the item name, its FormID (from `help`), and what you expected.

---

## 0. Plugin loads
- [ ] `EmberSmithing.esp` is enabled and the game reaches the main menu, alone and with the other mods in the series.
- [ ] `help "Dragonfire" 4` lists the 42 Dragonfire weapons.

## 1. Heat-tempering
- [ ] At a forge with Smithing 15 and no perks, carrying an Iron Sword, firewood and an iron ingot: no Ember Smithing recipe shows.
- [ ] Smithing 20: **Heat-Tempered Iron Sword** shows under Iron and can be made. It uses the sword, 1 iron ingot and 2 firewood.
- [ ] Without the Iron Sword in your inventory, the recipe is not listed.
- [ ] With Smithing 45, carrying the Heat-Tempered sword, 1 iron ingot and 2 charcoal: **Ember-Forged Iron Sword**. With 70, 2 ingots and fire salts: **Dragonfire Iron Sword**.
- [ ] Steel weapons need Steel Smithing (and so on for each material); without the perk the recipe doesn't show.
- [ ] The new weapons look, sound and swing like the vanilla weapon. Damage matches ITEMS.md at 0 skill bonus (compare against the vanilla weapon's displayed damage). Weight is the same.
- [ ] Try one of each type: sword, war axe, mace, greatsword, battleaxe, warhammer. Two-handed ones use two-handed animations and count for Two-Handed perks.
- [ ] A grindstone improves each tier with the usual ingot. With the matching smithing perk, the improvement is doubled as for vanilla items.
- [ ] Enchanting a Dragonfire weapon at an arcane enchanter works.

## 2. Engraving
- [ ] Smithing 30, carrying Steel Armor, a silver ingot and fire salts (plus Steel Smithing): **Ember-Engraved Steel Armor** shows under Steel. Its effect reads "Increases fire resistance by 20%."
- [ ] **Hearth-Engraved** (frost salts) and **Bellows-Engraved** (2 charcoal) work the same way. Check Active Effects / the Magic menu's resistances while worn.
- [ ] Engraved armor looks the same as vanilla on male and female characters, and the shield works as a shield (blocking, bash).
- [ ] Engraved Hide, Leather, Elven and Glass pieces count as light armor; Iron, Steel, Dwarven, Orcish and Ebony as heavy.
- [ ] At a workbench, engraved armor can only be improved with Arcane Blacksmith.
- [ ] Engraved armor can't be disenchanted at an arcane enchanter.

## 3. General
- [ ] Item values look reasonable at a merchant (see ITEMS.md).
- [ ] The vanilla recipes for the base items are unchanged.
- [ ] Save, quit, reload: crafted items are still there.
