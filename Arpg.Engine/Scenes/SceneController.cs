namespace Arpg.Engine.Scenes;


public static class ScenesController
{
  static Scene? currentScene = null;
  static Scene? previousScene = null;
  static readonly Dictionary<Type, Scene> scenes = [];
  static readonly Stack<Scene> sceneStack = [];

  public static Scene? CurrentScene => currentScene;
  public static Scene? PreviousScene => previousScene;
  public static bool IsTransitioning { get; private set; } = false;
  public static int SceneStackCount => sceneStack.Count;

  public static void Initialize()
  {
    if (currentScene != null && !currentScene.IsInitialized)
    {
      currentScene.Initialize();
    }
  }

  public static void Update(float dt)
  {
    currentScene?.Update(dt);
  }

  public static void Draw()
  {
    // Clear background with current scene's background color
    ClearBackground(currentScene?.BackgroundColor ?? Color.Black);

    // Draw all scenes in stack first (background scenes)
    foreach (var scene in sceneStack)
    {
      scene.Draw();
    }

    // Draw current scene on top
    currentScene?.Draw();
  }

  public static void AddScene(Scene scene)
  {
    scenes.Add(scene.GetType(), scene);
  }

  public static void AddScene<T>(T scene) where T : Scene
  {
    scenes[typeof(T)] = scene;
  }

  public static void SwitchTo<T>() where T : Scene, new()
  {
    if (scenes.TryGetValue(typeof(T), out var scene))
    {
      SwitchToScene(scene);
    }
    else
    {
      // Create new scene if not found
      var newScene = new T();
      scenes[typeof(T)] = newScene;
      SwitchToScene(newScene);
    }
  }


  private static void SwitchToScene(Scene scene)
  {
    IsTransitioning = true;

    // Handle previous scene cleanup
    if (currentScene != null)
    {
      previousScene = currentScene;
      previousScene.OnExit();
    }

    // Switch to new scene
    currentScene = scene;
    currentScene.OnEnter();

    // Initialize if not already initialized
    if (!currentScene.IsInitialized)
    {
      currentScene.Initialize();
    }

    IsTransitioning = false;
  }

  public static void PushScene<T>() where T : Scene
  {
    Scene? scene = GetScene<T>();
    if (scene == null)
    {
      return;
    }

    // Pause current scene and push to stack
    if (currentScene != null)
    {
      sceneStack.Push(currentScene);
      currentScene.OnPause();
    }

    // Make the new scene current
    currentScene = scene;
    currentScene.OnEnter();

    if (!currentScene.IsInitialized)
    {
      currentScene.Initialize();
    }
  }

  public static void PopScene()
  {
    if (sceneStack.Count > 0)
    {
      // Exit current scene
      currentScene?.OnExit();

      // Resume previous scene from stack
      currentScene = sceneStack.Pop();
      currentScene.OnResume();
    }
  }

  public static void PopAll()
  {
    while (sceneStack.Count > 0)
    {
      PopScene();
    }
  }

  public static T? GetScene<T>() where T : Scene
  {
    return scenes.TryGetValue(typeof(T), out var scene) ? scene as T : null;
  }

  public static bool HasScene<T>() where T : Scene
  {
    return scenes.ContainsKey(typeof(T));
  }

  public static void RemoveScene<T>() where T : Scene
  {
    if (scenes.TryGetValue(typeof(T), out var scene))
    {
      // If removing current scene, clear it
      if (currentScene == scene)
      {
        currentScene?.OnExit();
        currentScene = null;
      }

      // Remove from previous scene if it matches
      if (previousScene == scene)
      {
        previousScene = null;
      }

      // Remove from stack if present
      if (sceneStack.Contains(scene))
      {
        var tempStack = new Stack<Scene>(sceneStack.Reverse().Where(s => s != scene));
        sceneStack.Clear();
        while (tempStack.Count > 0)
        {
          sceneStack.Push(tempStack.Pop());
        }
      }

      scene.Dispose();
      scenes.Remove(typeof(T));
    }
  }

  public static void ClearAllScenes()
  {
    // Dispose all scenes
    foreach (var scene in scenes.Values)
    {
      scene.Dispose();
    }

    // Clear all collections
    scenes.Clear();
    sceneStack.Clear();
    currentScene = null;
    previousScene = null;
  }
}