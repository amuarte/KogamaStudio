Name "Test"
OutFile "test.exe"
InstallDir "$LOCALAPPDATA\KogamaLauncher-WWW\Launcher\Standalone"

Section
  SetOutPath "$INSTDIR"
  File "plik.txt"
  MessageBox MB_OK "Done!"
SectionEnd