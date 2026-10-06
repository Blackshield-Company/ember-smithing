using Modkit.Mutagen;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Skyrim;
using Noggog;
using static EmberSmithing.Generator.Vanilla;

namespace EmberSmithing.Generator;

internal sealed class Builder
{
    private readonly SkyrimMod _mod;
    private readonly FormIdRegistry _ids;
    private readonly ItemData _data;
    private static readonly SkyrimRelease R = SkyrimRelease.SkyrimSE;

    /// <summary>Script properties deliberately left unfilled ("EditorID:Script.Property"), for the validator.</summary>
    public HashSet<string> ExpectedUnfilled { get; } = new();

    public Builder(SkyrimMod mod, FormIdRegistry ids, ItemData data)
    {
        _mod = mod;
        _ids = ids;
        _data = data;
    }

    private FormKey Id(string editorId) => _ids.Get(editorId);
    private static FormLink<T> L<T>(FormKey k) where T : class, IMajorRecordGetter => new(k);
    private static FormLinkNullable<T> N<T>(string? hex) where T : class, IMajorRecordGetter
        => hex == null ? new FormLinkNullable<T>() : new FormLinkNullable<T>(K(hex));

    /// <summary>Heat-tempering tiers: name prefix, EditorID key, damage bonus (fraction, minimum), value multiplier, Smithing needed.</summary>
    public sealed record HeatTier(string Prefix, string Key, double Bonus, int MinBonus, double ValueMult, int Smithing);

    public static readonly HeatTier[] Tiers =
    {
        new("Heat-Tempered", "HeatTempered", 0.10, 1, 1.3, 20),
        new("Ember-Forged", "EmberForged", 0.20, 2, 1.7, 45),
        new("Dragonfire", "Dragonfire", 0.30, 3, 2.2, 70),
    };

    /// <summary>Engravings: name prefix, EditorID key, effect name, actor value, magnitude, description, recipe extra.</summary>
    public sealed record Engraving(string Prefix, string Key, string EffectName, ActorValue Av, float Magnitude, string Description, FormKey Extra, int ExtraCount);

    public static readonly Engraving[] Engravings =
    {
        new("Ember-Engraved", "Ember", "Ember Engraving", ActorValue.ResistFire, 20, "Increases fire resistance by <mag>%.", FireSalts, 1),
        new("Hearth-Engraved", "Hearth", "Hearth Engraving", ActorValue.ResistFrost, 20, "Increases frost resistance by <mag>%.", FrostSalts, 1),
        new("Bellows-Engraved", "Bellows", "Bellows Engraving", ActorValue.StaminaRateMult, 25, "Stamina regenerates <mag>% faster.", Charcoal, 2),
    };

    public const int EngraveSmithing = 30;

    public static int TierDamage(int baseDamage, HeatTier t)
        => baseDamage + Math.Max(t.MinBonus, (int)Math.Round(baseDamage * t.Bonus, MidpointRounding.AwayFromZero));

    private static string Compact(string name) => name.Replace(" ", "").Replace("-", "");

    // ----------------------------------------------------------------------------------------
    public void Build()
    {
        // --- Heat-tempered weapons ---------------------------------------------------------------
        foreach (var w in _data.Weapons)
        {
            var mat = MaterialOf(w.Material);
            FormKey previous = K(w.FormId);
            for (int t = 0; t < Tiers.Length; t++)
            {
                var tier = Tiers[t];
                var edid = $"EMB_{tier.Key}{Compact(w.Name)}";
                var weapon = Add(MakeWeapon(edid, $"{tier.Prefix} {w.Name}", w, TierDamage(w.Damage, tier), (uint)Math.Round(w.Value * tier.ValueMult, MidpointRounding.AwayFromZero)));

                var items = new List<(FormKey, int)> { (previous, 1) };
                if (t == 0) { items.Add((mat.Ingot, 1)); items.Add((Firewood, 2)); }
                else if (t == 1) { items.Add((mat.Ingot, 1)); items.Add((Charcoal, 2)); }
                else { items.Add((mat.Ingot, 2)); items.Add((FireSalts, 1)); }
                var conds = new List<Condition> { Carries(previous), SmithingAtLeast(tier.Smithing) };
                if (mat.Perk is { } perk) conds.Add(HasPerk(perk));
                Recipe($"EMB_Recipe{tier.Key}{Compact(w.Name)}", weapon.FormKey, CraftingSmithingForge, items, conds);
                Recipe($"EMB_Temper{tier.Key}{Compact(w.Name)}", weapon.FormKey, CraftingSmithingSharpeningWheel,
                    new() { (mat.Ingot, 1) }, TemperConditions());
                previous = weapon.FormKey;
            }
        }

        // --- Engravings ----------------------------------------------------------------------------
        var enchantments = Engravings.Select(e =>
        {
            var mgef = Add(new MagicEffect(Id($"EMB_{e.Key}EngravingEffect"), R)
            {
                EditorID = $"EMB_{e.Key}EngravingEffect",
                Name = e.EffectName,
                Flags = MagicEffect.Flag.Recover | MagicEffect.Flag.NoArea | MagicEffect.Flag.NoDuration | MagicEffect.Flag.NoHitEvent,
                MagicSkill = ActorValue.None, ResistValue = ActorValue.None, SecondActorValue = ActorValue.None,
                Archetype = new MagicEffectArchetype { Type = MagicEffectArchetype.TypeEnum.ValueModifier, ActorValue = e.Av },
                CastType = CastType.ConstantEffect,
                TargetType = TargetType.Self,
                CastingSoundLevel = SoundLevel.Normal,
                Description = e.Description,
            });
            var ench = new ObjectEffect(Id($"EMB_{e.Key}Engraving"), R)
            {
                EditorID = $"EMB_{e.Key}Engraving",
                Name = e.EffectName,
                EnchantmentCost = 0,
                Flags = ObjectEffect.Flag.NoAutoCalc,
                CastType = CastType.ConstantEffect,
                EnchantmentAmount = 0,
                TargetType = TargetType.Self,
                EnchantType = ObjectEffect.EnchantTypeEnum.Enchantment,
                ChargeTime = 0,
            };
            ench.Effects.Add(new Effect
            {
                BaseEffect = new FormLinkNullable<IMagicEffectGetter>(mgef.FormKey),
                Data = new EffectData { Magnitude = e.Magnitude, Area = 0, Duration = 0 },
            });
            return Add(ench).FormKey;
        }).ToArray();

        foreach (var a in _data.Armors)
        {
            var mat = MaterialOf(a.Material);
            for (int e = 0; e < Engravings.Length; e++)
            {
                var eng = Engravings[e];
                var edid = $"EMB_{eng.Key}Engraved{Compact(a.Name)}";
                var armor = Add(MakeArmor(edid, $"{eng.Prefix} {a.Name}", a, enchantments[e], (uint)Math.Round(a.Value * 1.5, MidpointRounding.AwayFromZero) + 100));
                var conds = new List<Condition> { Carries(K(a.FormId)), SmithingAtLeast(EngraveSmithing) };
                if (mat.Perk is { } perk) conds.Add(HasPerk(perk));
                Recipe($"EMB_Recipe{eng.Key}Engraved{Compact(a.Name)}", armor.FormKey, CraftingSmithingForge,
                    new() { (K(a.FormId), 1), (IngotSilver, 1), (eng.Extra, eng.ExtraCount) }, conds);
                Recipe($"EMB_Temper{eng.Key}Engraved{Compact(a.Name)}", armor.FormKey,
                    CraftingSmithingArmorTable,
                    new() { (mat.Ingot, 1) }, TemperConditions());
            }
        }
    }

    // ----------------------------------------------------------------------------------------
    // Record builders
    // ----------------------------------------------------------------------------------------

    private Weapon MakeWeapon(string edid, string name, WeaponDef w, int damage, uint value)
    {
        var weapon = new Weapon(Id(edid), R)
        {
            EditorID = edid,
            ObjectBounds = Bounds(w.Bounds),
            Name = name,
            Model = new Model { File = w.Model },
            EquipmentType = N<IEquipTypeGetter>(w.EquipType),
            BlockBashImpact = N<IImpactDataSetGetter>(w.BlockBashImpact),
            AlternateBlockMaterial = N<IMaterialTypeGetter>(w.AltBlockMaterial),
            Description = "",
            ImpactDataSet = N<IImpactDataSetGetter>(w.ImpactDataSet),
            FirstPersonModel = N<IStaticGetter>(w.FirstPersonModel),
            AttackFailSound = N<ISoundDescriptorGetter>(w.AttackFailSound),
            EquipSound = N<ISoundDescriptorGetter>(w.EquipSound),
            UnequipSound = N<ISoundDescriptorGetter>(w.UnequipSound),
            BasicStats = new WeaponBasicStats { Value = value, Weight = w.Weight, Damage = (ushort)damage },
            Data = new WeaponData
            {
                AnimationType = Enum.Parse<WeaponAnimationType>(w.AnimationType),
                Speed = w.Speed,
                Reach = w.Reach,
                NumProjectiles = w.NumProjectiles,
                AnimationAttackMult = w.AnimationAttackMult,
                Unknown2 = w.Unknown2,
                RumbleLeftMotorStrength = w.RumbleLeft,
                RumbleRightMotorStrength = w.RumbleRight,
                RumbleDuration = w.RumbleDuration,
                Skill = Enum.Parse<Skill>(w.Skill),
                Stagger = w.Stagger,
            },
            Critical = new CriticalData
            {
                Damage = (ushort)(damage / 2),
                PercentMult = w.CritPercentMult,
                Flags = w.CritOnDeath ? CriticalData.Flag.OnDeath : 0,
            },
            DetectionSoundLevel = Enum.Parse<SoundLevel>(w.DetectionSoundLevel),
        };
        weapon.Keywords = w.Keywords.Select(k => (IFormLinkGetter<IKeywordGetter>)L<IKeywordGetter>(K(k))).ToExtendedList();
        return weapon;
    }

    private Armor MakeArmor(string edid, string name, ArmorDef a, FormKey enchantment, uint value)
    {
        var armor = new Armor(Id(edid), R)
        {
            EditorID = edid,
            ObjectBounds = Bounds(a.Bounds),
            Name = name,
            WorldModel = new GenderedItem<ArmorModel?>(
                new ArmorModel { Model = new Model { File = a.Model } },
                a.FemaleModel == null ? null : new ArmorModel { Model = new Model { File = a.FemaleModel } }),
            BodyTemplate = new BodyTemplate
            {
                FirstPersonFlags = a.FirstPersonFlags.Select(Enum.Parse<BipedObjectFlag>).Aggregate((BipedObjectFlag)0, (x, y) => x | y),
                ArmorType = Enum.Parse<ArmorType>(a.ArmorType),
            },
            Race = N<IRaceGetter>(a.Race),
            Description = "",
            Value = value,
            Weight = a.Weight,
            ArmorRating = a.Rating,
            EquipmentType = N<IEquipTypeGetter>(a.EquipType),
            BashImpactDataSet = N<IImpactDataSetGetter>(a.BashImpact),
            AlternateBlockMaterial = N<IMaterialTypeGetter>(a.AltBlockMaterial),
            ObjectEffect = new FormLinkNullable<IObjectEffectGetter>(enchantment),
            TemplateArmor = new FormLinkNullable<IArmorGetter>(K(a.FormId)),
        };
        if (a.Shield) armor.MajorFlags |= Armor.MajorFlag.Shield;
        armor.Keywords = a.Keywords.Append(MagicDisallowEnchanting.ID.ToString("X6"))
            .Select(k => (IFormLinkGetter<IKeywordGetter>)L<IKeywordGetter>(K(k))).ToExtendedList();
        armor.Armature.AddRange(a.Armature.Select(x => (IFormLinkGetter<IArmorAddonGetter>)L<IArmorAddonGetter>(K(x))));
        return armor;
    }

    private void Recipe(string edid, FormKey created, FormKey workbench, List<(FormKey item, int count)> items, List<Condition> conditions)
    {
        var cobj = new ConstructibleObject(Id(edid), R)
        {
            EditorID = edid,
            CreatedObject = new FormLinkNullable<IConstructibleGetter>(created),
            WorkbenchKeyword = new FormLinkNullable<IKeywordGetter>(workbench),
            CreatedObjectCount = 1,
            Items = items.Select(i => new ContainerEntry { Item = new ContainerItem { Item = L<IItemGetter>(i.item), Count = i.count } }).ToExtendedList(),
        };
        cobj.Conditions.AddRange(conditions);
        _mod.ConstructibleObjects.Add(cobj);
    }

    /// <summary>Only list the recipe while the player carries the item it starts from (keeps the forge menu short).</summary>
    private static Condition Carries(FormKey item)
    {
        var d = new GetItemCountConditionData();
        d.ItemOrList.Link.SetTo(item);
        return Cond.Make(d, 1, CompareOperator.GreaterThanOrEqualTo);
    }

    private static Condition SmithingAtLeast(int value)
    {
        var d = new GetBaseActorValueConditionData { ActorValue = ActorValue.Smithing };
        return Cond.Make(d, value, CompareOperator.GreaterThanOrEqualTo);
    }

    private static Condition HasPerk(FormKey perk, bool or = false)
    {
        var d = new HasPerkConditionData();
        d.Perk.Link.SetTo(perk);
        return Cond.Make(d, 1, CompareOperator.EqualTo, or);
    }

    /// <summary>Same rule as the vanilla tempering recipes: unenchanted, or the Arcane Blacksmith perk.</summary>
    private static List<Condition> TemperConditions()
        => new() { Cond.Make(new EPTemperingItemIsEnchantedConditionData(), 0, CompareOperator.EqualTo, or: true), HasPerk(ArcaneBlacksmith) };

    private T Add<T>(T record) where T : SkyrimMajorRecord
    {
        _mod.GetTopLevelGroup<T>().Add(record);
        return record;
    }

    private static ObjectBounds Bounds(short[]? b)
    {
        b ??= new short[] { -28, -28, 0, 28, 28, 18 };
        return new() { First = new P3Int16(b[0], b[1], b[2]), Second = new P3Int16(b[3], b[4], b[5]) };
    }
}
