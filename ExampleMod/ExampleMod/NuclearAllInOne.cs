
using BrilliantSkies.Blocks.SteamEngines.Ui;
using BrilliantSkies.Core.Timing;
using BrilliantSkies.Ftd.Constructs.Modules.Main.Power;
using BrilliantSkies.Ftd.DamageLogging;
using BrilliantSkies.Localisation;
using BrilliantSkies.Localisation.Runtime.FileManagers.Files;
using BrilliantSkies.Ui.Tips;
using System;

namespace CultOfClang.NuclearReactor
{
    public class NuclearAllInOne : SteamTank
    {
        public new static ILocFile _locFile = Loc.GetFile("NuclearAllInOne");
        const float MultiplyerPowerDensity = 100; // normal is 15 real can do 100mw/m^3
        const float HeatPerVolume = 40;

        public float RtgVolume => MultiplyerPowerDensity * (float)this.item.SizeInfo.ArrayPositionsUsed;
        public float SteamPerSecond => 1000 * (float)this.item.SizeInfo.ArrayPositionsUsed;

        protected override void AppendToolTip(ProTip tip)
        {
            base.AppendToolTip(tip);
            tip.SetSpecial_Name(RTG._locFile.Get("SpecialName", "Simple Reactor", true), RTG._locFile.Get("SpecialDescription", "Nuclar Boiler generates endless steam using the power of the atom", true));
            tip.InfoOnly = true;
            tip.Add(Position.Middle, RTG._locFile.Format("Return_CreatesEnergyPer", "Creates <<{0} steam per second>>", SteamPerSecond));
            if (this.StorageModule != null)
                SteamSharedUiHelper.AppendPressureAndCapacityBar(tip, this.StorageModule);
        }

        public override void StateChanged(IBlockStateChange change)
        {
            base.StateChanged(change);
            if (change.IsAvailableToConstruct)
            {
                this.MainConstruct.PowerUsageCreationAndFuelRestricted.RtgVolume += RtgVolume;
                this.MainConstruct.HotObjectsRestricted.AddASimpleSourceOfBodyHeat(HeatChange);
                this.MainConstruct.SchedulerRestricted.RegisterFor1PerSecond(Update);

            }
            else
            {
                if (!change.IsLostToConstructOrConstructLost)
                    return;
                this.MainConstruct.PowerUsageCreationAndFuelRestricted.RtgVolume -= RtgVolume;
                this.MainConstruct.HotObjectsRestricted.RemoveASimpleSourceofBodyHeat(HeatChange);
                this.MainConstruct.SchedulerRestricted.UnregisterFor1PerSecond(Update);

            }
        }

        private float HeatChange => (float)(this.item.SizeInfo.ArrayPositionsUsed * HeatPerVolume);
       

        public override BlockTechInfo GetTechInfo()
        {
            return new BlockTechInfo()
                .AddSpec(RTG._locFile.Get("TechInfo_EnergyGeneratedPer", "Energy generated per second", true), RtgVolume)
                .AddSpec("Steam", this.StorageModule?.Amount)
                .AddStatement(RTG._locFile.Format("TechInfo_IrDetectionRange", "Adds {0}{1} to body temperature of the vehicle. This will have a small impact on IR detection range from all angles.",
    this.HeatChange,
     "°C/m\u00B3"
));
        }

        private void Update(ISectorTimeStep obj)
        {
            var steam = obj.DeltaTime * SteamPerSecond;
            this.StorageModule.AddSteam(steam);
            this.Stats.BoilerSteamCreated.Add(steam);
            if(StorageModule.Pressure > 9)
                this.ExplodeBlock(new BaseDamageLogger());
        }
    }
}
