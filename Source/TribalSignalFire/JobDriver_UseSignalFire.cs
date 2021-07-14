using System.Collections.Generic;
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
            yield return Toils_Reserve.Reserve((TargetIndex) 1);
            yield return Toils_Goto.GotoCell((TargetIndex) 1, (PathEndMode) 4).FailOn(delegate(Toil to)
            {
                var building_SignalFire = (Building_SignalFire) to.actor.jobs.curJob.GetTarget((TargetIndex) 1).Thing;
                return !building_SignalFire.CanUseSignalFireNow;
            });
            yield return new Toil
            {
                initAction = () =>
                {
                    var negotiator = pawn;
                    var building_SignalFire =
                        (Building_SignalFire) negotiator.jobs.curJob.GetTarget((TargetIndex) 1).Thing;
                    var canUseSignalFireNow = building_SignalFire.CanUseSignalFireNow;
                    if (canUseSignalFireNow)
                    {
                        negotiator.jobs.curJob.commTarget.TryOpenComms(negotiator);
                    }
                }
            };
        }
    }
}