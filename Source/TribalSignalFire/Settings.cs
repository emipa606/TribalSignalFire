using UnityEngine;
using Verse;

namespace TribalSignalFire
{
    // Token: 0x02000012 RID: 18
    public class Settings : ModSettings
    {
        public bool LimitContacts;

        // Token: 0x06000063 RID: 99 RVA: 0x00004630 File Offset: 0x00002830
        public void DoWindowContents(Rect canvas)
        {
            var gap = 8f;
            var listing_Standard = new Listing_Standard
            {
                ColumnWidth = canvas.width
            };
            listing_Standard.Begin(canvas);
            listing_Standard.Gap(gap);
            listing_Standard.CheckboxLabeled("Limit To Tribals", ref LimitContacts,
                "Limits the possible contacts to factions of the Tribal-type");
            listing_Standard.End();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref LimitContacts, "LimitContacts");
        }
    }
}