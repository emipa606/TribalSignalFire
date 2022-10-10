using UnityEngine;
using Verse;

namespace TribalSignalFire;

public class Settings : ModSettings
{
    public bool LimitContacts;

    public void DoWindowContents(Rect canvas)
    {
        var gap = 8f;
        var listing_Standard = new Listing_Standard
        {
            ColumnWidth = canvas.width
        };
        listing_Standard.Begin(canvas);
        listing_Standard.Gap(gap);
        listing_Standard.CheckboxLabeled("TSF.LimitTribal".Translate(), ref LimitContacts,
            "TSF.LimitTribal.Tooltip".Translate());
        if (ModStuff.currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("TSF.CurrentModVersion".Translate(ModStuff.currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref LimitContacts, "LimitContacts");
    }
}