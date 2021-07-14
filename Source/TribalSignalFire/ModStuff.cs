using UnityEngine;
using Verse;

namespace TribalSignalFire
{
    public class ModStuff : Mod
    {
        public static Settings Settings;

        public ModStuff(ModContentPack content) : base(content)
        {
            Settings = GetSettings<Settings>();
        }

        public override string SettingsCategory()
        {
            return "Tribal Signal Fire";
        }

        public override void DoSettingsWindowContents(Rect canvas)
        {
            Settings.DoWindowContents(canvas);
        }
    }
}