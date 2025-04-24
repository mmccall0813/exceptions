using GDWeave;
using macrottie.exceptions.Patches;

namespace macrottie.exceptions;

public class Mod : IMod {
    public Config Config;

    public Mod(IModInterface modInterface) {
        this.Config = modInterface.ReadConfig<Config>();
        
        modInterface.RegisterScriptMod(new NetworkPatcher());
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
