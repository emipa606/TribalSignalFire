using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace TribalSignalFire
{
    public class Building_SignalFire : Building
    {
        public bool CanUseSignalFireNow => Spawned;

        private void UseAct(Pawn myPawn, ICommunicable commTarget)
        {
            var job = new Job(DefDatabase<JobDef>.GetNamed("UseSignalFire"), this)
            {
                commTarget = commTarget
            };
            myPawn.jobs.TryTakeOrderedJob(job, 0);
            PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, (KnowledgeAmount) 6);
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
        {
            if (!myPawn.CanReach(this, (PathEndMode) 4, (Danger) 2))
            {
                var item = new FloatMenuOption("CannotUseNoPath".Translate(), null);
                return new List<FloatMenuOption>
                {
                    item
                };
            }

            // Its a smoke signal, it does not care about solar-flares
            //
            //if (Spawned && Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare))
            //{
            //    return new List<FloatMenuOption>
            //        {
            //            new FloatMenuOption(Translator.Translate("CannotUseSolarFlare"), null, (MenuOptionPriority)4, null, null, 0f, null, null)
            //        };
            //}

            if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
            {
                return new List<FloatMenuOption>
                {
                    new FloatMenuOption(
                        "CannotUseReason".Translate("IncapableOfCapacity".Translate(PawnCapacityDefOf.Sight.label)),
                        null)
                };
            }

            if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                return new List<FloatMenuOption>
                {
                    new FloatMenuOption(
                        "CannotUseReason".Translate(
                            "IncapableOfCapacity".Translate(PawnCapacityDefOf.Manipulation.label)), null)
                };
            }

            if (!CanUseSignalFireNow)
            {
                Log.Error(myPawn + " could not use signal fire for unknown reason.");
                return new List<FloatMenuOption>
                {
                    new FloatMenuOption("Cannot use now", null)
                };
            }

            var refuelable = this.TryGetComp<CompRefuelable>();

            if (refuelable == null || !refuelable.HasFuel)
            {
                return new List<FloatMenuOption>
                {
                    new FloatMenuOption("Cannot use now, need fuel", null)
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

                    if (!LeaderIsAvailableToTalk(faction))
                    {
                        string str = faction.leader != null
                            ? "LeaderUnavailable".Translate(faction.leader.LabelShort)
                            : "LeaderUnavailableNoLeader".Translate();

                        list.Add(new FloatMenuOption(text + " (" + str + ")", null));
                        continue;
                    }
                }

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
                    myPawn.jobs.TryTakeOrderedJob(job, 0);
                    PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, (KnowledgeAmount) 6);
                }

                list.Add(FloatMenuUtility.DecoratePrioritizedTask(
                    new FloatMenuOption(text, action, (MenuOptionPriority) 7), myPawn, this));
            }

            return list;
        }

        public static bool LeaderIsAvailableToTalk(Faction fac)
        {
            return fac.leader != null &&
                   (!fac.leader.Spawned || !fac.leader.Downed && !fac.leader.IsPrisoner && fac.leader.Awake() &&
                       !fac.leader.InMentalState);
        }
    }
}