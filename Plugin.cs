using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CreatureManager;
using HarmonyLib;
using ItemManager;
using LocationManager;
using PieceManager;
using ServerSync;
using SkillManager;
using StatusEffectManager;
using UnityEngine;
using PrefabManager = ItemManager.PrefabManager;
using Range = LocationManager.Range;

namespace ColoredTrollArmors
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class ColoredTrollArmorsPlugin : BaseUnityPlugin
    {
        internal const string ModName = "ColoredTrollArmors";
        internal const string ModVersion = "0.0.2";
        internal const string Author = "coemt";
        private const string ModGUID = Author + "." + ModName;
        private static string ConfigFileName = ModGUID + ".cfg";
        private static string ConfigFileFullPath = Paths.ConfigPath + Path.DirectorySeparatorChar + ConfigFileName;
        internal static string ConnectionError = "";
        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource ColoredTrollArmorsLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        private static readonly ConfigSync ConfigSync = new(ModGUID)
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        // Location Manager variables
        public Texture2D tex;
        private Sprite mySprite;
        private SpriteRenderer sr;

        public enum Toggle
        {
            On = 1,
            Off = 0
        }

        public void Awake()
        {
            _serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On,
                "If on, the configuration is locked and can be changed by server admins only.");
            _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);


            #region PieceManager Example Code

            
            BuildPiece.ConfigurationEnabled = false;

           
            BuildPiece piece_trollbench = new("station", "piece_trollbench", "assets");

            piece_trollbench.Name
                .English("Dye Station"); 
            piece_trollbench.Description.English("Trollhide Dye Station");
            piece_trollbench.RequiredItems.Add("FineWood", 8,
                true); 
            piece_trollbench.RequiredItems.Add("Resin", 20, false);
            piece_trollbench.RequiredItems.Add("TrophyForestTroll", 2, false);
            piece_trollbench.Category.Add(BuildPieceCategory.Crafting);
            piece_trollbench.Crafting.Set(PieceManager.CraftingTable
                .Workbench); 
            
            

          

            #endregion

            /*#region SkillManager Example Code

            Skill
                tenacity = new("Tenacity",
                    "tenacity-icon.png"); // Skill name along with the skill icon. By default the icon is found in the icons folder. Put it there if you wish to load one.

            tenacity.Description.English("Reduces damage taken by 0.2% per level.");
            tenacity.Name.German("Hartnäckigkeit"); // Use this to localize values for the name
            tenacity.Description.German(
                "Reduziert erlittenen Schaden um 0,2% pro Stufe."); // You can do the same for the description
            tenacity.Configurable = true;

            #endregion*/

            /*#region LocationManager Example Code

            _ = new LocationManager.Location("guildfabs", "GuildAltarSceneFab")
            {
                MapIcon = "portalicon.png",
                ShowMapIcon = ShowIcon.Explored,
                MapIconSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f),
                    100.0f),
                CanSpawn = true,
                SpawnArea = Heightmap.BiomeArea.Everything,
                Prioritize = true,
                PreferCenter = true,
                Rotation = Rotation.Slope,
                HeightDelta = new Range(0, 2),
                SnapToWater = false,
                ForestThreshold = new Range(0, 2.19f),
                Biome = Heightmap.Biome.Meadows,
                SpawnDistance = new Range(500, 1500),
                SpawnAltitude = new Range(10, 100),
                MinimumDistanceFromGroup = 100,
                GroupName = "groupName",
                Count = 15,
                Unique = true
            };

            #region Location Notes

            // MapIcon                      Sets the map icon for the location.
            // ShowMapIcon                  When to show the map icon of the location. Requires an icon to be set. Use "Never" to not show a map icon for the location. Use "Always" to always show a map icon for the location. Use "Explored" to start showing a map icon for the location as soon as a player has explored the area.
            // MapIconSprite                Sets the map icon for the location.
            // CanSpawn                     Can the location spawn at all.
            // SpawnArea                    If the location should spawn more towards the edge of the biome or towards the center. Use "Edge" to make it spawn towards the edge. Use "Median" to make it spawn towards the center. Use "Everything" if it doesn't matter.</para>
            // Prioritize                   If set to true, this location will be prioritized over other locations, if they would spawn in the same area.
            // PreferCenter                 If set to true, Valheim will try to spawn your location as close to the center of the map as possible.
            // Rotation                     How to rotate the location. Use "Fixed" to use the rotation of the prefab. Use "Random" to randomize the rotation. Use "Slope" to rotate the location along a possible slope.
            // HeightDelta                  The minimum and maximum height difference of the terrain below the location.
            // SnapToWater                  If the location should spawn near water.
            // ForestThreshold              If the location should spawn in a forest. Everything above 1.15 is considered a forest by Valheim. 2.19 is considered a thick forest by Valheim.
            // Biome
            // SpawnDistance                Minimum and maximum range from the center of the map for the location.
            // SpawnAltitude                Minimum and maximum altitude for the location.
            // MinimumDistanceFromGroup     Locations in the same group will keep at least this much distance between each other.
            // GroupName                    The name of the group of the location, used by the minimum distance from group setting.
            // Count                        Maximum number of locations to spawn in. Does not mean that this many locations will spawn. But Valheim will try its best to spawn this many, if there is space.
            // Unique                       If set to true, all other locations will be deleted, once the first one has been discovered by a player.

            #endregion

            #endregion*/

            /*#region StatusEffectManager Example Code

            CustomSE mycooleffect = new("Toxicity");
            mycooleffect.Name.English("Toxicity");
            mycooleffect.Type = EffectType.Consume;
            mycooleffect.IconSprite = null;
            mycooleffect.Name.German("Toxizität");
            mycooleffect.Effect.m_startMessageType = MessageHud.MessageType.TopLeft;
            mycooleffect.Effect.m_startMessage = "My Cool Status Effect Started";
            mycooleffect.Effect.m_stopMessageType = MessageHud.MessageType.TopLeft;
            mycooleffect.Effect.m_stopMessage = "Not cool anymore, ending effect.";
            mycooleffect.Effect.m_tooltip = "<color=orange>Toxic damage over time</color>";
            mycooleffect.AddSEToPrefab(mycooleffect, "SwordIron");

            CustomSE drunkeffect = new("se_drunk", "se_drunk_effect");
            drunkeffect.Name.English("Drunk"); // You can use this to fix the display name in code
            drunkeffect.Icon =
                "DrunkIcon.png"; // Use this to add an icon (64x64) for the status effect. Put your icon in an "icons" folder
            drunkeffect.Name.German("Betrunken"); // Or add translations for other languages
            drunkeffect.Effect.m_startMessageType =
                MessageHud.MessageType.Center; // Specify where the start effect message shows
            drunkeffect.Effect.m_startMessage = "I'm drunk!"; // What the start message says
            drunkeffect.Effect.m_stopMessageType =
                MessageHud.MessageType.Center; // Specify where the stop effect message shows
            drunkeffect.Effect.m_stopMessage = "Sober...again."; // What the stop message says
            drunkeffect.Effect.m_tooltip =
                "<color=red>Your vision is blurry</color>"; // Tooltip that will describe the effect applied to the player
            drunkeffect.AddSEToPrefab(drunkeffect,
                "TankardAnniversary"); // Adds the status effect to the Anniversary Tankard. Applies when equipped.

            // Create a new status effect in code and apply it to a prefab.
            CustomSE codeSE = new("CodeStatusEffect");
            codeSE.Name.English("New Effect");
            codeSE.Type = EffectType.Consume; // Set the type of status effect this should be.
            codeSE.Icon = "ModDevPower.png";
            codeSE.Name.German("Betrunken"); // Or add translations for other languages
            codeSE.Effect.m_startMessageType =
                MessageHud.MessageType.Center; // Specify where the start effect message shows
            codeSE.Effect.m_startMessage = "Mod Dev power, granted."; // What the start message says
            codeSE.Effect.m_stopMessageType =
                MessageHud.MessageType.Center; // Specify where the stop effect message shows
            codeSE.Effect.m_stopMessage = "Mod Dev power, removed."; // What the stop message says
            codeSE.Effect.m_tooltip =
                "<color=green>You now have Mod Dev POWER!</color>"; // Tooltip that will describe the effect applied to the player
            codeSE.AddSEToPrefab(codeSE,
                "SwordCheat"); // Adds the status effect to the Cheat Sword. Applies when equipped.

            #endregion*/

            #region ItemManager Example Code

            
            
            Item HelmetTrollWhite = new("colors", "HelmetTrollWhite", "assets");
            HelmetTrollWhite.Name.English("White Helmet Troll");
            HelmetTrollWhite.Description.English("Have custom mod ideas? @Coemt");
            HelmetTrollWhite.Crafting.Add("piece_trollbench",
                1); // Custom crafting stations can be specified as a string
            HelmetTrollWhite.RequiredItems.Add("TrollHide", 5);
            HelmetTrollWhite.RequiredItems.Add("WhiteDye_1", 5);
          HelmetTrollWhite.RequiredUpgradeItems.Add("WhiteDye_1", 5);
            HelmetTrollWhite.RequiredUpgradeItems.Add("TrollHide", 3); 
            HelmetTrollWhite.RequiredUpgradeItems.Add("WhiteDye_1", 3); 
            
            Item HelmetTrollPurple = new("colors", "HelmetTrollPurple", "assets");
            HelmetTrollPurple.Name.English("Purple Helmet Troll");
            HelmetTrollPurple.Description.English("Have custom mod ideas? @Coemt");
            HelmetTrollPurple.Crafting.Add("piece_trollbench",
                1);
            HelmetTrollPurple.RequiredItems.Add("TrollHide", 5);
            HelmetTrollPurple.RequiredItems.Add("PurpleDye_1", 5);
            HelmetTrollPurple.RequiredUpgradeItems.Add("BoneFragments", 5);
            HelmetTrollPurple.RequiredUpgradeItems.Add("TrollHide", 3); 
            HelmetTrollPurple.RequiredUpgradeItems.Add("PurpleDye_1", 3); 
            
            
            Item HelmetTrollRed = new("colors", "HelmetTrollRed", "assets");
            HelmetTrollRed.Name.English("Red Helmet Troll");
            HelmetTrollRed.Description.English("Have custom mod ideas? @Coemt");
            HelmetTrollRed.Crafting.Add("piece_trollbench",
                1);
            HelmetTrollRed.RequiredItems.Add("TrollHide", 5);
            HelmetTrollRed.RequiredItems.Add("RedDye_1", 5);
            HelmetTrollRed.RequiredUpgradeItems.Add("BoneFragments", 5);
            HelmetTrollRed.RequiredUpgradeItems.Add("TrollHide", 3); 
            HelmetTrollRed.RequiredUpgradeItems.Add("RedDye_1", 3); 

            
            Item HelmetTrollGreen = new("colors", "HelmetTrollGreen", "assets");
            HelmetTrollGreen.Name.English("Green Helmet Troll");
            HelmetTrollGreen.Description.English("Have custom mod ideas? @Coemt");
            HelmetTrollGreen.Crafting.Add("piece_trollbench",
                1);
            HelmetTrollGreen.RequiredItems.Add("TrollHide", 5);
            HelmetTrollGreen.RequiredItems.Add("GreenDye_1", 5);
            HelmetTrollGreen.RequiredUpgradeItems.Add("BoneFragments", 5);
            HelmetTrollGreen.RequiredUpgradeItems.Add("TrollHide", 5); 
            HelmetTrollGreen.RequiredUpgradeItems.Add("GreenDye_1", 3); 
            
            Item HelmetTrollDarkGreen = new("colors", "HelmetTrollDarkGreen", "assets");
            HelmetTrollDarkGreen.Name.English("Dark Green Helmet Troll");
            HelmetTrollDarkGreen.Description.English("Have custom mod ideas? @Coemt");
            HelmetTrollDarkGreen.Crafting.Add("piece_trollbench",
                1);
            HelmetTrollDarkGreen.RequiredItems.Add("TrollHide", 5);
            HelmetTrollDarkGreen.RequiredItems.Add("DarkGreenDye_1", 5);
            HelmetTrollDarkGreen.RequiredUpgradeItems.Add("BoneFragments", 5);
            HelmetTrollDarkGreen.RequiredUpgradeItems.Add("TrollHide", 3); 
            HelmetTrollDarkGreen.RequiredUpgradeItems.Add("DarkGreenDye_1", 3); 
            
            
            Item HelmetTrollYellow = new("colors", "HelmetTrollYellow", "assets");
            HelmetTrollYellow.Name.English("Yellow Helmet Troll");
            HelmetTrollYellow.Description.English("Have custom mod ideas? @Coemt");
            HelmetTrollYellow.Crafting.Add("piece_trollbench",
                1);
            HelmetTrollYellow.RequiredItems.Add("TrollHide", 5);
            HelmetTrollYellow.RequiredItems.Add("BoneFragments", 5);
            HelmetTrollYellow.RequiredItems.Add("YellowDye_1", 5);
            HelmetTrollYellow.RequiredUpgradeItems.Add("BoneFragments", 5);
            HelmetTrollYellow.RequiredUpgradeItems.Add("TrollHide", 3); 
            HelmetTrollYellow.RequiredUpgradeItems.Add("YellowDye_1", 3); 
            
            
            Item HelmetTrollGrey = new("colors", "HelmetTrollGrey", "assets");
            HelmetTrollGrey.Name.English("Grey Helmet Troll");
            HelmetTrollGrey.Description.English("Have custom mod ideas? @Coemt");
            HelmetTrollGrey.Crafting.Add("piece_trollbench",
                1);
            HelmetTrollGrey.RequiredItems.Add("TrollHide", 5);
            HelmetTrollGrey.RequiredItems.Add("BoneFragments", 5);
            HelmetTrollGrey.RequiredItems.Add("GreyDye_1", 5);
            HelmetTrollGrey.RequiredUpgradeItems.Add("BoneFragments", 5);
            HelmetTrollGrey.RequiredUpgradeItems.Add("TrollHide", 3); 
            HelmetTrollGrey.RequiredUpgradeItems.Add("GreyDye_1", 3); 
            
            Item CapeTrollRed = new("colors", "CapeTrollRed", "assets");
            CapeTrollRed.Name.English("Red Troll Cape");
            CapeTrollRed.Description.English("Have custom mod ideas? @Coemt");
            CapeTrollRed.Crafting.Add("piece_trollbench",
                1);
            CapeTrollRed.RequiredItems.Add("TrollHide", 10);
            CapeTrollRed.RequiredItems.Add("BoneFragments", 5);
            CapeTrollRed.RequiredItems.Add("RedDye_1", 5);
            CapeTrollRed.RequiredUpgradeItems.Add("BoneFragments", 5);
            CapeTrollRed.RequiredUpgradeItems.Add("TrollHide", 5); 
            CapeTrollRed.RequiredUpgradeItems.Add("RedDye_1", 3);
            
            Item CapeTrollPurple = new("colors", "CapeTrollPurple", "assets");
            CapeTrollPurple.Name.English("Purple Troll Cape");
            CapeTrollPurple.Description.English("Have custom mod ideas? @Coemt");
            CapeTrollPurple.Crafting.Add("piece_trollbench",
                1);
            CapeTrollPurple.RequiredItems.Add("TrollHide", 10);
            CapeTrollPurple.RequiredItems.Add("BoneFragments", 5);
            CapeTrollPurple.RequiredItems.Add("PurpleDye_1", 5);
            CapeTrollPurple.RequiredUpgradeItems.Add("BoneFragments", 5);
            CapeTrollPurple.RequiredUpgradeItems.Add("TrollHide", 5); 
            CapeTrollPurple.RequiredUpgradeItems.Add("PurpleDye_1", 3);

            
            Item CapeTrollYellow = new("colors", "CapeTrollYellow", "assets");
            CapeTrollYellow.Name.English("Yellow Troll Cape");
            CapeTrollYellow.Description.English("Have custom mod ideas? @Coemt");
            CapeTrollYellow.Crafting.Add("piece_trollbench",
                1);
            CapeTrollYellow.RequiredItems.Add("TrollHide", 10);
            CapeTrollYellow.RequiredItems.Add("BoneFragments", 5);
            CapeTrollYellow.RequiredItems.Add("YellowDye_1", 5);
            CapeTrollYellow.RequiredUpgradeItems.Add("BoneFragments", 5);
            CapeTrollYellow.RequiredUpgradeItems.Add("TrollHide", 5); 
            CapeTrollYellow.RequiredUpgradeItems.Add("YellowDye_1", 3);

            
            Item CapeTrollGrey = new("colors", "CapeTrollGrey", "assets");
            CapeTrollGrey.Name.English("Grey Troll Cape");
            CapeTrollGrey.Description.English("Have custom mod ideas? @Coemt");
            CapeTrollGrey.Crafting.Add("piece_trollbench",
                1);
            CapeTrollGrey.RequiredItems.Add("TrollHide", 10);
            CapeTrollGrey.RequiredItems.Add("BoneFragments", 5);
            CapeTrollGrey.RequiredItems.Add("GreyDye_1", 5);
            CapeTrollGrey.RequiredUpgradeItems.Add("BoneFragments", 5);
            CapeTrollGrey.RequiredUpgradeItems.Add("TrollHide", 5); 
            CapeTrollGrey.RequiredUpgradeItems.Add("GreyDye_1", 3);

            
            Item CapeTrollGreen = new("colors", "CapeTrollGreen", "assets");
            CapeTrollGreen.Name.English("Green Troll Cape");
            CapeTrollGreen.Description.English("Have custom mod ideas? @Coemt");
            CapeTrollGreen.Crafting.Add("piece_trollbench",
                1);
            CapeTrollGreen.RequiredItems.Add("TrollHide", 10);
            CapeTrollGreen.RequiredItems.Add("BoneFragments", 5);
            CapeTrollGreen.RequiredItems.Add("GreenDye_1", 5);
            CapeTrollGreen.RequiredUpgradeItems.Add("BoneFragments", 3);
            CapeTrollGreen.RequiredUpgradeItems.Add("TrollHide", 5); 
            CapeTrollGreen.RequiredUpgradeItems.Add("GreenDye_1", 3);
            
            Item CapeTrollDarkGreen = new("colors", "CapeTrollDarkGreen", "assets");
            CapeTrollDarkGreen.Name.English("Dark Green Troll Cape");
            CapeTrollDarkGreen.Description.English("Have custom mod ideas? @Coemt");
            CapeTrollDarkGreen.Crafting.Add("piece_trollbench",
                1);
            CapeTrollDarkGreen.RequiredItems.Add("TrollHide", 10);
            CapeTrollDarkGreen.RequiredItems.Add("BoneFragments", 5);
            CapeTrollDarkGreen.RequiredItems.Add("DarkGreenDye_1", 5);
            CapeTrollDarkGreen.RequiredUpgradeItems.Add("BoneFragments", 3);
            CapeTrollDarkGreen.RequiredUpgradeItems.Add("TrollHide", 5); 
            CapeTrollDarkGreen.RequiredUpgradeItems.Add("DarkGreenDye_1", 3);
            
            Item CapeTrollWhite = new("colors", "CapeTrollWhite", "assets");
            CapeTrollWhite.Name.English("White Troll Cape");
            CapeTrollWhite.Description.English("Have custom mod ideas? @Coemt");
            CapeTrollWhite.Crafting.Add("piece_trollbench",
                1);
            CapeTrollWhite.RequiredItems.Add("TrollHide", 10);
            CapeTrollWhite.RequiredItems.Add("WhiteDye_1", 5);
            CapeTrollWhite.RequiredItems.Add("BoneFragments", 5);
            CapeTrollWhite.RequiredUpgradeItems.Add("BoneFragments", 3);
            CapeTrollWhite.RequiredUpgradeItems.Add("TrollHide", 5); 
            CapeTrollWhite.RequiredUpgradeItems.Add("WhiteDye_1", 3);
            
            Item ArmorTrollLegsYellow = new("colors", "ArmorTrollLegsYellow", "assets");
            ArmorTrollLegsYellow.Name.English("Yellow Troll Legs");
            ArmorTrollLegsYellow.Description.English("Have custom mod ideas? @Coemt");
            ArmorTrollLegsYellow.Crafting.Add("piece_trollbench",
                1);
            ArmorTrollLegsYellow.RequiredItems.Add("TrollHide", 5);
            ArmorTrollLegsYellow.RequiredItems.Add("YellowDye_1", 5);
            ArmorTrollLegsYellow.RequiredUpgradeItems.Add("BoneFragments", 5);
            ArmorTrollLegsYellow.RequiredUpgradeItems.Add("TrollHide", 3); 
            ArmorTrollLegsYellow.RequiredUpgradeItems.Add("YellowDye_1", 3);
            
            
            Item ArmorTrollLegsGrey = new("colors", "ArmorTrollLegsGrey", "assets");
            ArmorTrollLegsGrey.Name.English("Grey Troll Legs");
            ArmorTrollLegsGrey.Description.English("Have custom mod ideas? @Coemt");
            ArmorTrollLegsGrey.Crafting.Add("piece_trollbench",
                1);
            ArmorTrollLegsGrey.RequiredItems.Add("TrollHide", 5);
            ArmorTrollLegsGrey.RequiredItems.Add("GreyDye_1", 5);
            ArmorTrollLegsGrey.RequiredUpgradeItems.Add("BoneFragments", 5);
            ArmorTrollLegsGrey.RequiredUpgradeItems.Add("TrollHide", 3); 
            ArmorTrollLegsGrey.RequiredUpgradeItems.Add("GreyDye_1", 3);
            
            
            Item ArmorTrollLegsGreen = new("colors", "ArmorTrollLegsGreen", "assets");
            ArmorTrollLegsGreen.Name.English("Green Troll Legs");
            ArmorTrollLegsGreen.Description.English("Have custom mod ideas? @Coemt");
            ArmorTrollLegsGreen.Crafting.Add("piece_trollbench",
                1);
            ArmorTrollLegsGreen.RequiredItems.Add("TrollHide", 5);
           ArmorTrollLegsGreen.RequiredItems.Add("GreenDye_1", 5);
            ArmorTrollLegsGreen.RequiredUpgradeItems.Add("BoneFragments", 5);
            ArmorTrollLegsGreen.RequiredUpgradeItems.Add("TrollHide", 3); 
            ArmorTrollLegsGreen.RequiredUpgradeItems.Add("GreenDye_1", 3);
            
            
            Item ArmorTrollLegsWhite = new("colors", "ArmorTrollLegsWhite", "assets");
            ArmorTrollLegsWhite.Name.English("White Troll Legs");
            ArmorTrollLegsWhite.Description.English("Have custom mod ideas? @Coemt");
            ArmorTrollLegsWhite.Crafting.Add("piece_trollbench",
                1);
            ArmorTrollLegsWhite.RequiredItems.Add("TrollHide", 5);
            ArmorTrollLegsWhite.RequiredItems.Add("WhiteDye_1", 5);
            ArmorTrollLegsWhite.RequiredUpgradeItems.Add("BoneFragments", 5);
            ArmorTrollLegsWhite.RequiredUpgradeItems.Add("TrollHide", 3); 
            ArmorTrollLegsWhite.RequiredUpgradeItems.Add("WhiteDye_1", 3);
            
            
            Item ArmorTrollLegsRed = new("colors", "ArmorTrollLegsRed", "assets");
            ArmorTrollLegsRed.Name.English("Red Troll Legs");
            ArmorTrollLegsRed.Description.English("Have custom mod ideas? @Coemt");
            ArmorTrollLegsRed.Crafting.Add("piece_trollbench",
                1);
            ArmorTrollLegsRed.RequiredItems.Add("TrollHide", 5);
            ArmorTrollLegsRed.RequiredItems.Add("RedDye_1", 5);
            ArmorTrollLegsRed.RequiredUpgradeItems.Add("BoneFragments", 5);
            ArmorTrollLegsRed.RequiredUpgradeItems.Add("TrollHide", 3); 
            ArmorTrollLegsRed.RequiredUpgradeItems.Add("RedDye_1", 3);
            
            Item ArmorTrollLegsPurple = new("colors", "ArmorTrollLegsPurple", "assets");
            ArmorTrollLegsPurple.Name.English("Purple Troll Legs");
            ArmorTrollLegsPurple.Description.English("Have custom mod ideas? @Coemt");
            ArmorTrollLegsPurple.Crafting.Add("piece_trollbench",
                1);
            ArmorTrollLegsPurple.RequiredItems.Add("TrollHide", 5);
            ArmorTrollLegsPurple.RequiredItems.Add("PurpleDye_1", 5);
            ArmorTrollLegsPurple.RequiredUpgradeItems.Add("BoneFragments", 5);
            ArmorTrollLegsPurple.RequiredUpgradeItems.Add("TrollHide", 3); 
            ArmorTrollLegsPurple.RequiredUpgradeItems.Add("PurpleDye_1", 3);
            
            Item ArmorTrollLegsDarkGreen = new("colors", "ArmorTrollLegsDarkGreen", "assets");
            ArmorTrollLegsDarkGreen.Name.English("Dark Green Troll Legs");
            ArmorTrollLegsDarkGreen.Description.English("Have custom mod ideas? @Coemt");
            ArmorTrollLegsDarkGreen.Crafting.Add("piece_trollbench",
                1);
            ArmorTrollLegsDarkGreen.RequiredItems.Add("TrollHide", 5);
            ArmorTrollLegsDarkGreen.RequiredItems.Add("DarkGreenDye_1", 5);
            ArmorTrollLegsDarkGreen.RequiredUpgradeItems.Add("BoneFragments", 5);
            ArmorTrollLegsDarkGreen.RequiredUpgradeItems.Add("TrollHide", 3); 
            ArmorTrollLegsDarkGreen.RequiredUpgradeItems.Add("DarkGreenDye_1", 3);

            
            
             Item TrollChestYellow = new("colors", "TrollChestYellow", "assets");
             TrollChestYellow.Name.English("Yellow Troll Chest");
             TrollChestYellow.Description.English("Have custom mod ideas? @Coemt");
             TrollChestYellow.Crafting.Add("piece_trollbench",
                 1);
             TrollChestYellow.RequiredItems.Add("TrollHide", 5);
            TrollChestYellow.RequiredItems.Add("YellowDye_1", 5);
             TrollChestYellow.RequiredUpgradeItems.Add("BoneFragments", 5);
             TrollChestYellow.RequiredUpgradeItems.Add("TrollHide", 3); 
             TrollChestYellow.RequiredUpgradeItems.Add("YellowDye_1", 3);
            
              
             Item TrollChestRed = new("colors", "TrollChestRed", "assets");
             TrollChestRed.Name.English("Red Troll Chest");
             TrollChestRed.Description.English("Have custom mod ideas? @Coemt");
             TrollChestRed.Crafting.Add("piece_trollbench",
                 1);
             TrollChestRed.RequiredItems.Add("TrollHide", 5);
             TrollChestRed.RequiredItems.Add("RedDye_1", 5);
             TrollChestRed.RequiredUpgradeItems.Add("BoneFragments", 5);
             TrollChestRed.RequiredUpgradeItems.Add("TrollHide", 3); 
             TrollChestRed.RequiredUpgradeItems.Add("RedDye_1", 3);
             
             Item TrollChestGreen = new("colors", "TrollChestGreen", "assets");
             TrollChestGreen.Name.English("Green Troll Chest");
             TrollChestGreen.Description.English("Have custom mod ideas? @Coemt");
             TrollChestGreen.Crafting.Add("piece_trollbench",
                 1);
             TrollChestGreen.RequiredItems.Add("TrollHide", 5);
             TrollChestGreen.RequiredItems.Add("GreenDye_1", 5);
             TrollChestGreen.RequiredUpgradeItems.Add("BoneFragments", 5);
             TrollChestGreen.RequiredUpgradeItems.Add("TrollHide", 3); 
             TrollChestGreen.RequiredUpgradeItems.Add("GreenDye_1", 3);

             Item TrollChestWhite = new("colors", "TrollChestWhite", "assets");
             TrollChestWhite.Name.English("White Troll Chest");
             TrollChestWhite.Description.English("Have custom mod ideas? @Coemt");
             TrollChestWhite.Crafting.Add("piece_trollbench",
                 1);
             TrollChestWhite.RequiredItems.Add("TrollHide", 5);
             TrollChestWhite.RequiredItems.Add("WhiteDye_1", 5);
             TrollChestWhite.RequiredUpgradeItems.Add("BoneFragments", 5);
             TrollChestWhite.RequiredUpgradeItems.Add("TrollHide", 3); 
             TrollChestWhite.RequiredUpgradeItems.Add("WhiteDye_1", 3);
             
             Item TrollChestPurple = new("colors", "TrollChestPurple", "assets");
             TrollChestPurple.Name.English("Purple Troll Chest");
             TrollChestPurple.Description.English("Have custom mod ideas? @Coemt");
             TrollChestPurple.Crafting.Add("piece_trollbench",
                 1);
             TrollChestPurple.RequiredItems.Add("TrollHide", 5);
             TrollChestPurple.RequiredItems.Add("PurpleDye_1", 5);
             TrollChestPurple.RequiredUpgradeItems.Add("BoneFragments", 5);
             TrollChestPurple.RequiredUpgradeItems.Add("TrollHide", 3); 
             TrollChestPurple.RequiredUpgradeItems.Add("PurpleDye_1", 3);
             
             Item TrollChestGrey = new("colors", "TrollChestGrey", "assets");
             TrollChestGrey.Name.English("Grey Troll Chest");
             TrollChestGrey.Description.English("Have custom mod ideas? @Coemt");
             TrollChestGrey.Crafting.Add("piece_trollbench",
                 1);
             TrollChestGrey.RequiredItems.Add("TrollHide", 5);
             TrollChestGrey.RequiredItems.Add("GreyDye_1", 5);
             TrollChestGrey.RequiredUpgradeItems.Add("BoneFragments", 5);
             TrollChestGrey.RequiredUpgradeItems.Add("TrollHide", 3); 
             TrollChestGrey.RequiredUpgradeItems.Add("GreyDye_1", 3);
             
             Item TrollChestDarkGreen = new("colors", "TrollChestDarkGreen", "assets");
             TrollChestDarkGreen.Name.English("Dark Green Troll Chest");
             TrollChestDarkGreen.Description.English("Have custom mod ideas? @Coemt");
             TrollChestDarkGreen.Crafting.Add("piece_trollbench",
                 1);
             TrollChestDarkGreen.RequiredItems.Add("TrollHide", 5);
            TrollChestDarkGreen.RequiredItems.Add("DarkGreenDye_1", 5);
             TrollChestDarkGreen.RequiredUpgradeItems.Add("BoneFragments", 5);
             TrollChestDarkGreen.RequiredUpgradeItems.Add("TrollHide", 3); 
             TrollChestDarkGreen.RequiredUpgradeItems.Add("DarkGreenDye_1", 3);
            
            
             ///DYE SYSTEM
             Item YellowDye_1 = new("station", "YellowDye_1", "assets");
             YellowDye_1.Name.English("Yellow Dye");
             YellowDye_1.Description.English("Have custom mod ideas? @Coemt");
             YellowDye_1.Crafting.Add("piece_trollbench",
                 1);
             YellowDye_1.RequiredItems.Add("Dandelion", 2);
             
             Item RedDye_1 = new("station", "RedDye_1", "assets");
             RedDye_1.Name.English("Red Dye");
             RedDye_1.Description.English("Have custom mod ideas? @Coemt");
             RedDye_1.Crafting.Add("piece_trollbench",
                 1);
             RedDye_1.RequiredItems.Add("Raspberry", 2);
             
             Item GreenDye_1 = new("station", "GreenDye_1", "assets");
             GreenDye_1.Name.English("Green Dye");
             GreenDye_1.Description.English("Have custom mod ideas? @Coemt");
             GreenDye_1.Crafting.Add("piece_trollbench",
                 1);
             GreenDye_1.RequiredItems.Add("AncientSeed", 1);
             
             Item GreyDye_1 = new("station", "GreyDye_1", "assets");
             GreyDye_1.Name.English("Grey Dye");
             GreyDye_1.Description.English("Have custom mod ideas? @Coemt");
             GreyDye_1.Crafting.Add("piece_trollbench",
                 1);
             GreyDye_1.RequiredItems.Add("Coal", 2);
             
             Item PurpleDye_1 = new("station", "PurpleDye_1", "assets");
             PurpleDye_1.Name.English("Purple Dye");
             PurpleDye_1.Description.English("Have custom mod ideas? @Coemt");
             PurpleDye_1.Crafting.Add("piece_trollbench",
                 1);
             PurpleDye_1.RequiredItems.Add("Raspberry", 2);
             PurpleDye_1.RequiredItems.Add("Blueberries", 2);
             
             Item WhiteDye_1 = new("station", "WhiteDye_1", "assets");
             WhiteDye_1.Name.English("White Dye");
             WhiteDye_1.Description.English("Have custom mod ideas? @Coemt");
             WhiteDye_1.Crafting.Add("piece_trollbench",
                 1);
             WhiteDye_1.RequiredItems.Add("BoneFragments", 2);
             
             
             Item DarkGreenDye_1 = new("station", "DarkGreenDye_1", "assets");
             DarkGreenDye_1.Name.English("Dark Green Dye");
             DarkGreenDye_1.Description.English("Have custom mod ideas? @Coemt");
             DarkGreenDye_1.Crafting.Add("piece_trollbench",
                 1);
             DarkGreenDye_1.RequiredItems.Add("ElderBark", 2);
             
             

            
            
            
            
            
            
            
            
            

            #endregion

            /*#region CreatureManager Example Code

            Creature wereBearBlack = new("werebear", "WereBearBlack")
            {
                Biome = Heightmap.Biome.Meadows,
                GroupSize = new CreatureManager.Range(1, 2),
                CheckSpawnInterval = 600,
                RequiredWeather = Weather.Rain | Weather.Fog,
                Maximum = 2
            };
            wereBearBlack.Localize()
                .English("Black Werebear")
                .German("Schwarzer Werbär")
                .French("Ours-Garou Noir");
            wereBearBlack.Drops["Wood"].Amount = new CreatureManager.Range(1, 2);
            wereBearBlack.Drops["Wood"].DropChance = 100f;

            Creature wereBearRed = new("werebear", "WereBearRed")
            {
                Biome = Heightmap.Biome.AshLands,
                GroupSize = new CreatureManager.Range(1, 1),
                CheckSpawnInterval = 900,
                AttackImmediately = true,
                RequiredGlobalKey = GlobalKey.KilledYagluth,
            };
            wereBearRed.Localize()
                .English("Red Werebear")
                .German("Roter Werbär")
                .French("Ours-Garou Rouge");
            wereBearRed.Drops["Coal"].Amount = new CreatureManager.Range(1, 2);
            wereBearRed.Drops["Coal"].DropChance = 100f;
            wereBearRed.Drops["Flametal"].Amount = new CreatureManager.Range(1, 1);
            wereBearRed.Drops["Flametal"].DropChance = 10f;

            #endregion*/


            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
            SetupWatcher();
        }

        private void OnDestroy()
        {
            Config.Save();
        }

        private void SetupWatcher()
        {
            FileSystemWatcher watcher = new(Paths.ConfigPath, ConfigFileName);
            watcher.Changed += ReadConfigValues;
            watcher.Created += ReadConfigValues;
            watcher.Renamed += ReadConfigValues;
            watcher.IncludeSubdirectories = true;
            watcher.SynchronizingObject = ThreadingHelper.SynchronizingObject;
            watcher.EnableRaisingEvents = true;
        }

        private void ReadConfigValues(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(ConfigFileFullPath)) return;
            try
            {
                ColoredTrollArmorsLogger.LogDebug("ReadConfigValues called");
                Config.Reload();
            }
            catch
            {
                ColoredTrollArmorsLogger.LogError($"There was an issue loading your {ConfigFileName}");
                ColoredTrollArmorsLogger.LogError("Please check your config entries for spelling and format!");
            }
        }


        #region ConfigOptions

        private static ConfigEntry<Toggle> _serverConfigLocked = null!;

        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }

        private ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private class ConfigurationManagerAttributes
        {
            public bool? Browsable = false;
        }

        class AcceptableShortcuts : AcceptableValueBase
        {
            public AcceptableShortcuts() : base(typeof(KeyboardShortcut))
            {
            }

            public override object Clamp(object value) => value;
            public override bool IsValid(object value) => true;

            public override string ToDescriptionString() =>
                "# Acceptable values: " + string.Join(", ", KeyboardShortcut.AllKeyCodes);
        }

        #endregion
    }
}