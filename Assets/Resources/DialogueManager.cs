using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueData
{
    public int id;
    public string illustration;
    public List<string> sentences;
    public List<Choice> choices;
}

[System.Serializable]
public class Choice
{
    public string text;
    public int nextDialogueId;
}

[System.Serializable]
public class DialogueList
{
    public List<DialogueData> dialogues;
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public Text dialogueName;
    public GameObject dialoguePanel;
    public GameObject npcDialogueBox;
    public Button nextButton;
    public Text alertText;
    public Text dialogueText;
    public GameObject choicePanel;
    public List<Button> choiceButtons;
    public Image illustrationImage;
    public TextAsset dialogueJsonFile;

    private Dictionary<int, DialogueData> dialogueDict = new Dictionary<int, DialogueData>();
    private int currentDialogueId;
    private int currentSentenceIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        LoadDialogueData();
        dialoguePanel.SetActive(false);
    }

    void LoadDialogueData()
    {
        if (dialogueJsonFile == null)
        {
            Debug.LogError("Dialogue JSON file not assigned.");
            return;
        }

        DialogueList loadedData = JsonUtility.FromJson<DialogueList>(dialogueJsonFile.text);
        foreach (DialogueData dialogue in loadedData.dialogues)
            dialogueDict[dialogue.id] = dialogue;
    }
    public void StartDialogue(int dialogueId)
    {
        if (!dialogueDict.ContainsKey(dialogueId))
        {
            Debug.LogError("존재하지 않는 아이디임, 아이디 번호 : " + dialogueId);
            return;
        }

        currentDialogueId = dialogueId;
        currentSentenceIndex = 0;

        DialogueData dialogue = dialogueDict[dialogueId];
        dialoguePanel.SetActive(true);
        npcDialogueBox.SetActive(true);
        choicePanel.SetActive(false);

        if (!string.IsNullOrEmpty(dialogue.illustration))
        {
            Sprite illust = Resources.Load<Sprite>(dialogue.illustration);
            illustrationImage.sprite = illust;
            illustrationImage.gameObject.SetActive(illust != null);
        }
        else
        {
            illustrationImage.gameObject.SetActive(false);
        }

        if (dialogue.sentences == null || dialogue.sentences.Count == 0)
        {
            ShowChoices(dialogue);
        }
        else
        {
            ShowSentence();
        }
    }

    public void OnClickNext()
    {
        DialogueData dialogue = dialogueDict[currentDialogueId];
        currentSentenceIndex++;

        if (currentSentenceIndex >= dialogue.sentences.Count)
        {
            ShowChoices(dialogue);
        }
        else
        {
            ShowSentence();
        }
    }

    IEnumerator TextRepeater(string sentence, string textType)
    {
        dialogueText.text = "";
        alertText.text = "";
        if (textType == "text")
        {
            dialogueText.GameObject().SetActive(true);
            alertText.GameObject().SetActive(false);
            dialogueName.GameObject().SetActive(true);
            foreach (char character in sentence)
            {
                dialogueText.text += character;
                yield return new WaitForSeconds(0.05f);
            }
        } else if (textType == "alert")
        {
            dialogueText.GameObject().SetActive(false);
            alertText.GameObject().SetActive(true);
            dialogueName.GameObject().SetActive(false);
            foreach (char character in sentence)
            {
                alertText.text += character;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    
    void ShowSentence()
    {
        DialogueData dialogue = dialogueDict[currentDialogueId];
        string sentence = dialogue.sentences[currentSentenceIndex];
        if (!(sentence.Contains("<알림>")))
        {
            string[] sentences = sentence.Split("</이름>");
            dialogueName.text = sentences[0];
            sentence = sentences[1];
            sentence = sentence.Replace("<독백>", "<color=#888888><size=32>...");
            sentence = sentence.Replace("</독백>", "</size></color>");
            StopAllCoroutines();
            StartCoroutine(TextRepeater(sentence, "text"));    
        }
        else
        {
            sentence = sentence.Replace("<알림>", "");
            sentence = sentence.Replace("<독백>", "<color=#888888><size=32>...");
            sentence = sentence.Replace("</독백>", "</size></color>");
            StopAllCoroutines();
            StartCoroutine(TextRepeater(sentence, "alert"));
        }
        
    }

    void ShowChoices(DialogueData dialogue)
    {
        npcDialogueBox.SetActive(false);
        choicePanel.SetActive(true);

        if (dialogue.choices == null || dialogue.choices.Count == 0)
        {
            EndDialogue();
            return;
        }

        for (int i = 0; i < choiceButtons.Count; i++)
        {
            if (i < dialogue.choices.Count)
            {
                int index = i;
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<Text>().text = dialogue.choices[index].text;
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(dialogue.choices[index].nextDialogueId));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnChoiceSelected(int nextId)
    {
        if (nextId == 0)
        {
            EndDialogue();
        }
        else
        {
            StartDialogue(nextId);
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
