using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Animator SceneChanger;

    private void Start()
    {
        SceneChanger.Play("In");
    }

    public void loadScene(string name)
    {
        StartCoroutine(changeScene(name));
    }

    IEnumerator changeScene(string name)
    {
        SceneChanger.Play("Out");

        yield return new WaitForSeconds(SceneChanger.GetCurrentAnimatorStateInfo(0).length);

        SceneManager.LoadScene(name);
    }
}
