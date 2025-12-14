#pragma once
#include <string>

namespace pipe {
	extern bool cursorVisible;
	extern bool gameInitialized;
	extern bool openMenu;
	void ListenForCommands();
	void ProcessCommand(const std::string& cmd);
}