#include "headers\Serializer.hpp"
#include <iostream>
#include <fstream>
#include <sstream>

Serializer::Serializer(Dialogue * dialogue)
{
	this->dialogue = dialogue;
	path = dialogue->path;
}

void Serializer::create_dialogue(string path, string name)
{
	this->path = path;
	char* auxString = doc.allocate_string(name.c_str());
	root = doc.allocate_node(node_element, "Dialogue");
	root->append_attribute(doc.allocate_attribute("Name", auxString));
}

void Serializer::create_node(Node * node)
{
	char* auxString;
	xml_node<>* newNode = doc.allocate_node(node_element, "Node");
	root->append_node(newNode);
	auxString = doc.allocate_string(std::to_string(node->get_id()).c_str());
	xml_attribute<> *attr = doc.allocate_attribute("ID", auxString);
	newNode->append_attribute(attr);
	auxString = doc.allocate_string(node->get_name().c_str());
	attr = doc.allocate_attribute("Name", auxString);
	newNode->append_attribute(attr);

	xml_node<>* lines = doc.allocate_node(node_element, "Lines"," ");
	newNode->append_node(lines);
	xml_node<>* answers = doc.allocate_node(node_element, "Answers", " ");
	newNode->append_node(answers);
	//node lines, dentro nodes por cada line
	//lo mismo para las answers
	int i;
	int numberOfLines = node->get_line_number();
	xml_node<>* line;
	for (i = 0; i < numberOfLines; i++)
	{
		auxString = doc.allocate_string(node->get_line_at(i).c_str());
		line = doc.allocate_node(node_element, "Line", auxString);
		lines->append_node(line);
	}

	numberOfLines = node->get_answer_number();
	pair<string, int> aux;
	for (i = 0; i < numberOfLines; i++)
	{
		aux = node->get_answer_at(i);
		auxString = doc.allocate_string(aux.first.c_str());
		line = doc.allocate_node(node_element, "Answer", auxString);
		auxString = doc.allocate_string(std::to_string(aux.second).c_str());
		line->append_attribute(doc.allocate_attribute("Connection", auxString));
		answers->append_node(line);
	}
}

void Serializer::remove_nodes()
{
	root->remove_all_nodes();
}

bool Serializer::open_file(string path)
{
	std::ifstream file(path);
	
	if (!file.is_open())
		return false;
	else 
	{
		std::stringstream buffer;
		buffer << file.rdbuf();
		std::string content(buffer.str());
		doc.parse<0>(&content[0]);
		if (strcmp(content.c_str(), "") == 0)
			create_doc_declaration();
		file.close();
		return true;
	}
}

void Serializer::save()
{
	if (check_path(path))
	{
		doc.append_node(root);
		std::ofstream file_stored(path, std::ofstream::trunc);
		file_stored << doc;
		file_stored.close();
	}
	else
	{
		//mensaje error
	}
}

void Serializer::create_xml(string path)
{
	if (check_path(path)) 
	{
		doc.append_node(root);
		std::ofstream file_to_create(path, std::fstream::trunc);
		file_to_create << doc;
		file_to_create.close();
	}
	else 
	{
		//mensaje error
	}
}

void Serializer::update_dialogue_name(string name)
{
	root->remove_first_attribute();
	char* auxString = doc.allocate_string(name.c_str());
	root = doc.allocate_node(node_element, "Dialogue");
	root->append_attribute(doc.allocate_attribute("Name", auxString));
}

void Serializer::load_to_dialogue(Dialogue * dialogue)
{
	Node* newNode;
	int ID;
	xml_node<>* linesNode;

	std::ifstream file(dialogue->path);
	std::stringstream buffer;
	buffer << file.rdbuf();
	std::string content(buffer.str());
	doc.parse<0>(&content[0]);

	if (file.is_open()) {
		xml_node<>* root = doc.first_node();
		dialogue->set_name(root->first_attribute()->value());

		for (xml_node<>* node = root->first_node(); node; node = node->next_sibling())
		{
			ID = atoi(node->first_attribute()->value());
			string name = node->last_attribute()->value();
			newNode = new Node(ID, name);
			linesNode = node->first_node();
			for (xml_node<>* line = linesNode->first_node(); line; line = line->next_sibling())
			{
				newNode->add_line(line->value());
			}
			linesNode = node->last_node();
			for (xml_node<>* line = linesNode->first_node(); line; line = line->next_sibling())
			{
				ID = atoi(line->first_attribute()->value());
				newNode->add_answer(line->value(), ID);
			}
			dialogue->add_node(newNode);
		}
	}
	file.close();
}

bool Serializer::check_path(string path)
{
	size_t pos = path.find(".xml");
	bool res = pos != string::npos;
	return res;
}

void Serializer::create_doc_declaration()
{
	xml_node<>* decl = doc.allocate_node(node_declaration);
	decl->append_attribute(doc.allocate_attribute("version", "1.0"));
	decl->append_attribute(doc.allocate_attribute("encoding", "utf-8"));
	doc.append_node(decl);
}
