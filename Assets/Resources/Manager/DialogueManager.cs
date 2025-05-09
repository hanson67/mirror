using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueData
{
    public int id;
    public string illustration;
    public List<int> conditions;
    public List<int> useditems;
    public string debug;
    public List<string> sentences;
    public List<Choice> choices;
}

[System.Serializable]
public class Choice
{
    public string text;
    public int[] conditions;
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
            DontDestroyOnLoad(this.gameObject);
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
        {
            dialogueDict[dialogue.id] = dialogue;
            //체크요망
            StoryManager.Instance.dialogueRed[dialogue.id] = false;
            StoryManager.Instance.itemUsed[dialogue.id] = false;
        }

    }
    public void alertDebug(int dialogueId)
    {
        if (dialogueDict[dialogueId].debug == null) return;
        GameManager.Instance.DialogFrame.gameObject.SetActive(true);
        GameManager.Instance.DialogFrame.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{dialogueDict[dialogueId].debug}";
        GameManager.Instance.DialogFrame.GetComponent<Button>().onClick.AddListener(() => endAlertDebug());
        GameManager.movable = false;
    }
    void endAlertDebug()
    {
        GameManager.Instance.DialogFrame.gameObject.SetActive(false);
        GameManager.Instance.DialogFrame.GetComponent<Button>().onClick.RemoveAllListeners();
        GameManager.movable = true;
    }
    public bool StartDialogue(int dialogueId)
    {
        if (!dialogueDict.ContainsKey(dialogueId))
            return false;
        if (StoryManager.Instance.dialogueRed[dialogueId])
            return true;
        foreach (int condition in dialogueDict[dialogueId].conditions)
        {
            if (!StoryManager.Instance.dialogueRed[condition])
            {
                alertDebug(dialogueId);
                return false;
            }
        }
        foreach (int useditem in dialogueDict[dialogueId].useditems)
        {
            if (!StoryManager.Instance.itemUsed[useditem])
            {
                alertDebug(dialogueId);
                return false;
            }
        }
        GameManager.movable = false;
        illustrationImage.gameObject.SetActive(false);
        currentDialogueId = dialogueId;
        currentSentenceIndex = 0;

        DialogueData dialogue = dialogueDict[dialogueId];
        dialoguePanel.SetActive(true);
        npcDialogueBox.SetActive(true);
        choicePanel.SetActive(false);
        if (dialogue.sentences == null || dialogue.sentences.Count == 0)
        {
            ShowChoices(dialogue);
        }
        else
        {
            ShowSentence();
        }
        return true;
    }

    public void OnClickNext()
    {
        Debug.Log(gameObject.name);
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
            string mostart ="";
            string moend = "";
            if (sentence.Contains("<독백>"))
            {
                mostart = "<color=#888888><size=32>...";
                moend = "</size></color>";
                sentence = sentence.Replace("<독백>", "");
                sentence = sentence.Replace("</독백>", "");
            }
            string content = "";
            foreach (char character in sentence)
            {
                content += character;
                dialogueText.text = mostart + content + moend;
                yield return new WaitForSeconds(0.05f);
            }
        } else if (textType == "alert")
        {
            dialogueText.GameObject().SetActive(false);
            alertText.GameObject().SetActive(true);
            dialogueName.GameObject().SetActive(false);
            if (sentence.Contains("<아이템>"))
            {
                string[] sentences = sentence.Split("<아이템>");
                sentence = sentences[0];
                string[] items = sentences[1].Split(",");
                foreach (string item in items)
                {
                    Item i = Resources.Load<Item>($"Item/{item}");
                    InventoryManager.Instance.OnGetItem(i);
                }
            }
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
        if (sentence.Contains("<사진>"))
        {
            string[] sentences = sentence.Split("<사진>");
            sentence= sentences[0];
            Sprite illust = Resources.Load<Sprite>($"Illustrations/{sentences[1]}");
            illustrationImage.sprite = illust;
            illustrationImage.gameObject.SetActive(illust != null);
        }
        if (!(sentence.Contains("<알림>")))
        {
            string[] sentences = sentence.Split("</이름>");
            dialogueName.text = sentences[0];
            sentence = sentences[1];
            StopAllCoroutines();
            StartCoroutine(TextRepeater(sentence, "text"));    
        }
        else
        {
            sentence = sentence.Replace("<알림>", "");
            StopAllCoroutines();
            StartCoroutine(TextRepeater(sentence, "alert"));
        }
    }

    void ShowChoices(DialogueData dialogue)
    {
        npcDialogueBox.SetActive(false);
        choicePanel.SetActive(true);
        StoryManager.Instance.dialogueRed[dialogue.id] = true;
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
                bool back = false;
                if (dialogue.choices[i].conditions != null)
                {
                    foreach (int condition in dialogue.choices[i].conditions)
                    {
                        if (!StoryManager.Instance.dialogueRed[condition])
                        {
                            back = true;
                            break;
                        }
                    }
                }
                if(back)
                {
                    back = false;
                    choiceButtons[i].onClick.RemoveAllListeners();
                    choiceButtons[i].GetComponent<Image>().color = Color.red;
                    continue;
                }
                choiceButtons[i].GetComponent<Image>().color = Color.white;
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
        GameManager.movable = true;
    }
}
