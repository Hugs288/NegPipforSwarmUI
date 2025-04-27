using Newtonsoft.Json.Linq;
using SwarmUI.Builtin_ComfyUIBackend;
using SwarmUI.Core;
using SwarmUI.Text2Image;
using SwarmUI.Utils;

// NOTE: Namespace must NOT contain "SwarmUI" (this is reserved for built-ins)
namespace SwarmExtensions.NegPip;

// NOTE: Classname must match filename
public class NegPipforSwarmUI : Extension
{
    // OnInit is called when the extension is loaded
    public override void OnInit()
    {
        InstallableFeatures.RegisterInstallableFeature(new(
            "NegPip",
            "negpip",
            "https://github.com/pamparamm/ComfyUI-ppm",
            "pamparamm", // Author
            "Installs the ppm custom nodes, enabling negpip support.\nDo you wish to install?"
        ));
        ScriptFiles.Add("assets/NegPip.js");

        ComfyUIBackendExtension.NodeToFeatureMap["CLIPNegPip"] = "negpip";

        T2IParamGroup negPipGroup = new("NegPip", Toggles: true, Open: false, IsAdvanced: false);

        T2IRegisteredParam<bool> useNegPipParam = T2IParamTypes.Register<bool>(new(
            Name: "Use NegPip",
            Description: "Enable NegPip. Allows you to use negative weight in the positive prompt.",
            Default: "false",
            Group: T2IParamTypes.GroupSampling,
            FeatureFlag: "negpip"
        ));

        // Add the step to the ComfyUI workflow generation process
        WorkflowGenerator.AddModelGenStep(g =>
        {
            if (g.UserInput.TryGet(useNegPipParam, out bool enabled) && enabled)
            {
                Logs.Verbose("[NegPip] Applying CLIPNegPip node during model generation step.");
                string negPipNodeId = g.CreateNode("CLIPNegPip", new JObject()
                {
                    ["model"] = g.LoadingModel, // Use g.LoadingModel
                    ["clip"] = g.LoadingClip   // Use g.LoadingClip
                });
                g.LoadingModel = [negPipNodeId, 0]; // Output 0 = MODEL
                g.LoadingClip = [negPipNodeId, 1];  // Output 1 = CLIP
            }
        }, priority: -7);
    }
}
