using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class BuildScript
{
    [MenuItem(itemName: "Build/WebGL")]
    public static void BuildWebGL()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/StartScene.unity", "Assets/Scenes/MobileTest.unity" };
        buildPlayerOptions.locationPathName = "Build/WebGL";
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary buildSummary = buildReport.summary;

        if(buildSummary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build Secceeded: " + buildSummary.totalSize + " bytes");
        }
        else
        {
            Debug.Log("Build Failed");
        }
    }
}