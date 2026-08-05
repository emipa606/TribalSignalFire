using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace TribalSignalFire;

public class Building_SignalFire : Building
{
    public bool CanUseSignalFireNow => Spawned;

    public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
    {
        if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some))
        {
            var item = new FloatMenuOption("CannotUseNoPath".Translate(), null);
            return new List<FloatMenuOption>
            {
                item
            };
        }

        if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
        {
            return new List<FloatMenuOption>
            {
                new(
                    "CannotUseReason".Translate("IncapableOfCapacity".Translate(PawnCapacityDefOf.Sight.label)),
                    null)
            };
        }

        if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
        {
            return new List<FloatMenuOption>
            {
                new(
                    "CannotUseReason".Translate(
                        "IncapableOfCapacity".Translate(PawnCapacityDefOf.Manipulation.label)), null)
            };
        }

        if (!CanUseSignalFireNow)
        {
            Log.Error($"{myPawn} could not use signal fire for unknown reason.");
            return new List<FloatMenuOption>
            {
                new("TSF.CantUse".Translate(), null)
            };
        }

        var refuelable = this.TryGetComp<CompRefuelable>();

        if (refuelable is not { HasFuel: true })
        {
            return new List<FloatMenuOption>
            {
                new("TSF.NeedFuel".Translate(), null)
            };
        }

        var list = new List<FloatMenuOption>();
        foreach (ICommunicable commTarget in Find.FactionManager.AllFactionsVisibleInViewOrder)
        {
            var localCommTarget = commTarget;
            var text = "CallOnRadio".Translate(localCommTarget.GetCallLabel());

            if (localCommTarget is Faction faction)
            {
                if (faction.IsPlayer)
                {
                    continue;
                }

                if (ModStuff.Settings.LimitContacts && faction.def.categoryTag != "Tribal")
                {
                    continue;
                }

                if (!leaderIsAvailableToTalk(faction))
                {
                    string str = faction.leader != null
                        ? "LeaderUnavailable".Translate(faction.leader.LabelShort)
                        : "LeaderUnavailableNoLeader".Translate();

                    list.Add(new FloatMenuOption(text + " (" + str + ")", null));
                    continue;
                }
            }

            list.Add(FloatMenuUtility.DecoratePrioritizedTask(
                new FloatMenuOption(text, action, MenuOptionPriority.InitiateSocial), myPawn, this));
            continue;

            void action()
            {
                if (commTarget is TradeShip)
                {
                    return;
                }

                var job = new Job(DefDatabase<JobDef>.GetNamed("UseSignalFire"), this)
                {
                    commTarget = localCommTarget
                };
                myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, (KnowledgeAmount)6);
            }
        }

        return list;
    }

    private static bool leaderIsAvailableToTalk(Faction fac)
    {
        return fac.leader != null &&
               (!fac.leader.Spawned || !fac.leader.Downed && !fac.leader.IsPrisoner && fac.leader.Awake() &&
                   !fac.leader.InMentalState);
    }
}