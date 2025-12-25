#include "stdafx.h"
#include "ResourcePackLoader.h"
#include "pipe.h"
#include "MenuHelpers.h"
#include <chrono>
#include <thread>
#include <filesystem>

namespace menu {
    bool isOpen = false;
    float test = 0.0f;
    static bool noTitleBar = false;
    static HMODULE hModule = nullptr;

    void Init() {
        static bool packsLoaded = false;
        static std::vector<Pack> cachedPacks;
        static float scale = 1.0f;
        static bool typing1 = false;
        static bool typing2 = false;
        static bool typing3 = false;

        static bool initializationNotification = false;

        if (!packsLoaded)
        {
            cachedPacks = LoadPacks();
            packsLoaded = true;
        }

        auto packs = cachedPacks;

        if (!hModule) {
            hModule = GetModuleHandle("KogamaStudio.dll");
            if (hModule) {
                DebugLog("[menu] KogamaStudio.dll loaded\n");
            }
        }

        // Begin frame UI setup
        DebugLog("[menu] Rendering menu. isOpen=%d, test=%.2f\n", isOpen, test);

        ImGuiIO& io = ImGui::GetIO();

        if (!isOpen) {
            return;
        }

        if (!pipe::cursorVisible) {
            RECT rect;
            GetWindowRect(GetForegroundWindow(), &rect);
            int centerX = (rect.left + rect.right) / 2;
            int centerY = (rect.top + rect.bottom) / 2;
            SetCursorPos(centerX, centerY);
        }

        if (!pipe::openMenu) {
            return;
        }

        if (typing1 || typing2 || typing3) {
            io.WantCaptureKeyboard = true;
        }
        else {
            io.WantCaptureKeyboard = false;
        }


        // Style setup (one-time)
        static bool styled = false;
        if (!styled) {
            ImGui::StyleColorsDark();
            ImVec4* colors = ImGui::GetStyle().Colors;

            ImGui::GetStyle().ScaleAllSizes(scale);

            styled = true;
            DebugLog("[menu] Style applied.\n");
        }

        // Window flags
        ImGuiWindowFlags flags = ImGuiWindowFlags_NoResize | ImGuiWindowFlags_NoCollapse;
        ImGui::SetNextWindowSize(ImVec2(450, 600), ImGuiCond_FirstUseEver);
        ImGui::SetNextWindowPos(ImVec2(20, 20), ImGuiCond_FirstUseEver);

        if (ImGui::Begin("KogamaStudio", nullptr))
        {

            if (ImGui::BeginTabBar("Tabs"))
            {
                if (ImGui::BeginTabItem("Tools"))
                {
                    static bool NoBuildLimit = false;
                    static bool AntiAFK = false;
                    static bool SingleSidePainting = false;

                    static bool CustomGridSizeEnabled = false;
                    static float CustomGridSizeValue = 1.0f;

                    static bool CustomRotStepEnabled = false;
                    static float CustomRotStepValue = 15.0f;

                    // no build limit
                    if (ImGui::Checkbox("No Build Limit", &NoBuildLimit)) {
                        if (NoBuildLimit) SendCommand("option_no_build_limit|true");
                        else SendCommand("option_no_build_limit|false");
                    }

                    // anti afk
                    if (ImGui::Checkbox("Anti AFK", &AntiAFK)) {
                        if (AntiAFK) SendCommand("option_anti_afk|true");
                        else SendCommand("option_anti_afk|false");
                    }

                    // custom grid size
                    if (ImGui::Checkbox("Custom Grid Size", &CustomGridSizeEnabled)) {
                        if (CustomGridSizeEnabled) SendCommand("option_custom_grid_size_enabled|true");
                        else SendCommand("option_custom_grid_size_enabled|false");
                    }

                    if (CustomGridSizeEnabled) {
                        ImGui::PushItemWidth(100);
                        ImGui::InputFloat("Size", &CustomGridSizeValue);
                        ImGui::PopItemWidth();

                        typing1 = ImGui::IsItemActive();

                        if (ImGui::IsItemEdited()) {
                            SendCommand(("option_custom_grid_size|" + std::to_string(CustomGridSizeValue)).c_str());
                        }
                    }

                    // custom rot step
                    if (ImGui::Checkbox("Custom Rotation Step", &CustomRotStepEnabled)) {
                        if (CustomRotStepEnabled) SendCommand("option_custom_rot_step_enabled|true");
                        else SendCommand("option_custom_rot_step_enabled|false");
                    }

                    if (CustomRotStepEnabled) {
                        ImGui::PushItemWidth(100);
                        ImGui::InputFloat("Step", &CustomRotStepValue);
                        ImGui::PopItemWidth();

                        typing2 = ImGui::IsItemActive();

                        if (ImGui::IsItemEdited()) {
                            SendCommand(("option_custom_rot_step_size|" + std::to_string(CustomRotStepValue)).c_str());
                        }
                    }

                    // single side painting
                    // cause crashes
                    if (ImGui::Checkbox("##single_side", &SingleSidePainting)) {
                        if (SingleSidePainting) SendCommand("option_single_side_painting|true");
                        else SendCommand("option_single_side_painting|false");
                    }
                    ImGui::SameLine();
                    ImGui::TextColored(ImVec4(1.0f, 1.0f, 0.0f, 1.0f), "Single Side Painting [EXPERIMENTAL]");

                    if (ImGui::IsItemHovered())
                        ImGui::SetTooltip("Enabling this may cause crashes.");


                    ImGui::EndTabItem();
                }

                // RESOURCE PACKS
                if (ImGui::BeginTabItem("Resource Packs"))
                {
                    if (ImGui::Button("Refesh Packs")) cachedPacks = LoadPacks();
                    ImGui::SameLine();
                    if (ImGui::Button("Open Folder")) OpenFolder("ResourcePacks");
                    ImGui::SameLine();
                    if (ImGui::Button("Reset")) SendCommand("resourcepacks_reset");

                    ImGui::Spacing();
                    ImGui::Separator();
                    ImGui::Spacing();

                    auto DrawPack = [](const char* name, const char* author, const char* description, const char* cmd, ImTextureID icon)
                        {
                            ImGui::BeginChild(name, ImVec2(300, 100), true);
                            ImGui::Columns(2, nullptr, false);
                            ImGui::SetColumnWidth(0, 200);

                            ImGui::Text(name);
                            ImGui::TextDisabled(author);
                            ImGui::TextWrapped(description);

                            ImGui::Spacing();
                            ImGui::Spacing();
                            if (ImGui::Button("Load", ImVec2(-1, 0))) SendCommand(cmd);

                            ImGui::NextColumn();
                            ImGui::Image(icon, ImVec2(80, 80));

                            ImGui::Columns(1);
                            ImGui::EndChild();
                        };

                    for (auto& p : packs)
                    {
                        std::string cmd = std::string("resourcepacks_load|") + p.folder;
                        DrawPack(p.name.c_str(), p.author.c_str(), p.description.c_str(), cmd.c_str(), p.iconTexture);
                    }

                    ImGui::EndTabItem();

                }

                if (ImGui::BeginTabItem("Generate"))
                {
                    if (ImGui::Button("Open Folder")) OpenFolder("Generate\\Models");  

                    ImGui::Spacing();
                    ImGui::Separator();
                    ImGui::Spacing();

                    // save section
                    //ImGui::Text("Save Model");
                    //static char modelName[256] = "";

                    //ImGui::PushItemWidth(100);
                    //ImGui::InputText("##modelName", modelName, sizeof(modelName));
                    //ImGui::PopItemWidth();
                    //typing3 = ImGui::IsItemActive();


                    //ImGui::SameLine();
                    //if (ImGui::Button("Save")) {
                    //    std::string cmd = std::string("save_model|") + modelName;
                    //    SendCommand(cmd.c_str());
                    //}

                    //ImGui::Spacing();
                    //ImGui::Separator();
                    //ImGui::Spacing();

                    // load, process section
                    //ImGui::Text("Load Model");
                    if (pipe::generateProgress == 1.0f || pipe::generateProgress == 0.0f) {
                        // showing available models
                        char path[MAX_PATH];
                        ExpandEnvironmentStringsA("%LOCALAPPDATA%\\KogamaStudio\\Generate\\Models", path, MAX_PATH);
                        DisplayDirTree(path);
                    }
                    else
                    {
                        ImGui::Text("Progress");
                        ImGui::SameLine();

                        ImGui::ProgressBar(pipe::generateProgress, ImVec2(-100, 0));
                        ImGui::SameLine();
                        if (ImGui::Button("Cancel")) {
                            SendCommand("generate_cancel");
                            pipe::generateProgress = 0.0f;
                        }
                    }

                    ImGui::EndTabItem();
                }

                if (ImGui::BeginTabItem("About"))
                {
                    ImGui::Text("KogamaStudio v0.2.0");
                    ImGui::Spacing();
                    ImGui::Separator();
                    ImGui::Spacing();
                    ImGui::Text("Special Thanks to:");
                    ImGui::BulletText("Sh0ckFR - Universal-Dear-ImGui-Hook");
                    ImGui::BulletText("Beckowl - KogamaTools");
                    ImGui::BulletText("ocornut - Dear ImGui");
                    ImGui::Spacing();
                    ImGui::EndTabItem();
                }

                ImGui::EndTabBar();
            }
        }
        ImGui::End();
    }
}
