using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// Uses file system watcher to track changes to specific files in the project directory.
/// </summary>
public static class FileWatcher
{

    /// <summary>
    /// Invoked when a file is created or modified; returns modified asset path.
    /// </summary>
    public static event Action<string> OnModified;

    private const string fileFilter = "*.cs"; // Change the filter to suite it for your file types.
    private static readonly ConcurrentQueue<string> modifiedPaths = new ConcurrentQueue<string>();

    [InitializeOnLoadMethod]
    private static void Initialize ()
    {
        EditorApplication.update += Update;
        var dataPath = Application.dataPath;
        Task.Run(() => StartWatcher(dataPath))
            .ContinueWith(StopWatcher, TaskScheduler.FromCurrentSynchronizationContext());
    }
    
    private static void Update ()
    {
        if (modifiedPaths.Count == 0) return;
        if (!modifiedPaths.TryDequeue(out var fullPath)) return;
        if (!File.Exists(fullPath)) return;
        
        var assetPath = AbsoluteToAssetPath(fullPath);
        AssetDatabase.ImportAsset(assetPath);
        OnModified?.Invoke(assetPath);

        // Only required in case you're using delayed calls in response to OnModified;
        // delayed calls are not invoked while editor is not in focus otherwise.
        if (!InternalEditorUtility.isApplicationActive)
            EditorApplication.delayCall?.Invoke();
    }

    private static FileSystemWatcher StartWatcher (string path)
    {
        var watcher = new FileSystemWatcher();
        watcher.Path = path;
        watcher.IncludeSubdirectories = true;
        watcher.NotifyFilter = NotifyFilters.LastWrite; 
        watcher.Filter = fileFilter;
        watcher.Changed += (_, e) => modifiedPaths.Enqueue(e.FullPath);
        watcher.EnableRaisingEvents = true;
        return watcher;
    }

    private static void StopWatcher (Task<FileSystemWatcher> startTask)
    {
        try
        {
            var watcher = startTask.Result;
            AppDomain.CurrentDomain.DomainUnload += (EventHandler)((_, __) => { watcher.Dispose(); });
        }
        catch (Exception e) { Debug.LogError($"Failed to stop file watcher: {e.Message}"); }
    }

    private static string AbsoluteToAssetPath (string absolutePath)
    {
        absolutePath = absolutePath.Replace("\\", "/");
        return "Assets" + absolutePath.Replace(Application.dataPath, string.Empty);
    }
}