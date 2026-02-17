using System;
using System.Threading.Tasks;
using SDL3;

namespace PlutoniumAltLauncher;

public class Gamepad
{
    //Defines if pool for button inputs is active or not
    public bool IsActive { get; set; }
    
    //Event sent to UI thread
    public event EventHandler<string>? GamepadButtonPressed;
    
    private async Task<IntPtr> PollOpenGamepad()
    {
        while (true)
        {
            SDL.UpdateGamepads();
            // Background work here
            var gamepads = SDL.GetGamepads(out var count);
            if (gamepads is null || gamepads.Length == 0)
            {
                Console.WriteLine("No gamepads found");
                await Task.Delay(5000);
                continue;
            }
            
            var gamepad = SDL.OpenGamepad(gamepads[0]);
            Console.WriteLine($"Gamepad {SDL.GetGamepadName(gamepad)} is now connected ");
            return gamepad;
        }
    }
    
    public void StartGamepadHandling()
    {
        Task.Run(async () =>
        {
            SDL.Init(SDL.InitFlags.Gamepad);
            IntPtr gamepad = 0;
            
            //Up, down, left, right, south
            bool[] prevBtns = [false, false, false, false, false];
            bool[] curBtns = [false, false, false, false, false];
            
            while (true)
            {
                if (!IsActive)
                {
                    await Task.Delay(1000);
                    continue;
                }
                
                SDL.UpdateGamepads();
                if (!SDL.GamepadConnected(gamepad))
                {
                    if (!string.IsNullOrEmpty(SDL.GetGamepadName(gamepad))) Console.WriteLine($"Gamepad {SDL.GetGamepadName(gamepad)} is now disconnected");
                    gamepad = await PollOpenGamepad();
                }
                
                curBtns[0] = SDL.GetGamepadButton(gamepad, SDL.GamepadButton.DPadUp);
                curBtns[1] = SDL.GetGamepadButton(gamepad, SDL.GamepadButton.DPadDown);
                curBtns[2] = SDL.GetGamepadButton(gamepad, SDL.GamepadButton.DPadLeft);
                curBtns[3] = SDL.GetGamepadButton(gamepad, SDL.GamepadButton.DPadRight);
                
                curBtns[4] = SDL.GetGamepadButton(gamepad, SDL.GamepadButton.South);

                for (var i = 0; i < prevBtns.Length; i++)
                {
                    if (!prevBtns[i] && curBtns[i])
                    {
                        GamepadButtonPressed?.Invoke(null, i.ToString());
                        Console.WriteLine("Pressed " + i);
                    }
                    prevBtns[i] = curBtns[i];
                }
                
                await Task.Delay(10);
            }

        });
    }
}