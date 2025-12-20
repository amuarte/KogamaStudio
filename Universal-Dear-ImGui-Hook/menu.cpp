#include "stdafx.h"
#include "ResourcePackLoader.h"
#include "pipe.h"
#include <chrono>
#include <thread>

namespace menu {
    bool isOpen = false;
    float test = 0.0f;
    static bool noTitleBar = false;
    static HMODULE hModule = nullptr;

    void SendCommand(const char* cmd)
    {
        HANDLE hPipe = CreateFileA(
            "\\\\.\\pipe\\KogamaStudio",
            GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL
        );
        if (hPipe != INVALID_HANDLE_VALUE) {
            DWORD written;
            WriteFile(hPipe, cmd, strlen(cmd), &written, NULL);
            CloseHandle(hPipe);
            DebugLog("[menu] Command sent: %s\n", cmd);
        }
        else {
            DebugLog("[menu] Pipe not connected\n");
        }
    }

    void OpenResourcePacksFolder()
    {
        char* buffer = nullptr;
        size_t len;
        _dupenv_s(&buffer, &len, "APPDATA");
        std::string path = std::string(buffer) + "\\..\\Local\\KogamaStudio\\ResourcePacks";
        free(buffer);

        std::string cmd = "explorer \"" + path + "\"";
        system(cmd.c_str());
    }

    void Init() {
        static bool packsLoaded = false;
        static std::vector<Pack> cachedPacks;
        static float scale = 1.0f;

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

        if (!pipe::openMenu) return;

        // Style setup (one-time)
        static bool styled = false;
        if (!styled) {
            ImGui::StyleColorsClassic();
            ImVec4* colors = ImGui::GetStyle().Colors;
            // Custom color palette
            /*
            colors[ImGuiCol_WindowBg] = ImVec4(0.192f, 0.231f, 0.271f, 1.0f); // like kogama
            colors[ImGuiCol_Header] = ImVec4(0.035f, 0.31f, 0.4f, 0.8f); // like kogama maybe
            colors[ImGuiCol_HeaderHovered] = ImVec4(0.035f, 0.31f, 0.4f, 1.0f); // like kogama

            //button
            colors[ImGuiCol_Button] = ImVec4(0.157f, 0.192f, 0.227f, 1.0f); // like kogama
            colors[ImGuiCol_ButtonHovered] = ImVec4(0.357f, 0.392f, 0.427f, 1.0f); // like kogama maybe
            colors[ImGuiCol_ButtonActive] = ImVec4(0.557f, 0.592f, 0.627f, 1.0f); // like kogama

            //checkbox
            colors[ImGuiCol_CheckMark] = ImVec4(1.0f, 1.0f, 1.0f, 1.0f);
            colors[ImGuiCol_FrameBg] = ImVec4(0.157f, 0.192f, 0.227f, 1.0f);
            colors[ImGuiCol_FrameBgHovered] = ImVec4(0.357f, 0.392f, 0.427f, 1.0f);

            //begin
            colors[ImGuiCol_TitleBg] = ImVec4(0.035f, 0.31f, 0.4f, 1.0f);
            colors[ImGuiCol_TitleBgActive] = ImVec4(0.035f, 0.31f, 0.4f, 1.0f);
            colors[ImGuiCol_TitleBgCollapsed] = ImVec4(0.035f, 0.31f, 0.4f, 0.5f);

            //tabbar
            colors[ImGuiCol_Tab] = ImVec4(0.035f, 0.31f, 0.4f, 1.0f);
            colors[ImGuiCol_TabHovered] = ImVec4(0.235f, 0.51f, 0.6f, 1.0f);
            colors[ImGuiCol_TabActive] = ImVec4(0.135f, 0.41f, 0.5f, 1.0f);
            */

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
                    static bool SingleSidePainting = false;

                    if (ImGui::Checkbox("No Build Limit", &NoBuildLimit)) {
                        if (NoBuildLimit) SendCommand("option_no_build_limit|true");
                        else SendCommand("option_no_build_limit|false");
                    }

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

                if (ImGui::BeginTabItem("Resource Packs"))
                {
                    if (ImGui::Button("Refesh Packs")) cachedPacks = LoadPacks();
                    ImGui::SameLine();
                    if (ImGui::Button("Open Folder")) OpenResourcePacksFolder();
                    ImGui::SameLine();
                    if (ImGui::Button("Reset")) SendCommand("resourcepacks_reset");

                    ImGui::Spacing();

                    auto DrawPack = [](const char* name, const char* author, const char* description, const char* cmd, ImTextureID icon)
                        {
                            ImGui::BeginChild(name, ImVec2(300*scale, 100*scale), true);
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

                if (ImGui::BeginTabItem("Generating"))
                {
                    if (ImGui::Button("Place cube", ImVec2(-1, 0))) SendCommand("generating_model");

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
