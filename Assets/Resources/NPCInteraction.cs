using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    private enum NPCState
    {
        ReadyToTalk,
        HasTalked,
        Idle
    }

    private NPCState npcState = NPCState.ReadyToTalk;

    public int dialogueId;
    // public Text idleDialogueText;
    public string fixedDialogue;
    public float typeSpeed = 0.05f;
    public GameObject uiPrefab;
    private GameObject uiInstance;
    
    private bool isPlayerNear;

    void Start()
    {
        // idleDialogueText.text = "";
    }
    private void Update()
    {
        interAction();
    }
    void interAction()
    {
        if (npcState == NPCState.ReadyToTalk && Input.GetKeyDown(KeyCode.C))
        {
            if (uiInstance != null)
                Destroy(uiInstance);
            StartDialogue();
            Vector2 pos = new Vector2(transform.position.x-2, transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.right, 4.0f, LayerMask.GetMask("NPC"));
            if (!hit)
            {
                return;
            }
            if (npcState == NPCState.ReadyToTalk)
            {
                GameObject canvas = GameObject.Find("TestCanvas");
                uiInstance = Instantiate(uiPrefab, canvas.transform);
                uiInstance.transform.position = gameObject.transform.position + new Vector3(0, 2f, 0);
                Debug.Log("test1");
            }
            else if (npcState == NPCState.HasTalked)
            {
                if (fixedDialogue != null)
                {
                    BubbleChat bubbleChat = gameObject.GetComponent<BubbleChat>();
                    bubbleChat.showBubble(GameObject.Find("TestCanvas"), gameObject, fixedDialogue);
                }
            }
        }
    }
    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        isPlayerNear = true;
    //        if (npcState == NPCState.ReadyToTalk)
    //        {
    //            GameObject canvas = GameObject.Find("TestCanvas");
    //            uiInstance = Instantiate(uiPrefab, canvas.transform);
    //            uiInstance.transform.position = gameObject.transform.position + new Vector3(0, 2f, 0);
    //            Debug.Log("test1");
    //        }
    //        else if (npcState == NPCState.HasTalked)
    //        {
    //            if (fixedDialogue != null)
    //            {
    //                BubbleChat bubbleChat = gameObject.GetComponent<BubbleChat>();
    //                bubbleChat.showBubble(GameObject.Find("TestCanvas"), gameObject, fixedDialogue);
    //            }
    //        }
    //    }
    //}

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (uiInstance != null)
            {
                Destroy(uiInstance);
            }
            if (fixedDialogue != null)
            {
                BubbleChat bubbleChat = gameObject.GetComponent<BubbleChat>();
                bubbleChat.removeBubble(gameObject);
            }
        }
    }

    void StartDialogue()
    {
        Debug.Log("대화 시작");
        DialogueManager.Instance.StartDialogue(dialogueId);
        npcState = NPCState.HasTalked;
    }
}