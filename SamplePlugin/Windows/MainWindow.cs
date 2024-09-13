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

        private string userInput = string.Empty;


        private string[] profileTexts = new string[5];
        private int currentProfileIndex = 0;

        private float fontSize = 16.0f; // Default font size

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

            // Initialize profile texts to empty
            for (int i = 0; i < profileTexts.Length; i++)
            {
                profileTexts[i] = string.Empty;
            }
        }

        public void Dispose() { }

        public override void PreDraw()
        {
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
            // Lock Window Button
            bool isLocked = !IsMainWindowWindowMovable;
            if (ImGui.Checkbox("Lock Window", ref isLocked))
            {
                IsMainWindowWindowMovable = !isLocked;
                //Configuration.Save();
            }
            ImGui.Spacing();

            // Font Size Slider
            if (ImGui.SliderFloat("Font Size", ref fontSize, 8.0f, 32.0f, "%.0f"))
            {
                // Font size changed
                //Configuration.FontSize = fontSize;
                //Configuration.Save();
            }
            ImGui.Spacing();

            // Profile Buttons (1-5)
            ImGui.BeginGroup();
            for (int i = 0; i < 5; i++)
            {
                string buttonLabel = $"Profile {i + 1}";
                if (ImGui.Button(buttonLabel))
                {
                    profileTexts[currentProfileIndex] = userInput;

                    currentProfileIndex = i;
                    userInput = profileTexts[currentProfileIndex];
                }
            }
            ImGui.EndGroup();

            ImGui.SameLine(); // Aligning the text box beside the profile buttons

            // Text Box
            ImGui.BeginGroup();
            ImGui.Text($"Notes for Profile {currentProfileIndex + 1}:");
            var windowSize2 = ImGui.GetWindowSize();
            var textBoxHeight = windowSize2.Y - ImGui.GetCursorPosY() - ImGui.GetFrameHeightWithSpacing();
            var textBoxSize = new Vector2(windowSize2.X - 100, textBoxHeight); // Adjusted width to fit buttons

            // child window to contain the input text
            ImGui.BeginChild("ScrollingRegion", textBoxSize, true, ImGuiWindowFlags.HorizontalScrollbar);

            float wrapWidth = ImGui.GetContentRegionAvail().X;

            ImGui.PushFont(ImGui.GetFont());
            ImGui.SetWindowFontScale(fontSize / 16.0f);

            ImGui.InputTextMultiline(
                "##UserInputTextBox",
                ref userInput,
                1024,
                new Vector2(wrapWidth, textBoxHeight - 20),
                ImGuiInputTextFlags.AllowTabInput | ImGuiInputTextFlags.CtrlEnterForNewLine,
                (ImGuiInputTextCallback)null
            );

            ImGui.SetWindowFontScale(1.0f);
            ImGui.PopFont();

            ImGui.EndChild();
            ImGui.EndGroup();

            ImGui.Spacing();
        }
    }
}
