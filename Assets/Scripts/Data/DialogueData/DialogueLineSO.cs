using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "RPG Setup/Dialogue Data/New Line Data", fileName = "Line - ")]
public class DialogueLineSO : ScriptableObject
{
    [Header("Dialogue info")]
    public string dialogueGroupName;
    public DialogueSpeakerSO speaker;

    [Header("Text options")]
    [TextArea] public string[] textLine;

    [Header("Answer setup")]
    public bool playCanAnswer;
    public DialogueLineSO[] answerLine;

    public string GetRandomLine()
    {
        return textLine[Random.Range(0, textLine.Length)];
    }
}
