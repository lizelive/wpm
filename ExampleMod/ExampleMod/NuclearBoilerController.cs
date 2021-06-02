using BrilliantSkies.Blocks.SteamEngines.Ui;
using BrilliantSkies.Common.StatusChecking;
using BrilliantSkies.Core.Serialisation.Parameters.Prototypes;
using BrilliantSkies.Localisation;
using BrilliantSkies.Localisation.Runtime.FileManagers.Files;
using BrilliantSkies.Ui.Displayer;
using BrilliantSkies.Ui.Tips;
using System;
using UnityEngine;

namespace CultOfClang.NuclearReactor
{
    // Decompiled with JetBrains decompiler
    // Type: SteamBoilerController
    // Assembly: Ftd, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
    // MVID: 944AC3E7-A072-4A73-8D00-1C47CEA89795
    // Assembly location: G:\SteamLibrary\steamapps\common\From The Depths\From_The_Depths_Data\StreamingAssets\Mods\ExampleMod\ExampleMod\ExampleMod\ReferenceDlls\Ftd.dll



    public class NuclearBoilerController :
      SteamBoilerController,
      IGoverningBlock<SteamBoilerNode>,
      IGoverningBlock,
      IBlock,
      IAlive,
      IFlagState,
      IBlockWithNode<SteamBoilerNode>,
      IBlockWithSteamStats
    {
        public new static ILocFile _locFile = Loc.GetFile("Steam_Boiler_Controller");
        public bool WasLoaded;

        public SteamStorageData StorageData { get; set; } = new SteamStorageData(58U);

        public SteamBoilerControllerData Data { get; set; } = new SteamBoilerControllerData(51U);

        public SteamSize Size { get; protected set; }

        public SteamStats Stats { get; set; }

        public SteamBoilerNode Node { get; set; }

        public INode NodeInterface => (INode)this.Node;

        public SteamPartType PartType => SteamPartType.BoilerController;

        public override void ItemSet() => this.Size = SteamSize.GetByItemParameter(this.item.Code.Variables.GetInt("Size"));

        public override void BlockStart()
        {
            base.BlockStart();
            this.Data.BurnRate.SetChangeAction(new Action<float, float, ChangeFunctionCallType>(this.UpdateBurnStats));
        }

        public void GenerateHeat(float materialsBurned)
        {
        }

        private void UpdateBurnStats(float newValue, float oldValue, ChangeFunctionCallType changeType) => this.Node?.UpdateBurnStats();

        public override void StateChanged(IBlockStateChange change)
        {
            base.StateChanged(change);
            if (change.IsAvailableToConstruct)
            {
                this.MainConstruct.NodeSetsRestricted.DictionaryOfAllSets.Get<SteamBoilerNodeSet>().AddSender(this);
                this.MainConstruct.iBlockTypeStorage.SteamBoilerControllerStore.Add(this);
            }
            else
            {
                if (!change.IsLostToConstructOrConstructLost)
                    return;
                this.MainConstruct.NodeSetsRestricted.DictionaryOfAllSets.Get<SteamBoilerNodeSet>().RemoveSender(this);
                this.MainConstruct.iBlockTypeStorage.SteamBoilerControllerStore.Remove(this);
            }
        }

        public override void CheckStatus(IStatusUpdate updater)
        {
            base.CheckStatus(updater);
            if (this.Node == null || this.Node.Connections.Count != 0)
                return;
            updater.FlagError((IFlagState)this, NuclearBoilerController._locFile.Get("Error_NoSteamPipe", "Boiling chamber has no steam pipe connected on the end", true));
        }

        public override BlockTechInfo GetTechInfo()
        {
            BlockTechInfo techInfo = base.GetTechInfo();
            techInfo.AddSpec(NuclearBoilerController._locFile.Get("TechInfo_SteamPerMaterial", "Steam/material burned", true), (object)SteamConstants.SteamPerMaterial);
            if (this.Size == SteamSize.L)
                techInfo.AddSpec(NuclearBoilerController._locFile.Get("TechInfo_SteamCapacity", "Steam capacity", true), (object)this.Size.BoilerCapacity).AddSpec(NuclearBoilerController._locFile.Get("TechInfo_MaxMaterialsBurned", "Max materials burned/s", true), (object)this.Size.BoilerMatPerSec);
            return techInfo;
        }

        //public override void Secondary(Transform T) => new SteamBoilerUi(this).ActivateGui(GuiActivateType.Stack);

        protected override void AppendToolTip(ProTip tip)
        {
            base.AppendToolTip(tip);
            tip.SetSpecial_Name(this.item.DisplayName.ToString() + SteamSharedUiHelper.GetSteamSystemIdString(this.Stats), ((object)this.item.Description).ToString());
            SteamSharedUiHelper.AppendBoilerData(tip, this.Node);
            SteamSharedUiHelper.AppendQToBoilerSettingsPrompt(tip);
        }
    }

}
