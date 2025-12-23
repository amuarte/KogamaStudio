#pragma once
#include <string>

namespace menu
{
	void OpenFolder(std::string subpath);
	void SendCommand(const char* cmd);
	void DisplayDirTree(const std::string& path, const std::string& rel = "");
}
