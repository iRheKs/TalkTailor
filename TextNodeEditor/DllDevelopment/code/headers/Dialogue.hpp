#pragma once
#include "Node.hpp"

class Serializer;

/*!
 * Class that represents a dialogue to be serialized
 * 
 */
class Dialogue {

	friend class Serializer;

private:

	string path, name;
	vector<Node*> nodes;
	Serializer * serializer = nullptr;
	bool fileExists = false;

public:

	Dialogue();
	/*!
	 * Constructs a dialogue within a given path and name. Also you can chose if an initial node will be created
	 * 
	 * \param path for the dialogue to be serialized
	 * \param name of the dialogue
	 * \param initialNode posibility of creating an initial node if true
	 */
	Dialogue(string path, string name, bool initialNode = false);
	~Dialogue() {}

	/*!
	 * Adds a new node to the dialogue ordered by node id
	 * 
	 */
	void add_node();
	/*!
	 * Adds a given node to the dialogue ordered by node id
	 * 
	 * \param node given to be added
	 */
	void add_node(Node* node);
	/*!
	 * Adds a line to a node
	 * 
	 * \param nodeID of the desired node
	 * \param line text of the node line to be added
	 */
	void add_line_to_node(int nodeID, string line);
	/*!
	 * Adds an answer to a node
	 * 
	 * \param currentNodeID id of the desired node to add
	 * \param connectedNodeID where is the connection going
	 * \param answer text of the node answer to be added
	 */
	void add_answer_to_node(int currentNodeID, int connectedNodeID, string answer);
	/*!
	 * Removes the last node of the dialogue
	 * 
	 */
	void remove_last_node();
	/*!
	 * Removes a node with a given nodeID
	 * 
	 * \param nodeID of the node to be removed
	 */
	void remove_node_by_id(int nodeID);
	/*!
	 * Removes a line from a desired node
	 * 
	 * \param nodeID id of the node to remove from
	 * \param position index of the line to remove ( if -1 will remove the last )
	 */
	void remove_line_from_node(int nodeID, int position = -1);
	/*!
	 * Removes an answer from a desired node
	 * 
	 * \param nodeID id of the node to remove from
	 * \param position index of the answer to remove ( if -1 will remove the last )
	 */
	void remove_answer_from_node(int nodeID, int position = -1);
	/*!
	 * Sets a new name to the dialogue
	 * 
	 * \param newName of the dialogue to be set
	 */
	void set_name(string newName) 
	{
		name = newName;
	}
	/*!
	 * Gets the name of the dialogue
	 * 
	 * \return dialogue name
	 */
	string get_name() 
	{
		return name;
	}
	/*!
	 * Returns a node with a given ID
	 * 
	 * \param nodeID of the node to be retrieved
	 * \return the reference to the desired node
	 */
	Node* get_node_by_id(int nodeID);
	/*!
	 * Return the node ID placed in a specific position of the vector
	 * 
	 * \param index of the vector position
	 * \return ID of the node
	 */
	int get_node_id(int index);
	/*!
	 * Saves the dialogue into the xml file that is already created
	 * 
	 */
	void save();
	/*!
	 * Saves the dialogue in a new xml file that will be created in the specified path.
	 * (Path must have the file name and extension at the end)
	 * 
	 * \param filePath to create the new xml file
	 */
	void export_xml(string filePath);
	/*!
	 * Loads a dialogue from a specified xml
	 * 
	 * \param filePath to load from
	 */
	void load(string filePath);
	/*!
	 * Gets the current path of the dialogue
	 * 
	 * \return path of the dialogue
	 */
	string get_path() 
	{
		return path;
	}
	/*!
	 * Sets a new current path to the dialogue
	 * 
	 * \param filePath of the new file
	 */
	void set_path(string filePath) {
		path = filePath;
	}
	/*!
	 * Returns the size of the node vector
	 * 
	 * \return size of the node vector
	 */
	int get_nodes_length() 
	{
		return (int)nodes.size();
	}
	/*!
	 * Returns the next availabe id. If theres no id available will return node vector size (if current id nodes are: 1,2,3; this will return: 4)
	 * Always will be the lowest number (if current id nodes are: 1,3,5; this will return: 2)
	 * 
	 * \return available ID
	 */
	int next_id_available();
};