using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

class Program
{
   
    [DllImport("E:/LockBomb/TextNodeEditor/DllDevelopment/TextEditorDll/x64/Release/TextEditorDll.dll", EntryPoint = "createDialogue")]
    private static extern void createDialogue(string path, string name);

    [DllImport("E:/LockBomb/TextNodeEditor/DllDevelopment/TextEditorDll/x64/Release/TextEditorDll.dll", EntryPoint = "addNode")]
    public static extern void addNode(string name);

    [DllImport("E:/LockBomb/TextNodeEditor/DllDevelopment/TextEditorDll/x64/Release/TextEditorDll.dll", EntryPoint = "saveDialogue")]
    public static extern void saveDialogue();

    [DllImport("E:/LockBomb/TextNodeEditor/DllDevelopment/TextEditorDll/x64/Release/TextEditorDll.dll", EntryPoint = "getNodeName")]
    public static extern void getNodeName(int nodeID, StringBuilder name, int stringLength);

    public static void CreateDialogue(string path, string name)
    {
        createDialogue(path,name);
    }

    static void Main(string[] args)
    {
        string path = "E:/LockBomb/TextNodeEditor/Assets/Dialogues/Dialogue.xml";
        CreateDialogue(path, "Dialogue");

        addNode("Nodorl_CSharp");

        saveDialogue();

        StringBuilder sb = new StringBuilder(10);

        getNodeName(1,sb,sb.Capacity);

        string nodeName = sb.ToString();

        Console.WriteLine("Node name: " + nodeName);

        Console.ReadLine();
    }
}

