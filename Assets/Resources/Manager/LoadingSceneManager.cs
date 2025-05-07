using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    private static LoadingSceneManager instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static LoadingSceneManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public string currentscene;
    public string nextscene;
    
    Image progressbar;
    public void loadScene(string scenename)
    {
        currentscene = SceneManager.GetActiveScene().name;
        nextscene = scenename;
        SceneManager.LoadScene("Loading");
        for (int i = 0; i < GameManager.Instance.transform.childCount; i++)
        {
            GameManager.Instance.transform.GetChild(i).gameObject.SetActive(false);
        }
        StartCoroutine(loadScene());
    }
    IEnumerator loadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextscene);
        op.allowSceneActivation = false;
        progressbar = GameObject.Find("loadingbar").GetComponent<Image>();
        float elasped = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            elasped += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressbar.fillAmount = Mathf.Lerp(progressbar.fillAmount, op.progress, elasped);
                if (progressbar.fillAmount >= op.progress)
                {
                    elasped = 0f;
                }
            }
            else
            {
                progressbar.fillAmount = Mathf.Lerp(progressbar.fillAmount, 1f, elasped);
                if (progressbar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                }
            }
        }
        for (int i = 0; i < GameManager.Instance.transform.childCount; i++)
        {
            GameManager.Instance.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}