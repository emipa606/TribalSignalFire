﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace TribalSignalFire
{
    public class Building_SignalFire : Building
    {
        public bool CanUseSignalFireNow => Spawned;

        private void UseAct(Pawn myPawn, ICommunicable commTarget)
        {
            Job job = new Job(DefDatabase<JobDef>.GetNamed("UseSignalFire", true), this)
            {
                commTarget = commTarget
            };
            myPawn.jobs.TryTakeOrderedJob(job, 0);
            PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, (KnowledgeAmount)6);
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
        {
            if (!ReachabilityUtility.CanReach(myPawn, this, (PathEndMode)4, (Danger)2, false, 0))
            {
                FloatMenuOption item = new FloatMenuOption(Translator.Translate("CannotUseNoPath"), null, (MenuOptionPriority)4, null, null, 0f, null, null);
                return new List<FloatMenuOption>
                {
                    item
                };
            }

            if (Spawned && Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.SolarFlare))
            {
                return new List<FloatMenuOption>
                    {
                        new FloatMenuOption(Translator.Translate("CannotUseSolarFlare"), null, (MenuOptionPriority)4, null, null, 0f, null, null)
                    };
            }

            if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
            {
                return new List<FloatMenuOption>
                        {
                            new FloatMenuOption(Translator.Translate("CannotUseReason", new object[]
                            {
                                Translator.Translate("IncapableOfCapacity", new object[]
                                {
                                    PawnCapacityDefOf.Talking.label
                                })
                            }), null, (Verse.MenuOptionPriority)4, null, null, 0f, null, null)
                        };
            }

            if (!this.CanUseSignalFireNow)
            {
                Log.Error(myPawn + " could not use comm console for unknown reason.");
                return new List<FloatMenuOption>
                            {
                                new FloatMenuOption("Cannot use now", null, (MenuOptionPriority)4, null, null, 0f, null, null)
                            };
            }

            List<FloatMenuOption> list = new List<FloatMenuOption>();
            foreach (ICommunicable commTarget in Find.FactionManager.AllFactionsInViewOrder)
            {
                ICommunicable localCommTarget = commTarget;
                string text = Translator.Translate("CallOnRadio", new object[]
                {
                                    localCommTarget.GetCallLabel()
                });

                if(localCommTarget is Faction faction)
                {
                    if (faction.IsPlayer)
                    {
                        continue;
                    }
                    if (!LeaderIsAvailableToTalk(faction))
                    {
                        string str;
                        if (faction.leader != null)
                        {
                            str = Translator.Translate("LeaderUnavailable", new object[]
                            {
                                                faction.leader.LabelShort
                            });
                        }
                        else
                        {
                            str = Translator.Translate("LeaderUnavailableNoLeader");
                        }
                        list.Add(new FloatMenuOption(text + " (" + str + ")", null, (MenuOptionPriority)4, null, null, 0f, null, null));
                        continue;
                    }
                }
                void action()
                {
                    if (commTarget is TradeShip && !Building_OrbitalTradeBeacon.AllPowered(Map).Any())
                    {
                        Messages.Message(Translator.Translate("MessageNeedBeaconToTradeWithShip"), this, MessageTypeDefOf.RejectInput, false);
                    }
                    else
                    {
                        Job job = new Job(DefDatabase<JobDef>.GetNamed("UseSignalFire", true), this)
                        {
                            commTarget = localCommTarget
                        };
                        myPawn.jobs.TryTakeOrderedJob(job, 0);
                        PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, (KnowledgeAmount)6);
                    }
                }
                list.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action, (MenuOptionPriority)7, null, null, 0f, null, null), myPawn, this, "ReservedBy"));
            }
            return list;
        }

        public static bool LeaderIsAvailableToTalk(Faction fac)
            => fac.leader != null && 
            (!fac.leader.Spawned || (!fac.leader.Downed && !fac.leader.IsPrisoner && RestUtility.Awake(fac.leader) && !fac.leader.InMentalState));
    }
}
