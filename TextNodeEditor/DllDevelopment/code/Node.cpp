#include "headers\Node.hpp"

Node::Node(int id, string name)
{
	this->id = id;
	removable = true;
	if (strcmp(name.c_str(), "Node") == 0)
		this->name = name + "_" + to_string(id);
	else
		this->name = name;
}

Node::Node(int id, string firstLine, string name)
{
	this->id = id;
	add_line(firstLine);
	removable = true;
	if (strcmp(name.c_str(), "Node") == 0)
	this->name = name + "_" + to_string(id);
	else
		this->name = name;
}

Node::Node(int id, string firstAnswerText, int nodeConnectedTo, string name)
{
	this->id = id;
	add_answer(firstAnswerText, nodeConnectedTo);
	removable = true;
	if (strcmp(name.c_str(), "Node") == 0)
	this->name = name + "_" + to_string(id);
	else
		this->name = name;
}

Node::Node(int id, string firstLine, string firstAnswerText, int nodeConnectedTo, string name)
{
	this->id = id;
	add_line(firstLine);
	add_answer(firstAnswerText, nodeConnectedTo);
	removable = true;
	if (strcmp(name.c_str(), "Node") == 0)
	this->name = name + "_" + to_string(id);
	else
		this->name = name;
}

Node::Node(int id, bool isRemovable, string name)
{
	this->id = id;
	removable = isRemovable;
	if (strcmp(name.c_str(), "Node") == 0)
	this->name = name + "_" + to_string(id);
	else
		this->name = name;
}

void Node::delete_line(int index)
{
	if (index == -1)
		lines.pop_back();
	else
	{
		lines.erase(lines.begin() + index-1);
	}
}

void Node::delete_answer(int index)
{
	if (index == -1)
		answers.pop_back();
	else
	{
		answers.erase(answers.begin() + index-1);
	}
}