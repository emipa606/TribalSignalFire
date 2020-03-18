using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace TribalSignalFire
{
    public class JobDriver_UseSignalFire : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }
        
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Reserve.Reserve((TargetIndex)1, 1, -1, null);
            yield return ToilFailConditions.FailOn(Toils_Goto.GotoCell((TargetIndex)1, (PathEndMode)4), delegate (Toil to)
            {
                Building_SignalFire building_SignalFire = (Building_SignalFire)to.actor.jobs.curJob.GetTarget((TargetIndex)1).Thing;
                return !building_SignalFire.CanUseSignalFireNow;
            });
            yield return new Toil
            {
                initAction = () =>
                {
                    Pawn pawn = this.pawn;
                    Building_SignalFire building_SignalFire = (Building_SignalFire)pawn.jobs.curJob.GetTarget((TargetIndex)1).Thing;
                    bool canUseSignalFireNow = building_SignalFire.CanUseSignalFireNow;
                    if (canUseSignalFireNow)
                    {
                        pawn.jobs.curJob.commTarget.TryOpenComms(pawn);
                    }
                }
            };
        }
    }
}
