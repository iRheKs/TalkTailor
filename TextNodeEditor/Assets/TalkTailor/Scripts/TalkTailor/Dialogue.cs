using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextEditor
{
    [CreateAssetMenu(menuName = "Dialogue"), System.Serializable]
    public class Dialogue : ScriptableObject
    {

#if UNITY_EDITOR
        [SerializeField]
        private bool initialized;

        private bool firstOpened;

        [SerializeField]
        private DialogueEditor editor;
        public DialogueEditor GetEditor()
        {
            if (editor.dialogue == null)
                editor.dialogue = this;
            return editor;
        }

        public long pointer;

        public void Second()
        {
            firstOpened = true;
        }

        public bool IsCreated()
        {
            return initialized;
        }

        public bool IsFirst()
        {
            return !firstOpened;
        }

        public void CreateDialogue()
        {
            nodes = new List<NodeInfo>();
            pointer = DLLWrapper.CreateDialogue(path, dialogueName);
            isDialogue = true;
            firstOpened = true;
        }
        
        public string GetName()
        {
            dialogueName = DLLWrapper.GetDialogueName();
            return dialogueName;
        }

        public void AddNode(NodeInfo node)
        {
            nodes.Add(node);
        }

        public void RemoveNode(NodeInfo infoToRemove)
        {
            nodes.Remove(infoToRemove);
        }

#endif

        public string dialogueName;

        public bool isDialogue;

        public string path;
        [SerializeField]
        private List<NodeInfo> nodes;

        public bool dialogueStarted, dialogueFinished;
        [SerializeField]
        private int currentNodeIndex = 0;
        [SerializeField]
        private int currentLineIndex = 0;

        public string GetNextLine(int nodeID)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ID == nodeID)
                    currentNodeIndex = i;
            }
            if (dialogueStarted)
            {
                if(nodes[currentNodeIndex].lineNumber > currentLineIndex)
                {
                    return nodes[currentNodeIndex].lines[currentLineIndex++];
                }
                else
                {
                    currentLineIndex = 0;
                    if (currentNodeIndex < nodes.Count)
                        return nodes[currentNodeIndex].lines[currentLineIndex++];
                    else
                    {
                        dialogueFinished = true;
                        dialogueStarted = false;
                        return null;
                    }
                }
            }
            else
            {
                dialogueStarted = true;
                dialogueFinished = false;
                return nodes[currentNodeIndex].lines[0];
            }
        }

        public string[] GetAnswers(int nodeID)
        {
            string[] res = new string[nodes[currentNodeIndex].answerNumber];

            for (int i = 0; i < res.Length; i++)
            {
                res[i] = nodes[currentNodeIndex].answers[i].answerText;
            }

            return res;
        }

        public int GetConnectedNodeID(int answerIndex)
        {
            return nodes[currentNodeIndex].answers[answerIndex].connection;
        }

    }
}
