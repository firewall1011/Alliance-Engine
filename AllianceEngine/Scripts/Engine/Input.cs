using Silk.NET.Input;
using Silk.NET.Windowing;

namespace AllianceEngine
{
    public static class Input
    {
        public static IKeyboard Keyboard { get; private set; }
        public static IMouse Mouse { get; private set; }
        public static IInputContext InputContext { get; private set; }
        
        public static void Initialize(IView window)
        {
            InputContext = window.CreateInput();
            Keyboard = InputContext.Keyboards[0];
            Mouse = InputContext.Mice[0];
        }

    }
}
