#pragma once

struct Modificator
{
	// left
	WORD One = 0;
	WORD Two = 0;
	WORD Three = 0;

	// right
	WORD Four = 0;
	WORD Five = 0;
	WORD Six = 0;
};


struct Part
{
	Modificator modificator1;
	WORD character1 = 32;
	Modificator modificator2;
	WORD character2 = 32;
	WORD speed = 0;
};


