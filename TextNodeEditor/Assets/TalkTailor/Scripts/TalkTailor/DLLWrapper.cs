using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
namespace TextEditor
{
    public static class DLLWrapper
    {
        private static StringBuilder names = new StringBuilder(32);
        private static StringBuilder texts = new StringBuilder(128);

        [DllImport("TextEditorDll", EntryPoint = "createDialogue")]
        private static extern long createDialogue(string path, string name);

        [DllImport("TextEditorDll", EntryPoint = "swapDialogue")]
        private static extern void swapDialogue(long dialoguePointer);

        [DllImport("TextEditorDll", EntryPoint = "loadDialogue")]
        private static extern long loadDialogue(string path);

        [DllImport("TextEditorDll", EntryPoint = "addNode")]
        private static extern int addNode(string name);

        [DllImport("TextEditorDll", EntryPoint = "addLine")]
        private static extern void addLine(int nodeID, string text);

        [DllImport("TextEditorDll", EntryPoint = "addUnconnectedAnswer")]
        private static extern void addUnconnectedAnswer(int nodeID, string text);

        [DllImport("TextEditorDll", EntryPoint = "addAnswer")]
        private static extern void addAnswer(int nodeID, int connectedNodeID, string text);

        [DllImport("TextEditorDll", EntryPoint = "deleteNode")]
        private static extern void deleteNode(int nodeID);

        [DllImport("TextEditorDll", EntryPoint = "deleteLine")]
        private static extern void deleteLine(int nodeID, int lineIndex);

        [DllImport("TextEditorDll", EntryPoint = "deleteAnswer")]
        private static extern void deleteAnswer(int nodeID, int answerIndex);

        [DllImport("TextEditorDll", EntryPoint = "deleteConnection")]
        private static extern void deleteConnection(int nodeID, int answerIndex);

        [DllImport("TextEditorDll", EntryPoint = "getDialogueName")]
        private static extern void getDialogueName(StringBuilder name, int nameLength);

        [DllImport("TextEditorDll", EntryPoint = "getNodeID")]
        private static extern int getNodeID(int nodeIndex);

        [DllImport("TextEditorDll", EntryPoint = "getNodeName")]
        private static extern void getNodeName(int nodeID, StringBuilder name, int nameLength);

        [DllImport("TextEditorDll", EntryPoint = "getLineAt")]
        private static extern void getLineAt(int nodeID, int lineIndex, StringBuilder line, int lineLength);

        [DllImport("TextEditorDll", EntryPoint = "getAnswerAt")]
        private static extern void getAnswerAt(int nodeID, int answerIndex, StringBuilder answer, int answerLength);

        [DllImport("TextEditorDll", EntryPoint = "getConnectionFrom")]
        private static extern int getConnectionFrom(int nodeID, int answerIndex);

        [DllImport("TextEditorDll", EntryPoint = "changeDialogueName")]
        private static extern void changeDialogueName(string newName);

        [DllImport("TextEditorDll", EntryPoint = "changeNodeName")]
        private static extern void changeNodeName(int nodeID, string newName);

        [DllImport("TextEditorDll", EntryPoint = "changeLine")]
        private static extern void changeLine(int nodeID, int lineIndex, string newLine);

        [DllImport("TextEditorDll", EntryPoint = "changeAnswer")]
        private static extern void changeAnswer(int nodeID, int answerIndex, string newAnswer);

        [DllImport("TextEditorDll", EntryPoint = "switchConnection")]
        private static extern void switchConnection(int nodeID, int answerIndex, int newConnectionID);

        [DllImport("TextEditorDll", EntryPoint = "saveDialogue")]
        private static extern void saveDialogue();

        [DllImport("TextEditorDll", EntryPoint = "exportDialogue")]
        private static extern void exportDialogue(string path);


        public static long CreateDialogue(string path, string name)
        {
            return createDialogue(path, name);
        }

        public static void SwapDialogue(long pointer)
        {
            swapDialogue(pointer);
        }

        public static long LoadDialogue(string path)
        {
            return loadDialogue(path);
        }

        public static void ExportDialogue(string path)
        {
            exportDialogue(path);
        }

        public static void SaveDialogue()
        {
            saveDialogue();
        }

        public static int AddNode(string name)
        {
            return addNode(name);
        }

        public static void AddLine(int nodeID, string line)
        {
            addLine(nodeID, line);
        }

        public static void AddAnswer(int nodeID, string answer, int connectedNodeID = -1)
        {
            if (connectedNodeID == -1)
            {
                addUnconnectedAnswer(nodeID, answer);
            }
            else
            {
                addAnswer(nodeID, connectedNodeID, answer);
            }
        }

        public static void DeleteNode(int nodeID)
        {
            deleteNode(nodeID);
        }

        public static void DeleteLine(int nodeID, int lineIndex)
        {
            deleteLine(nodeID, lineIndex);
        }

        public static void DeleteAnswer(int nodeID, int answerIndex)
        {
            deleteAnswer(nodeID, answerIndex);
        }

        public static void DeleteConnection(int nodeID, int answerIndex)
        {
            deleteConnection(nodeID, answerIndex);
        }

        public static int GetNodeID(int nodeIndex)
        {
            return getNodeID(nodeIndex);
        }

        public static string GetDialogueName()
        {
            names.Clear();

            getDialogueName(names, names.Capacity);

            return names.ToString();
        }

        public static string GetNodeName(int nodeID)
        {
            names.Clear();

            getNodeName(nodeID, names, names.Capacity);

            return names.ToString();
        }

        public static string GetLineAt(int nodeID, int lineIndex)
        {
            texts.Clear();

            getLineAt(nodeID, lineIndex, texts, texts.Capacity);

            return texts.ToString();
        }

        public static string GetAnswerAt(int nodeID, int answerIndex)
        {
            texts.Clear();

            getAnswerAt(nodeID, answerIndex, texts, texts.Capacity);

            return texts.ToString();
        }

        public static int GetConnectionFrom(int nodeID, int answerIndex)
        {
            return getConnectionFrom(nodeID, answerIndex);
        }

        public static void ChangeDialogueName(string newName)
        {
            changeDialogueName(newName);
        }

        public static void ChangeNodeName(int nodeID, string newName)
        {
            changeNodeName(nodeID, newName);
        }

        public static void ChangeLine(int nodeID, int lineIndex, string newLine)
        {
            changeLine(nodeID, lineIndex, newLine);
        }

        public static void ChangeAnswer(int nodeID, int answerIndex, string newAnswer)
        {
            changeAnswer(nodeID, answerIndex, newAnswer);
        }

        public static void ChangeConnection(int nodeID, int answerIndex, int newConnectionID)
        {
            switchConnection(nodeID, answerIndex, newConnectionID);
        }
    }
}
