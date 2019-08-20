#pragma once
#include "Dialogue.hpp"
#include "Includes.hpp"
#include "rapidxml_ext.h"
#include <fstream>

using namespace rapidxml;

/*!
 * Serializer is used to interact with rapidxml and create the desired xml files
 * 
 */
class Serializer {

private:

	string path;
	fstream file;
	xml_document<> doc;
	xml_node<>* root = nullptr;
	Dialogue * dialogue = nullptr;

public:

	Serializer(){}
	/*!
	 * Constructs a serializer with a reference of a dialogue
	 * 
	 * \param dialogue reference to the dialogue that will be used
	 */
	Serializer(Dialogue * dialogue);

	~Serializer(){}

	/*!
	 * Creates a new xml doc for a new dialogue
	 * 
	 * \param path of the file to be saved on
	 */
	void create_dialogue(string path, string name = "Dialogue");

	/*!
	 * Creates a new node in the xml doc
	 * 
	 * \param node reference of the node to get the information
	 */
	void create_node(Node * node);

	/*!
	 * Deletes all nodes of the xml file
	 * 
	 */
	void remove_nodes();

	/*!
	 * Opens an xml file from the desired path
	 * 
	 * \param path of the file to open
	 * \return a boolean which indicates if opening the file was sucssesfull
	 */
	bool open_file(string path);

	/*!
	 * Saves the xml file and in case it doesn't exists creates a new one
	 * it uses the path that the serializer already has
	 */
	void save();

	/*!
	 * Always creates and saves a xml file at the desired path
	 * it uses the info saved in the doc variable of the serializer
	 * 
	 * \param path where to export the xml file
	 */
	void create_xml(string path);

	/*!
	 * Updates the name of the Dialogue node (root)
	 * 
	 * \param name new name of the Dialogue
	 */
	void update_dialogue_name(string name);

	/*!
	 * Loads the information of the xml to the dialogue
	 * 
	 * \param dialogue to load the information to
	 */
	void load_to_dialogue(Dialogue* dialogue);

	/*!
	 * Creates the declaration block in the xml file
	 * 
	 */
	void create_doc_declaration();

	/*!
	 * Clears the xml_document
	 * 
	 */
	void clear_document() 
	{
		doc.clear();
	}
private:
	/*!
	 * Checks if the path that was given ends up with .xml
	 * 
	 * \param path to check from
	 * \return true if path is ok, else return false
	 */
	bool check_path(string path);
};