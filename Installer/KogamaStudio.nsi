!include "MUI2.nsh"

Name "KogamaStudio Installer"
OutFile "KogamaStudio-Installer.exe"
InstallDir "$LOCALAPPDATA"
Icon "icon.ico"
BrandingText " "
RequestExecutionLevel user

Var selectedServer
Var hwndCustomRadio
Var hwndBrowseBtn

!insertmacro MUI_LANGUAGE "English"

Page Custom ServerSelectPage LeaveServerSelectPage
!insertmacro MUI_PAGE_INSTFILES

Function ServerSelectPage
  nsDialogs::Create 1018
  Pop $0
  
  ${NSD_CreateLabel} 0 0 100% 20u "Select KogamaLauncher Server:"
  Pop $1
  
  ${NSD_CreateRadioButton} 10u 30u 200u 15u "KogamaLauncher-BR" $2
  Pop $3
  ${NSD_CreateRadioButton} 10u 50u 200u 15u "KogamaLauncher-Friends" $4
  Pop $5
  ${NSD_CreateRadioButton} 10u 70u 200u 15u "KogamaLauncher-WWW" $6
  Pop $7
  ${NSD_Check} $7
  
  ${NSD_CreateRadioButton} 10u 90u 200u 15u "Custom folder" $8
  Pop $hwndCustomRadio
  
  ${NSD_CreateText} 10u 110u 290u 15u "$LOCALAPPDATA\KogamaLauncher-WWW\Launcher\Standalone"
  Pop $hwndBrowseBtn
  
  nsDialogs::Show
FunctionEnd

Function LeaveServerSelectPage
  ${NSD_GetState} $3 $0
  ${If} $0 <> 0
    StrCpy $selectedServer "$LOCALAPPDATA\KogamaLauncher-BR\Launcher\Standalone"
    Return
  ${EndIf}
  
  ${NSD_GetState} $5 $0
  ${If} $0 <> 0
    StrCpy $selectedServer "$LOCALAPPDATA\KogamaLauncher-Friends\Launcher\Standalone"
    Return
  ${EndIf}
  
  ${NSD_GetState} $7 $0
  ${If} $0 <> 0
    StrCpy $selectedServer "$LOCALAPPDATA\KogamaLauncher-WWW\Launcher\Standalone"
    Return
  ${EndIf}
  
  ${NSD_GetState} $hwndCustomRadio $0
  ${If} $0 <> 0
    ${NSD_GetText} $hwndBrowseBtn $selectedServer
    Return
  ${EndIf}
FunctionEnd

Section "Install"
  SetOutPath "$LOCALAPPDATA\KogamaStudio"
  File /r "files_for_appdata\KogamaStudio\*.*"
  
  CreateDirectory "$selectedServer"
  SetOutPath "$selectedServer"
  File /r "files_for_launcher\*.*"
  
  MessageBox MB_OK "KogamaStudio v0.1.1 installed successfully!$\n$\nServer path: $selectedServer"
SectionEnd