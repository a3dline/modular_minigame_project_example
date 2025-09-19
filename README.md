# Modular MiniGame Project Example

>[!WARNING]
> Disclaimer:
> This is a simplified example for demonstration purposes only.
> NO Unit tests, NO integration tests
> Can be buggy, incomplete, and not production-ready.

## Overview

This project demonstrates a modular approach to building a mini-game application
using Unity. 

Project is structured into using "games" as attachable modules. 
This provides the following benefits:

1. Each game is a self-contained module, making it easy to add, remove, or modify games without affecting the core application.
2. No code-references between core and game. All communication is done via .yaml configuration files. 
3. If any game is removed from the project, no code changes are required.

Demonstrated project does not use `Addressables` for the reasons of modularity. Instead of it I have 
implemented a simple `AssetBundle` loader and binding storage. 

## Project Structure

`Assets/Bootstrap.unity` - The main scene that initializes the solution.
`Assets/Launcher` - Contains the app launcher logic and exception handling.
`Assets/GameManager` - Manages game loading and switching.
`Assets/Games` - Contains individual game modules

## Used Technologies

`Playtika.ControllersTree` - HMVC framework for Unity. https://github.com/PlaytikaOSS/controllers-tree
`UserMetadataStorage` - Simple key-value storage for Unity Object metadata (yaml). https://github.com/a3dline/unity-metadata-storage
`UniTask` - For async/await support in Unity. https://github.com/Cysharp/UniTask
`UniTaskSemaphore` - Semaphore implementation for UniTask without synchronization context https://github.com/a3dline/unitask-semaphore
`VContainer` - Dependency injection framework for Unity. https://github.com/hadashiA/VContainer

## How to Build an existing project

1. Select "GameA" directory inspector<br>
![img.png](img/img.png)
2. Use `Build Asset Bundle` button for building all game assets into `StreamingAssets` directory
3. Do the same for "GameB" directory
4. Build and Run the project in a unity common way

## How to Add a New Game
1. Create a new directory<br>
![img_1.png](img/img_1.png)
2. Use `Enable Game` button to create a new game module
3. Set Game Name<br>
![img_2.png](img/img_2.png)
3. Create a game launcher
```C#
namespace MyGame
{
    public class MyGameLauncher : IGameLauncher
    {
        public UniTask LaunchAsync(CancellationToken token)
        {
            // Implement your game launch logic here
            Debug.Log("My Game is launching...");
            return UniTask.CompletedTask;
        }
    }
}
```
4. Create a manifest script with type reference to the launcher
```C#
namespace MyGame
{
    public class MyGameManifest : IGameManifest
    {
        public ITypeReference LauncherType => new GameLauncherTypeReference<MyGameLauncher>();
        public ITypeReference ScopeType { get; }
    }
}
```
5. Bind manifest to the game directory inspector<br>
![img_3.png](img/img_3.png)
6. Create a game initial scene
7. Bind the scene to the game directory inspector<br>
![img_4.png](img/img_4.png)
8. Run the game<br>
![img_5.png](img/img_5.png)

## Game Scope
Each game can have its own DI scope.
Create a scope class:
```C#
public class MyGameScope : IInstaller
{
    public void Install(IContainerBuilder builder)
    {
        builder.Register<MyGameDependency>(Lifetime.Singleton);
    }
}
```
Attach it to the manifest:
```C#
public ITypeReference ScopeType => new ScopeTypeReference<MyGameScope>();
```
Scope is available in a launcher:
```C#
public class MyGameLauncher : IGameLauncher
{
    private readonly MyGameDependency _dependency;
    public MyGameLauncher(MyGameDependency dependency)
    {
        _dependency = dependency;
    }

    public UniTask LaunchAsync(CancellationToken token)
    {
        _dependency.DoWork();
        return UniTask.CompletedTask;
    }
}
```

## Access to the game scene
Game scene is loaded additively. You can access it using `IGameSceneProvider`:
```C#
public GameASceneViewController(IControllerFactory controllerFactory,
                                IGameSceneProvider sceneProvider) 
        : base(controllerFactory)
    {
        _sceneProvider = sceneProvider;
    }

    protected override UniTask OnFlowAsync(CancellationToken cancellationToken)
    {
        var view = _sceneProvider.Scene
                                 .GetRootGameObjects()
                                 .Select(x => x.GetComponent<SceneAView>())
                                 .FirstOrDefault(x => x != null);
    }
```

## Access to the game assets
Game assets are loaded from the AssetBundle. You can access them using `IGameAssetsProvider`:
```C#
public GameBPopupLaunchController(IControllerFactory controllerFactory,
                                  IGameAssetsProvider assetsProvider)
        : base(controllerFactory)
    {
        _assetsProvider = assetsProvider;
    }

    protected override async UniTask OnFlowAsync(CancellationToken cancellationToken)
    {
        var popupPrefab = await _assetsProvider.LoadAsync<GameObject>("Popup", cancellationToken);
        _instance = Object.Instantiate(popupPrefab, Args);
    }
```

## Run controller in an app tree
I recommend running game controllers in the app tree. It gives the benefits of exception handling and debug tools.
Use `IGameControllerRunner` that available in a container:
```C#
public class GameALauncher : IGameLauncher
{
    private readonly IGameControllerRunner _controllerRunner;

    public GameALauncher(IGameControllerRunner controllerRunner)
    {
        _controllerRunner = controllerRunner;
    }

    public UniTask LaunchAsync(CancellationToken token)
    {
        return _controllerRunner.ExecuteAndWaitResultAsync<GameASceneViewController>(token);
    }
}
```

>[!TIP]
> Don't forget to build AssetBundle after any changes before running the project in a device

>[!NOTE]
> For editor testing it is not nessessary to build AssetBundle because the core 
> module has an emulation mode that loads assets directly from the project.