#include "headers\DLLExternal.hpp"
#include <assert.h>

#define AppDialogue Application::get_instance()->currentDialogue

TEXT_NODE_EDITOR_API _int64 createDialogue(const char * path, const char * name)
{
	AppDialogue = new Dialogue(path,name);
	return reinterpret_cast<_int64>(AppDialogue);
}

TEXT_NODE_EDITOR_API void swapDialogue(_int64 dialoguePointer)
{
	AppDialogue = reinterpret_cast<Dialogue*>(dialoguePointer);
}

TEXT_NODE_EDITOR_API _int64 loadDialogue(const char * path)
{
	if (AppDialogue != nullptr)
		AppDialogue->load(path);
	else 
	{
		AppDialogue = new Dialogue(path,"");
		AppDialogue->load(path);
	}
	return reinterpret_cast<_int64>(AppDialogue);
}

TEXT_NODE_EDITOR_API int addNode(const char * name)
{
	//assert(AppDialogue);
	int id = AppDialogue->next_id_available();
	AppDialogue->add_node(new Node(id, name));
	return id;
}

TEXT_NODE_EDITOR_API void addLine(int nodeID, const char * text)
{
	//assert(AppDialogue);
	AppDialogue->add_line_to_node(nodeID,text);
}

TEXT_NODE_EDITOR_API void addUnconnectedAnswer(int nodeID, const char * text)
{
	AppDialogue->add_answer_to_node(nodeID, -1, text);
}

TEXT_NODE_EDITOR_API void addAnswer(int nodeID, int connectedNodeID, const char * text)
{
	//assert(AppDialogue);
	AppDialogue->add_answer_to_node(nodeID, connectedNodeID, text);
}

TEXT_NODE_EDITOR_API void deleteNode(int nodeID)
{
	//assert(AppDialogue);
	AppDialogue->remove_node_by_id(nodeID);
}

TEXT_NODE_EDITOR_API void deleteLine(int nodeID, int lineIndex)
{
	//assert(AppDialogue);
	AppDialogue->remove_line_from_node(nodeID, lineIndex);
}

TEXT_NODE_EDITOR_API void deleteAnswer(int nodeID, int answerIndex)
{
	//assert(AppDialogue);
	AppDialogue->remove_answer_from_node(nodeID, answerIndex);
}

TEXT_NODE_EDITOR_API void saveDialogue()
{
	//assert(AppDialogue);
	AppDialogue->save();
}

TEXT_NODE_EDITOR_API void exportDialogue(const char * path)
{
	//assert(AppDialogue);
	AppDialogue->export_xml(path);
}

TEXT_NODE_EDITOR_API void getDialogueName(char * name, int nameLength)
{
	string dialogueName = AppDialogue->get_name();
	if (dialogueName.length() > nameLength)
	{
		dialogueName.resize((size_t)nameLength);
		dialogueName.shrink_to_fit();
	}
	strcpy(name, dialogueName.c_str());
}

TEXT_NODE_EDITOR_API void getNodeName(int nodeID, char * name, int nameLength)
{
	//assert(AppDialogue);
	Node* node = AppDialogue->get_node_by_id(nodeID);
	string nodeName = node->get_name();
	if (nodeName.length() > nameLength)
	{
		nodeName.resize((size_t)nameLength);
		nodeName.shrink_to_fit();
	}
	strcpy(name, nodeName.c_str());
}

TEXT_NODE_EDITOR_API int getNodeID(int nodeIndex)
{
	//assert(AppDialogue);
	return AppDialogue->get_node_id(nodeIndex);
}

TEXT_NODE_EDITOR_API void getLineAt(int nodeID, int lineIndex, char * line, int lineLength)
{
	//assert(AppDialogue);
	Node* node = AppDialogue->get_node_by_id(nodeID);
	string nodeLine = node->get_line_at(lineIndex);
	if (nodeLine.length() > lineLength)
	{
		nodeLine.resize((size_t)lineLength);
		nodeLine.shrink_to_fit();
	}
	strcpy(line, nodeLine.c_str());
}

TEXT_NODE_EDITOR_API void getAnswerAt(int nodeID, int answerIndex, char * answer, int answerLength)
{
	//assert(AppDialogue);
	Node* node = AppDialogue->get_node_by_id(nodeID);
	string nodeAnswer = node->get_line_at(answerIndex);
	if (nodeAnswer.length() > answerLength)
	{
		nodeAnswer.resize((size_t)answerLength);
		nodeAnswer.shrink_to_fit();
	}
	strcpy(answer, nodeAnswer.c_str());
}

TEXT_NODE_EDITOR_API int getConnectionFrom(int nodeID, int answerIndex)
{
	//assert(AppDialogue);
	return AppDialogue->get_node_by_id(nodeID)->get_answer_connection(answerIndex);
}

TEXT_NODE_EDITOR_API void changeDialogueName(const char * newName)
{
	AppDialogue->set_name(newName);
}

TEXT_NODE_EDITOR_API void changeNodeName(int nodeID, const char * newName)
{
	//assert(AppDialogue);
	AppDialogue->get_node_by_id(nodeID)->set_name(newName);
}

TEXT_NODE_EDITOR_API void changeLine(int nodeID, int lineIndex, const char * newLine)
{
	//assert(AppDialogue);
	AppDialogue->get_node_by_id(nodeID)->change_line(lineIndex, newLine);
}

TEXT_NODE_EDITOR_API void changeAnswer(int nodeID, int answerIndex, const char * newAnswer)
{
	//assert(AppDialogue);
	AppDialogue->get_node_by_id(nodeID)->change_answer(answerIndex, newAnswer);
}

TEXT_NODE_EDITOR_API void switchConnection(int nodeID, int answerIndex, int newConnectionID)
{
	//assert(AppDialogue);
	AppDialogue->get_node_by_id(nodeID)->switch_connection(answerIndex, newConnectionID);
}

TEXT_NODE_EDITOR_API void deleteConnection(int nodeID, int answerIndex)
{
	//assert(AppDialogue);
	AppDialogue->get_node_by_id(nodeID)->delete_connection(answerIndex);
}
