using Arpg.Game.Core;
#if DEBUG
using Arpg.Editor;
#endif

// Initialize settings (could also be loaded from config file)
// Settings are now static, so no need to pass them around

var window = new Arpg.Engine.Window("ARPG Game");

// Set up concrete loop instances
window.GameLoop = new GameLoop();
#if DEBUG
window.EditorLoop = new EditorLoop();
#endif

window.Run();
