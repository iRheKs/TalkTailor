#pragma once
#include"Includes.hpp"
#include <iostream>
/*!
 * Class that represents a node in the dialogue
 * 
 */
class Node {

private:

	int id = 0;
	string name;
	vector<string> lines;
	vector<pair<string, int>> answers;

	bool removable = true;

public:

	Node() {}
	/*!
	 * Most common constructor. Creates a node with an ID and a name
	 * 
	 * \param id of the node
	 * \param name of the node
	 */
	Node(int id, string name);
	/*!
	 * Creates a node with the first line
	 * 
	 * \param id of the node
	 * \param firstLine of the node
	 * \param name of the node
	 */
	Node(int id, string firstLine, string name );
	/*!
	 * Creates a node with the first answer
	 * 
	 * \param id of the node
	 * \param firstAnswerText of the node
	 * \param nodeConnectedTo id of the node answer is connected to
	 * \param name of the node
	 */
	Node(int id, string firstAnswerText, int nodeConnectedTo, string name);
	/*!
	 * Creates a node with the first line and the first answer
	 * 
	 * \param id of the node
	 * \param firstLine of the node
	 * \param firstAnswerText of the node
	 * \param nodeConnectedTo id of the node answer is connected to
	 * \param name of the node
	 */
	Node(int id, string firstLine, string firstAnswerText, int nodeConnectedTo, string name);
	/*!
	 * Creates a node with the posibility of being unremovable
	 * 
	 * \param id of the node
	 * \param isRemovable indicates either if node could be deleted or not
	 * \param name of the node
	 */
	Node(int id, bool isRemovable, string name);

	~Node() {}
	/*!
	 * Sets a new id to the node
	 * 
	 * \param newId to be setted
	 */
	void set_id(int newId) {
		id = newId;
	}
	/*!
	 * Gets the id of the node
	 * 
	 * \return id number
	 */
	int get_id() {
		return id;
	}
	/*!
	 * Sets a new name to the node
	 * 
	 * \param newName of the node
	 */
	void set_name(string newName) {
		this->name = name;
	}
	/*!
	 * Gets the current name of the node
	 * 
	 * \return current name of the node
	 */
	string get_name() {
		return name;
	}
	/*!
	 * Returns the line vector size
	 * 
	 * \return line vector size
	 */
	int get_line_number() {
		return (int)lines.size();
	}
	/*!
	 * Returns the value of a line at the given index
	 * 
	 * \param index to look from
	 * \return line at the desired index
	 */
	string get_line_at(int index) 
	{
		return lines[index];
	}
	/*!
	 * Returns the size of answer vector
	 * 
	 * \return answer vector size
	 */
	int get_answer_number() {
		return (int)answers.size();
	}
	/*!
	 * Returns the answer at a desired index
	 * 
	 * \param index to look from
	 * \return answer pair at the desired index
	 */
	pair<string, int> get_answer_at(int index) {
		return answers[index];
	}
	/*!
	 * Returns the connected ID of an answer
	 * 
	 * \param index to look from
	 * \return connected ID of an answer
	 */
	int get_answer_connection(int index)
	{
		return answers[index].second;
	}
	/*!
	 * Adds a line to the node
	 * 
	 * \param newLine value of the new line
	 */
	void add_line(string newLine) {
		lines.push_back(newLine);
	}
	/*!
	 * Changes the value of a desired line
	 * 
	 * \param index of the line to change
	 * \param newLine new value of the line
	 */
	void change_line(int index, string newLine) 
	{
		lines[index] = newLine;
	}
	/*!
	 * Adds an answer to the node
	 * 
	 * \param text of the answer
	 * \param nodeConnectedTo ID of the node to be connected with
	 */
	void add_answer(string text, int nodeConnectedTo) {
		answers.push_back(pair<string, int>(text,nodeConnectedTo));
	}
	/*!
	 * Changes the answer value of a desired answer
	 * 
	 * \param index of the answer to change
	 * \param newAnswer new value of the answer
	 */
	void change_answer(int index, string newAnswer)
	{
		answers[index].first = newAnswer;
	}
	/*!
	 * Change a connection of a desired answer
	 * 
	 * \param index of the answer to change connection
	 * \param newConnection ID of the new node to connect to
	 */
	void switch_connection(int index, int newConnection) 
	{
		answers[index].second = newConnection;
	}
	/*!
	 * Removes a line from a desired position
	 * 
	 * \param index of the line to remove
	 */
	void delete_line(int index);
	/*!
	 * Removes an answer from a desired position
	 * 
	 * \param index of the answer to remove
	 */
	void delete_answer(int index);
	/*!
	 * Removes a connection from a desired position
	 * 
	 * \param index of the answer to remove its connection
	 */
	void delete_connection(int index) 
	{
		answers[index].second = -1;
	}
	/*!
	 * Gets the removable state of the node
	 * 
	 * \return true if can be reomved or false if not
	 */
	bool is_removable() {
		return removable;
	}
	/*!
	 * Sets removable state of the node
	 * 
	 * \param state of removability
	 */
	void set_removable(bool state) {
		removable = state;
	}

};