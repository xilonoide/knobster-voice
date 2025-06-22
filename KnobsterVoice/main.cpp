#include <stdio.h>
#include "libknobster.h"
#include "Parser.cpp"
#include "SendKeys.cpp"

void readEvents(Parser parser)
{
	struct Knobster* knobsters[1];
	int nr_knobster = libknobster_scan(knobsters, 1);

	if (nr_knobster == 0)
		throw std::exception("\n\nKnobster not found\n");

	struct Knobster* knobster = knobsters[0];

	if (libknobster_connect(knobster) != 0)
		throw std::exception("\n\nFailed to connect to Knobster\n");
	printf("- KnobsterVoice v.1.3.5 -\n\n");

	printf("Knobster found!\n\n\n\n");

	//if (GetKeyState(VK_NUMLOCK)) // if is ON I will turn off
	//{
	//	printf("Shutting down [NumLock]\n\n");

	//	keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | 0, 0);
	//	keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
	//}

	printf("IMPORTANT: Don't close this window, I'll be working for you\n\n\n\n");
	printf("PRO-TIP: Start this application with -minimized parameter to not see this window again. Sample:\n\n");
	printf("C:\\KnobsterVoice.exe -minimized\n\n");

	enum KnobsterEvent event;
	bool isPushing = false;
	bool error = false;

	Part* parts = parser.GetParts("6003000000R00C000003001G00000C006000000100R00C000003001G00000080");

	while (knobster != NULL)
	{
			parts = parser.GetParts();

		if (parts == NULL)
			continue;

		Modificator modificator = parts[0].modificator1;
		char character = parts[0].character1;
		int speed = parts[0].speed;

		if (isPushing && (modificator.One != 0 || modificator.Two != 0 || modificator.Three != 0 || character != 32))
			SendKeys(modificator, character, speed, true, false);

		while ((knobster != NULL) && ((event = libknobster_poll(knobster)) != KNOBSTER_EVENT_NO_EVENT))
		{
			speed = 0;
			modificator.One = 0;
			modificator.Two = 0;
			modificator.Three = 0;
			character = 32;

			switch (event)
			{
			case KNOBSTER_EVENT_ERROR_NO_RESPONSE:
			case KNOBSTER_EVENT_ERROR_TRANSFER:
				printf("\n\n************* CONNECTION LOST ************* PLEASE, RESTART KNOBSTERVOICE *************  \n");
				libknobster_close(knobster);
				error = true;
				continue;

			case KNOBSTER_EVENT_BUTTON_PRESSED:
				isPushing = true;
				break;

			case KNOBSTER_EVENT_BUTTON_RELEASED:
				modificator = parts[0].modificator1;
				character = parts[0].character1;
				speed = parts[0].speed;
				SendKeys(modificator, character, speed, false, true);
				isPushing = false;

				modificator = parts[0].modificator2;
				character = parts[0].character2;
				speed = parts[0].speed;
				break;

			case KNOBSTER_EVENT_DIAL_MINOR_CCW:
				modificator = parts[1].modificator1;
				character = parts[1].character1;
				speed = parts[1].speed;
				break;

			case KNOBSTER_EVENT_DIAL_MINOR_CW:
				modificator = parts[1].modificator2;
				character = parts[1].character2;
				speed = parts[1].speed;
				break;

			case KNOBSTER_EVENT_DIAL_MAJOR_CCW:
				modificator = parts[2].modificator1;
				character = parts[2].character1;
				speed = parts[2].speed;
				break;

			case KNOBSTER_EVENT_DIAL_MAJOR_CW:
				modificator = parts[2].modificator2;
				character = parts[2].character2;
				speed = parts[2].speed;
				break;
			}

			if ((modificator.One != 0 || modificator.Two != 0 || modificator.Three != 0 || character != 32))
				SendKeys(modificator, character, speed, true, true);
		}

		if (error)
			continue;

		Sleep(5);
	}
}

void main(int argc, const char* argv[]) {
	try
	{
		if (argc > 1 && strcmp(argv[1], "-minimized") == 0)
			ShowWindow(GetConsoleWindow(), SW_MINIMIZE);

		readEvents(Parser(1));
	}
	catch (const std::exception& ex)
	{
		printf(ex.what());
		printf("\n");
		system("pause");
	}
	catch (...)
	{
		printf("\n\n*************  CRITICAL ERROR ************* \n\n");
		printf("\n");
		system("pause");
	}
}
