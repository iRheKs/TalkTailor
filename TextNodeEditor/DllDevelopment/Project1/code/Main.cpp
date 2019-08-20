//#include <Test.hpp>
#include <string>
#include <iostream>

using namespace std;

int main() 
{
	Dialogue newDialogue = createDialogue("../Assets/Test.xml");

	unsigned short i;
	for ( i = 0; i < 10; i++)
	{
		newDialogue = addNode(newDialogue);
		newDialogue = addLine(newDialogue, i+1, "Hola");
		newDialogue = addAnswer(newDialogue, i+1, i+2, "Que tal?");
	}

	saveDialogue(newDialogue);

	newDialogue = deleteNode(newDialogue);

	saveDialogue(newDialogue);

	Dialogue newDialogue2 = loadDialogue("../Assets/Test.xml");

	newDialogue2 = deleteNode(newDialogue2, 3);//deleteNode(newDialogue); 
	newDialogue2 = deleteLine(newDialogue2, 4);
	newDialogue2 = deleteLine(newDialogue2, 5, 1);
	newDialogue2 = deleteAnswer(newDialogue2, 4);
	newDialogue2 = deleteAnswer(newDialogue2, 5, 1);

	exportDialogue(newDialogue2, "../Assets/Test2.xml");

	system("pause");

	return 0;
}