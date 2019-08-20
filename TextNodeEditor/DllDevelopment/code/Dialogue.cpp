#include "headers\Dialogue.hpp"
#include "headers\Serializer.hpp"
#include <iostream>

Dialogue::Dialogue()
{
	this->path = "";
	serializer = new Serializer(this);
	fileExists = false;
}

Dialogue::Dialogue(string path, string name, bool initialNode)
{
	if(serializer == nullptr)
		serializer = new Serializer(this);

	if (!serializer->open_file(path)) {
		serializer->create_xml(path);
	}
	
	this->path = path;
	this->name = name;
	if (initialNode) {
		nodes.push_back(new Node(1, false, "Node"));
	}
	serializer->create_dialogue(path);
	fileExists = true;
}

void Dialogue::add_node()
{
	int id = next_id_available();
	Node* newNode = new Node(id, "Node");
	if (id == nodes.size()) {
		nodes.push_back(newNode);
	}
	else {
		int index = id - 1;
		nodes.insert(nodes.begin() + index, newNode);
	}
}

void Dialogue::add_node(Node * node)
{
	int id = node->get_id();
	if (id == nodes.size()) {
		nodes.push_back(node);
	}
	else {
		int index = id - 1;
		nodes.insert(nodes.begin() + index, node);
	}
}

void Dialogue::add_line_to_node(int nodeID, string line)
{
	Node* nodeIndex = get_node_by_id(nodeID);
	nodeIndex->add_line(line);
}

void Dialogue::add_answer_to_node(int currentNodeID, int connectedNodeID, string answer)
{
	Node* nodeIndex = get_node_by_id(currentNodeID);
	nodeIndex->add_answer(answer, connectedNodeID);
}

void Dialogue::remove_last_node()
{
	Node* leak = nodes.back();
	nodes.pop_back();
	leak->~Node();
	delete leak;
}

void Dialogue::remove_node_by_id(int nodeID)
{
	Node* node = get_node_by_id(nodeID);
	if (node != nullptr) {
		int index;
		for (index = 0; index < nodes.size() && nodes[index]->get_id() != nodeID; index++);
		if (node->is_removable()) {
			nodes.erase(nodes.begin() + index);
		}
		node->~Node();
		delete node;
	}
}

void Dialogue::remove_line_from_node(int nodeID, int position)
{
	Node* nodeIndex = get_node_by_id(nodeID);
	nodeIndex->delete_line(position);
}

void Dialogue::remove_answer_from_node(int nodeID, int position)
{
	Node* nodeIndex = get_node_by_id(nodeID);
	nodeIndex->delete_answer(position);
}

Node * Dialogue::get_node_by_id(int nodeID)
{
	for (size_t i = 0; i < nodes.size(); i++)
	{
		if (nodes[i]->get_id() == nodeID)
			return nodes[i];
	}

	return nullptr;
}

int Dialogue::get_node_id(int index)
{
	return nodes[index]->get_id();
}

void Dialogue::save()
{
	serializer->clear_document();
	serializer->create_doc_declaration();
	serializer->create_dialogue(path, name);
	serializer->remove_nodes();

	for (auto node : nodes) 
	{
		serializer->create_node(node);
	}

	if (fileExists) {
		serializer->save();
	}
	else {
		serializer->create_xml(path);
	}
}

void Dialogue::export_xml(string filePath)
{
	serializer->clear_document();
	serializer->create_doc_declaration();
	serializer->create_dialogue(filePath, name);
	serializer->remove_nodes();

	for (auto node : nodes)
	{
		serializer->create_node(node);
	}

	serializer->create_xml(filePath);
}

void Dialogue::load(string filePath)
{
	path = filePath;
	serializer->load_to_dialogue(this);
}

int Dialogue::next_id_available()
{
	int size, index = size = (int)nodes.size();

	for (int i = 0; i < size && index == size; i++)
	{
		if (nodes[i]->get_id() != i + 1)
			index = i;
	}

	return index + 1;
}
