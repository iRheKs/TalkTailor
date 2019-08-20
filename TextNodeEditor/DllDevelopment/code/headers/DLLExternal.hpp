#pragma once

#define TEXT_NODE_EDITOR_API __declspec(dllexport) 

#include "Application.hpp"

extern "C" 
{

	TEXT_NODE_EDITOR_API _int64 createDialogue(const char* path, const char* name);

	TEXT_NODE_EDITOR_API void swapDialogue(_int64 dialoguePointer);
	
	TEXT_NODE_EDITOR_API _int64 loadDialogue(const char* path);

	TEXT_NODE_EDITOR_API int addNode(const char* name);

	TEXT_NODE_EDITOR_API void addLine(int nodeID, const char* text);

	TEXT_NODE_EDITOR_API void addUnconnectedAnswer(int nodeID, const char* text);

	TEXT_NODE_EDITOR_API void addAnswer(int nodeID, int connectedNodeID, const char* text);

	TEXT_NODE_EDITOR_API void deleteNode(int nodeID);

	TEXT_NODE_EDITOR_API void deleteLine(int nodeID, int lineIndex);

	TEXT_NODE_EDITOR_API void deleteAnswer(int nodeID, int answerIndex);

	TEXT_NODE_EDITOR_API void saveDialogue();

	TEXT_NODE_EDITOR_API void exportDialogue(const char* path);


	TEXT_NODE_EDITOR_API void getDialogueName(char* name, int nameLength);

	TEXT_NODE_EDITOR_API void getNodeName(int nodeID, char* name, int nameLength);

	TEXT_NODE_EDITOR_API int getNodeID(int nodeIndex);

	TEXT_NODE_EDITOR_API void getLineAt(int nodeID, int lineIndex, char* line, int lineLength);

	TEXT_NODE_EDITOR_API void getAnswerAt(int nodeID, int answerIndex, char* answer, int answerLength);

	TEXT_NODE_EDITOR_API int getConnectionFrom(int nodeID, int answerIndex);

	TEXT_NODE_EDITOR_API void changeDialogueName(const char* newName);

	TEXT_NODE_EDITOR_API void changeNodeName(int nodeID, const char* newName);

	TEXT_NODE_EDITOR_API void changeLine(int nodeID, int lineIndex, const char* newLine);

	TEXT_NODE_EDITOR_API void changeAnswer(int nodeID, int answerIndex, const char* newAnswer);

	TEXT_NODE_EDITOR_API void switchConnection(int nodeID, int answerIndex, int newConnectionID);

	TEXT_NODE_EDITOR_API void deleteConnection(int nodeID, int answerIndex);

}