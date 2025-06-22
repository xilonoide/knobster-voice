#include <vector>
#include <Windows.h>
#include "Parser.h"

class SendKeys
{
private:
	INPUT createScanCodeEvent(WORD KeyValue, WORD ScanCode, bool isDown)
	{
		INPUT input = {};
		input.type = INPUT_KEYBOARD;
		input.ki.wVk = KeyValue;
		input.ki.wScan = KeyValue == 0 && ScanCode != 0 ? ScanCode : MapVirtualKey(KeyValue, MAPVK_VK_TO_VSC);
		input.ki.time = 0;
		input.ki.dwExtraInfo = 0;
		input.ki.dwFlags = (isDown ? 0 : KEYEVENTF_KEYUP) | KEYEVENTF_SCANCODE;

		if (KeyValue == 33 || KeyValue == 34 || KeyValue == 35 || KeyValue == 36 || KeyValue == 45 || KeyValue == 46 // insert, delete, home, etc.
			|| ScanCode == 285 || ScanCode == 312) // Rcontrol, Ralt, Rshift
			input.ki.dwFlags = (isDown ? 0 : KEYEVENTF_KEYUP) | KEYEVENTF_SCANCODE | KEYEVENTF_EXTENDEDKEY;

		return input;
	}

	void Press(Modificator modificator, WORD KeyValue)
	{
		std::vector<INPUT> keystrokeDown;

		if (modificator.One != 0)
			keystrokeDown.push_back(createScanCodeEvent(0, modificator.One, true));

		if (modificator.Two != 0)
			keystrokeDown.push_back(createScanCodeEvent(0, modificator.Two, true));

		if (modificator.Three != 0)
			keystrokeDown.push_back(createScanCodeEvent(0, modificator.Three, true));

		if (modificator.Four != 0)
			keystrokeDown.push_back(createScanCodeEvent(0, modificator.Four, true));

		if (modificator.Five != 0)
			keystrokeDown.push_back(createScanCodeEvent(0, modificator.Five, true));

		if (modificator.Six != 0)
			keystrokeDown.push_back(createScanCodeEvent(0, modificator.Six, true));

		if (KeyValue != 32)
			keystrokeDown.push_back(createScanCodeEvent(KeyValue, 0, true));

		SendInput(keystrokeDown.size(), keystrokeDown.data(), sizeof(keystrokeDown[0]));
	}

	void Release(Modificator modificator, WORD KeyValue)
	{
		std::vector<INPUT> keystrokeUp;

		if (modificator.One != 0)
			keystrokeUp.push_back(createScanCodeEvent(0, modificator.One, false));

		if (modificator.Two != 0)
			keystrokeUp.push_back(createScanCodeEvent(0, modificator.Two, false));

		if (modificator.Three != 0)
			keystrokeUp.push_back(createScanCodeEvent(0, modificator.Three, false));

		if (modificator.Four != 0)
			keystrokeUp.push_back(createScanCodeEvent(0, modificator.Four, false));

		if (modificator.Five != 0)
			keystrokeUp.push_back(createScanCodeEvent(0, modificator.Five, false));

		if (modificator.Six != 0)
			keystrokeUp.push_back(createScanCodeEvent(0, modificator.Six, false));

		if (KeyValue != 32)
			keystrokeUp.push_back(createScanCodeEvent(KeyValue, 0, false));

		SendInput(keystrokeUp.size(), keystrokeUp.data(), sizeof(keystrokeUp[0]));
	}

public:
	SendKeys(Modificator modificator, WORD KeyValue, int speed, bool press, bool release)
	{
		/*if (KeyValue == '\0')
			return;*/

		if (speed == 0)
		{
			if (press)
				Press(modificator, KeyValue);
			
			if (release)
				Release(modificator, KeyValue);
		}
		else
		{ 
			if (press)
			{
				for (int ix = 0; ix < speed; ix++)
				{
					Press(modificator, KeyValue);

					Sleep(5);

					Release(modificator, KeyValue);
				}
			}
			
			if (release)
			{
				Sleep(5);

				Release(modificator, KeyValue);
			}
		}
	}
};



