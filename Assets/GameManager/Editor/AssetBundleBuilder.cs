using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEngine;

namespace Game.GameManager.Editor
{
    public static class AssetBundleBuilder
    {
        public static void Build(string directoryPath)
        {
            var bundleName = GameMetadataStorage.GetGameName(directoryPath);
            bundleName = bundleName.Replace(' ', '_').ToLowerInvariant();

            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            var output = Application.streamingAssetsPath;
            var parameters = new BundleBuildParameters(buildTarget, buildTargetGroup, output);

            var bundles = new List<AssetBundleBuild>();

            var assets = GameMetadataStorage.GetAllAssets(directoryPath)
                                            .Where(x => x != null)
                                            .Distinct()
                                            .ToArray();
            if (assets.Length > 0)
            {
                var assetBundle = new AssetBundleBuild();
                assetBundle.assetBundleName = $"{bundleName}.assets.bundle";
                assetBundle.assetNames = assets.Select(AssetDatabase.GetAssetPath).ToArray();
                assetBundle.addressableNames = assetBundle.assetNames.Select(Path.GetFileNameWithoutExtension).ToArray();
                bundles.Add(assetBundle);
            }

            var scene = GameMetadataStorage.GetScene(directoryPath);
            if (scene != null)
            {
                var sceneBundle = new AssetBundleBuild();
                sceneBundle.assetBundleName = $"{bundleName}.scene.bundle";
                sceneBundle.assetNames = new[] { AssetDatabase.GetAssetPath(scene) };
                sceneBundle.addressableNames = new[] { Path.GetFileNameWithoutExtension(scene.name) };
                bundles.Add(sceneBundle);
            }

            var buildContent = new BundleBuildContent(bundles);

            ContentPipeline.BuildAssetBundles(parameters, buildContent, out var result);

            var logFile = Path.Combine(output, "buildlogtep.json");
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
        }
    }
}