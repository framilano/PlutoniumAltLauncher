using System;
using System.Threading.Tasks;
using SDL3;

namespace PlutoniumAltLauncher;

public class Gamepad
{
    //Event sent to UI thread
    public event EventHandler<string>? GamepadButtonPressed;

    public bool ShouldBeKilled = false;

    public void StopGamepadHandling()
    {
        ShouldBeKilled = true;
    }


    public void StartGamepadHandling()
    {
        ShouldBeKilled = false;
        Task.Run(async () =>
        {
            SDL.Init(SDL.InitFlags.Gamepad);
            IntPtr gamepad = 0;
            
            //Up, down, left, right, south, east
            bool[] prevBtns = [false, false, false, false, false, false];
            bool[] curBtns = [false, false, false, false, false, false];
            
            while (true)
            {
                if (ShouldBeKilled) return; //Completely stops polling if should be killed is set
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
                curBtns[5] = SDL.GetGamepadButton(gamepad, SDL.GamepadButton.East);

                for (var i = 0; i < prevBtns.Length; i++)
                {
                    if (!prevBtns[i] && curBtns[i])
                    {
                        GamepadButtonPressed?.Invoke(null, i.ToString());
                        //Console.WriteLine("Pressed " + i);
                    }
                    prevBtns[i] = curBtns[i];
                }
                
                await Task.Delay(10);
            }
        });
    }

    
    private async Task<IntPtr> PollOpenGamepad()
    {
        while (true)
        {
            SDL.UpdateGamepads();
            // Background work here
            var gamepads = SDL.GetGamepads(out var count);
            if (gamepads is null || gamepads.Length == 0)
            {
                //Console.WriteLine("No gamepads found");
                await Task.Delay(5000);
                continue;
            }
            
            var gamepad = SDL.OpenGamepad(gamepads[0]);
            //Console.WriteLine($"Gamepad {SDL.GetGamepadName(gamepad)} is now connected ");
            return gamepad;
        }
    }
}