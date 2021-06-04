using System;
using BrilliantSkies.Core;
using BrilliantSkies.Core.Collections;
using BrilliantSkies.Core.Logger;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Containers;
using BrilliantSkies.Modding.Types;
using BrilliantSkies.Ui.Layouts;
using BrilliantSkies.Ui.Tips;

namespace CultOfClang.NuclearReactor
{

    /// <summary>
    /// All code files using the GamePlugin or GamePlugin_PostLoad interfaces (no need to use both)
    /// will have their OnLoad method called when they are loaded by the plugin loader.
    /// </summary>
    public class ExampleModPlugin : GamePlugin
    {


        public void OnLoad()
        {
            AdvLogger.LogInfo("Loaded Example Mod");
        }

        /// <summary>
        /// Not currently called from anywhere in FTD.
        /// </summary>
        public void OnSave()
        {
            AdvLogger.LogInfo("Saved Example Mod");
        }


        /// <summary>
        /// Not directly used in FTD.
        /// </summary>
        public string name
        {
            get { return "Nuclear Reactor"; }
        }

        /// <summary>
        /// Don't worry about this- it's not used.
        /// </summary>
        public Version version
        {
            get { return new Version(0, 0, 1); }
        }


        ///// <summary>
        ///// Used if using GamePlugin_PostLoad interface.
        ///// </summary>
        ///// <returns></returns>
        //public bool AfterAllPluginsLoaded()
        //{
        //    AdvLogger.LogInfo("Called after all other mods loaded... Example Mod");
        //    ModdingEvents.AddYourModules += HookUp;
        //    return true;
        //}

        //private void HookUp(IDictionaryOfTypedTypes<IComponentContainer> typecontainer, string directory)
        //{
        //    // Add our ModWidgets container to all mods / configurations from now on.
        //    typecontainer.Add(new ModWidgets(directory));
        //}


    }

    

    //public class ModWidgets : AbstractContainer<ModWidget>
    //{
    //    public ModWidgets(string directoryWithTrailingSlash) : base(directoryWithTrailingSlash)
    //    {
    //        CreateUiContent("Widget example", "Added by ExampleMod. Used to demonstrate how to add your own categories of object via mods");
    //    }

    //    public override IModComponent AddNew()
    //    {
    //        var newOne = new ModWidget();
    //        Components.Add(newOne);
    //        return newOne;
    //    }

    //    protected override string DirName => "MyWidgets";

    //    protected override string FileExtension => ".myWidget";
    //}

    //public class ModWidget : ModComponentAbstract
    //{
    //    public string OurUselessString { get; set; } = "Useless string";

    //    public override void GuiEditor()
    //    {
    //        base.GuiEditor();
    //       OurUselessString =  MapEditorGuiCommon.StringEditor("Useless string example", OurUselessString, new ToolTip("Just an example"));
    //    }
    //}

}
