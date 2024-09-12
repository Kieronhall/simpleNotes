using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;

namespace SamplePlugin.Windows
{
    public class MainWindow : Window, IDisposable
    {
        private string IconImagePath;
        private Plugin Plugin;

        private bool IsMainWindowWindowMovable = true;
        private Configuration Configuration;

        // Variable to hold the user's input text
        private string userInput = string.Empty;

        public MainWindow(Plugin plugin, string iconImagePath)
            : base("frimpyNotes", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(375, 330),
                MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
            };

            IconImagePath = iconImagePath;
            Plugin = plugin;
        }

        public void Dispose() { }

        public override void PreDraw()
        {
            //Checking of window is movable or not
            if (IsMainWindowWindowMovable)
            {
                Flags &= ~ImGuiWindowFlags.NoMove;
            }
            else
            {
                Flags |= ImGuiWindowFlags.NoMove;
            }
        }

        public override void Draw()
        {
            
            //Lock Window Button
            if (ImGui.Button("Lock Window"))
            {
                IsMainWindowWindowMovable = !IsMainWindowWindowMovable;
                //Configuration.Save();
            }

            ImGui.Spacing();

            var imageSize = new Vector2(50, 50);
            var iconImage = Plugin.TextureProvider.GetFromFile(IconImagePath).GetWrapOrDefault();
            if (iconImage != null)
            {
                var windowSize = ImGui.GetWindowSize();
                // putting image top right corner
                var imagePosition = new Vector2(
                    windowSize.X - imageSize.X - 10, 
                    30 
                );
                ImGui.SetCursorPos(imagePosition);
                ImGui.Image(iconImage.ImGuiHandle, imageSize);
            }
            else
            {
                ImGui.Text("Image not found.");
            }

            ImGui.Spacing();

            // Text Box
            ImGui.Text("Enter some notes:");

            var windowSize2 = ImGui.GetWindowSize();

            var textBoxHeight = windowSize2.Y - ImGui.GetCursorPosY() - ImGui.GetFrameHeightWithSpacing(); // Remaining height minus the label height and some padding
            var textBoxSize = new Vector2(windowSize2.X - 20, textBoxHeight); // Adjust width and height

            ImGui.InputTextMultiline("##UserInputTextBox", ref userInput, 1024, textBoxSize, ImGuiInputTextFlags.AllowTabInput);


            ImGui.Spacing();
        }
    }
}
