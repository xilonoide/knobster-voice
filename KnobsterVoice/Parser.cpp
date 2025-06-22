#include <string>
#include <Windows.h>
#include "Parser.h"
#include "cppcodec\base32_crockford.hpp"

// v.1.1.0: " caEcsaB2$scaAcsaB$scaAcsaB"
// v.1.2.0: 33343334133341 shiftQ shiftW
// v.1.3.0: 6C04201K010G0CR08403602100100CR084036021001G0 shift A

class Parser
{
private:
	int defaultSpeed;

	Modificator getModificatorL(Modificator result, WORD modificator)
	{
		const WORD SCANCODE_LCONTROL = 0x1D;
		const WORD SCANCODE_LALT = 0x38;
		const WORD SCANCODE_LSHIFT = 0x2A;

		if (modificator == 0)
			return result;

		result.One = modificator == '1' || modificator == '4' || modificator == '5' || modificator == '6' ? SCANCODE_LCONTROL : 0;
		result.Two = modificator == '2' || modificator == '4' || modificator == '5' || modificator == '7' ? SCANCODE_LALT : 0;
		result.Three = modificator == '3' || modificator == '5' || modificator == '6' || modificator == '7' ? SCANCODE_LSHIFT : 0;

		return result;
	}

	Modificator getModificatorR(Modificator result, WORD modificator)
	{
		const WORD SCANCODE_RCONTROL = 0x11D;
		const WORD SCANCODE_RALT = 0x138;
		const WORD SCANCODE_RSHIFT = 0x36;

		if (modificator == 0)
			return result;

		result.Four = modificator == '1' || modificator == '4' || modificator == '5' || modificator == '6' ? SCANCODE_RCONTROL : 0;
		result.Five = modificator == '2' || modificator == '4' || modificator == '5' || modificator == '7' ? SCANCODE_RALT : 0;
		result.Six = modificator == '3' || modificator == '5' || modificator == '6' || modificator == '7' ? SCANCODE_RSHIFT : 0;

		return result;
	}

	std::string getClipboardText()
	{
		if (!OpenClipboard(nullptr))
			return "";

		HANDLE hData = GetClipboardData(CF_TEXT);

		CloseClipboard();

		if (hData == nullptr)
			return "";

		char* pszText = static_cast<char*>(hData);

		if (pszText == nullptr)
			return "";

		try
		{
			std::string text(pszText);

			return text;
		}
		catch (...) { return ""; }
	}

public:
	Parser(int defaultSpeed)
	{
		this->defaultSpeed = defaultSpeed;
	}

	Part* GetParts(std::string clipBoard = "")
	{
		using base32 = cppcodec::base32_crockford;

		Part* parts = new Part[3];
		std::string tokens[3];

		std::vector<uint8_t> decoded;

		try
		{
			if (clipBoard.empty())
				clipBoard = getClipboardText();

			decoded = base32::decode(clipBoard);
		}
		catch (...)
		{
			return NULL;
		}

		if (decoded.capacity() != 40)
			return NULL;

		Part pushRelease;
		pushRelease.modificator1 = getModificatorL(pushRelease.modificator1, decoded[0] | decoded[1] << 8);
		pushRelease.modificator1 = getModificatorR(pushRelease.modificator1, decoded[2] | decoded[3] << 8);
		pushRelease.character1 = decoded[4] | decoded[5] << 8;
		pushRelease.modificator2 = getModificatorL(pushRelease.modificator2, decoded[6] | decoded[7] << 8);
		pushRelease.modificator2 = getModificatorR(pushRelease.modificator2, decoded[8] | decoded[9] << 8);
		pushRelease.character2 = decoded[10] | decoded[11] << 8;
		parts[0] = pushRelease;

		Part inner;
		inner.modificator1 = getModificatorL(inner.modificator1, decoded[12] | decoded[13] << 8);
		inner.modificator1 = getModificatorR(inner.modificator1, decoded[14] | decoded[15] << 8);
		inner.character1 = decoded[16] | decoded[17] << 8;
		inner.modificator2 = getModificatorL(inner.modificator2, decoded[18] | decoded[19] << 8);
		inner.modificator2 = getModificatorR(inner.modificator2, decoded[20] | decoded[21] << 8);
		inner.character2 = decoded[22] | decoded[23] << 8;
		WORD speedInner = decoded[24] | decoded[25] << 8;
		inner.speed = speedInner != 0 ? speedInner : defaultSpeed;
		parts[1] = inner;
		
		Part outer;
		outer.modificator1 = getModificatorL(outer.modificator1, decoded[26] | decoded[27] << 8);
		outer.modificator1 = getModificatorR(outer.modificator1, decoded[28] | decoded[29] << 8);
		outer.character1 = decoded[30] | decoded[31] << 8;
		outer.modificator2 = getModificatorL(outer.modificator2, decoded[32] | decoded[33] << 8);
		outer.modificator2 = getModificatorR(outer.modificator2, decoded[34] | decoded[35] << 8);
		outer.character2 = decoded[36] | decoded[37] << 8;
		WORD speedOuter = decoded[38] | decoded[39] << 8;
		outer.speed = speedOuter != 0 ? speedOuter : defaultSpeed;
		parts[2] = outer;

		return parts;
	}
};
