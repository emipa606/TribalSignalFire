using UnityEngine;
using Verse;

namespace TribalSignalFire;

public class Settings : ModSettings
{
    public bool LimitContacts;

    public void DoWindowContents(Rect canvas)
    {
        const float gap = 8f;
        var listingStandard = new Listing_Standard
        {
            ColumnWidth = canvas.width
        };
        listingStandard.Begin(canvas);
        listingStandard.Gap(gap);
        listingStandard.CheckboxLabeled("TSF.LimitTribal".Translate(), ref LimitContacts,
            "TSF.LimitTribal.Tooltip".Translate());
        if (ModStuff.CurrentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("TSF.CurrentModVersion".Translate(ModStuff.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref LimitContacts, "LimitContacts");
    }
}