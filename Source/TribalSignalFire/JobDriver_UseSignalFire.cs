using System.Collections.Generic;
using Verse.AI;

namespace TribalSignalFire;

public class JobDriver_UseSignalFire : JobDriver
{
    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return true;
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        yield return Toils_Reserve.Reserve(TargetIndex.A);
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOn(delegate(Toil to)
        {
            var buildingSignalFire = (Building_SignalFire)to.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
            return !buildingSignalFire.CanUseSignalFireNow;
        });
        yield return new Toil
        {
            initAction = () =>
            {
                var negotiator = pawn;
                var buildingSignalFire =
                    (Building_SignalFire)negotiator.jobs.curJob.GetTarget(TargetIndex.A).Thing;
                var canUseSignalFireNow = buildingSignalFire.CanUseSignalFireNow;
                if (canUseSignalFireNow)
                {
                    negotiator.jobs.curJob.commTarget.TryOpenComms(negotiator);
                }
            }
        };
    }
}