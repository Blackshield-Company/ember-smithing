using Mutagen.Bethesda.Plugins;

namespace EmberSmithing.Generator;

/// <summary>
/// Vanilla Skyrim.esm records this mod links to. Only FormIDs are referenced;
/// no Bethesda data or assets are copied into the plugin or the repo.
/// FormIDs were looked up with the Mutagen.Bethesda.FormKeys.SkyrimSE package.
/// The vanilla weapons and armor themselves are described in ../vanilla-items.json.
/// </summary>
internal static class Vanilla
{
    public static readonly ModKey Skyrim = ModKey.FromFileName("Skyrim.esm");
    public static FormKey K(uint id) => new(Skyrim, id);
    public static FormKey K(string hex) => new(Skyrim, Convert.ToUInt32(hex, 16));

    // Workbench keywords
    public static readonly FormKey CraftingSmithingForge = K(0x088105);
    public static readonly FormKey CraftingSmithingSharpeningWheel = K(0x088108);
    public static readonly FormKey CraftingSmithingArmorTable = K(0x0ADB78);
    public static readonly FormKey MagicDisallowEnchanting = K(0x0C27BD);

    // Materials
    public static readonly FormKey IngotIron = K(0x05ACE4);
    public static readonly FormKey IngotSteel = K(0x05ACE5);
    public static readonly FormKey IngotOrichalcum = K(0x05AD99);
    public static readonly FormKey IngotDwarven = K(0x0DB8A2);
    public static readonly FormKey IngotMoonstone = K(0x05AD9F);
    public static readonly FormKey IngotMalachite = K(0x05ADA1);
    public static readonly FormKey IngotEbony = K(0x05AD9D);
    public static readonly FormKey IngotSilver = K(0x05ACE3);
    public static readonly FormKey Leather = K(0x0DB5D2);
    public static readonly FormKey Firewood = K(0x06F993);
    public static readonly FormKey Charcoal = K(0x033760);
    public static readonly FormKey FireSalts = K(0x03AD5E);
    public static readonly FormKey FrostSalts = K(0x03AD5F);

    // Smithing perks
    public static readonly FormKey SteelSmithing = K(0x0CB40D);
    public static readonly FormKey DwarvenSmithing = K(0x0CB40E);
    public static readonly FormKey ElvenSmithing = K(0x0CB40F);
    public static readonly FormKey OrcishSmithing = K(0x0CB410);
    public static readonly FormKey GlassSmithing = K(0x0CB411);
    public static readonly FormKey EbonySmithing = K(0x0CB412);
    public static readonly FormKey ArcaneBlacksmith = K(0x05218E);

    public sealed record Material(FormKey Ingot, FormKey? Perk);

    /// <summary>Tempering material and forging perk per vanilla material (as in the vanilla recipes).</summary>
    public static Material MaterialOf(string name) => name switch
    {
        "Iron" => new(IngotIron, null),
        "Steel" => new(IngotSteel, SteelSmithing),
        "Orcish" => new(IngotOrichalcum, OrcishSmithing),
        "Dwarven" => new(IngotDwarven, DwarvenSmithing),
        "Elven" => new(IngotMoonstone, ElvenSmithing),
        "Glass" => new(IngotMalachite, GlassSmithing),
        "Ebony" => new(IngotEbony, EbonySmithing),
        "Hide" => new(Leather, null),
        "Leather" => new(Leather, null),
        _ => throw new ArgumentOutOfRangeException(nameof(name), name),
    };
}
