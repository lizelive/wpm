
using BrilliantSkies.Core.Timing;
using BrilliantSkies.Ftd.Constructs.Modules.Main.Power;
using BrilliantSkies.Localisation;
using BrilliantSkies.Localisation.Runtime.FileManagers.Files;
using BrilliantSkies.Ui.Tips;
using System;

namespace CultOfClang.NuclearReactor
{
    public class NuclearAllInOne : Block, ISteamProvider, IBlockWithSteamStats
    {
        public new static ILocFile _locFile = Loc.GetFile("NuclearAllInOne");
        const float MultiplyerPowerDensity = 100; // normal is 15 real can do 100mw/m^3
        const float HeatPerVolume = 40;

        public float EnergyPerSecond => MultiplyerPowerDensity * (float)this.item.SizeInfo.ArrayPositionsUsed;

        protected override void AppendToolTip(ProTip tip)
        {
            base.AppendToolTip(tip);
            tip.SetSpecial_Name(RTG._locFile.Get("SpecialName", "Radioisotope Thermoelectric Generator (RTG)", true), RTG._locFile.Get("SpecialDescription", "Nuclar Boiler generates endless steam using the power of the atom", true));
            tip.InfoOnly = true;
            tip.Add(Position.Middle, RTG._locFile.Format("Return_CreatesEnergyPer", "Creates <<{0} steam per second>>", EnergyPerSecond));
        }

        public override void StateChanged(IBlockStateChange change)
        {
            base.StateChanged(change);
            if (change.IsAvailableToConstruct)
            {
                this.MainConstruct.PowerUsageCreationAndFuelRestricted.RtgVolume += EnergyPerSecond;
                this.MainConstruct.HotObjectsRestricted.AddASimpleSourceOfBodyHeat(HeatChange);
            }
            else
            {
                if (!change.IsLostToConstructOrConstructLost)
                    return;
                this.MainConstruct.PowerUsageCreationAndFuelRestricted.RtgVolume -= EnergyPerSecond;
                this.MainConstruct.HotObjectsRestricted.RemoveASimpleSourceofBodyHeat(HeatChange);
            }
        }

        private float HeatChange => (float)(this.item.SizeInfo.ArrayPositionsUsed * HeatPerVolume);

        public SteamPartType PartType => SteamPartType.Boiler;

        public float Pressure => 1000 * Amount / Capacity;

        public float Capacity => this.item.SizeInfo.ArrayPositionsUsed * 10;

        public float Amount { get; set; }

        public ulong LastFlowFrame { get; set; }
        SteamStats IBlockWithSteamStats.Stats { get => new SteamStats(10)
        {
            
        }; set => throw new NotImplementedException(); }

       

        public override BlockTechInfo GetTechInfo()
        {
            return new BlockTechInfo()
                .AddSpec(RTG._locFile.Get("TechInfo_EnergyGeneratedPer", "Energy generated per second", true), EnergyPerSecond)
                .AddSpec("Steam", Amount)
                .AddStatement(RTG._locFile.Format("TechInfo_IrDetectionRange", "Adds {0}{1} to body temperature of the vehicle. This will have a small impact on IR detection range from all angles.",
    this.HeatChange,
     "°C/m\u00B3"
));
        }

        public override InteractionReturn Secondary()
        {
            return new InteractionReturn($"i have {Amount} steam");
        }

        public float TakeSteam(float amountToTake)
        {
            float num = Math.Min(this.Amount, amountToTake);
            this.Amount -= amountToTake;
            return num;
        }

        public override void BlockStart()
        {
            this.MainConstruct.SchedulerRestricted.RegisterForHalfPerSecond(Update);
          }

        public override void ScrapBlock()
        {
            this.MainConstruct.SchedulerRestricted.UnregisterForHalfPerSecond(Update);
        }

        const float RegenRate = 100;
        private void Update(ISectorTimeStep obj)
        {
            Amount += obj.DeltaTime * RegenRate;
        }

        /*
         base.StateChanged(change);
            if (change.InitiatedOrInitiatedInUnrepairedState_OnlyCalledOnce)
            {
                this.MainConstruct.SchedulerRestricted.RegisterForFixedUpdate(new Action<ITimeStep>(this.FixedStep));
            }
            else if (change.IsPermanentlyRemovedOrConstructDestroyed)
            {
                this.MainConstruct.SchedulerRestricted.UnregisterForFixedUpdate(new Action<ITimeStep>(this.FixedStep));
            }*/

        public void AttachStats(SteamStats change)
        {
            change.AddBlock(this);
        }
    }
}
