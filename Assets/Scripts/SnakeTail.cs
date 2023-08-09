using UnityEngine;

public class SnakeTail : MonoBehaviour
{
    public void follow(Vector2 nextPos)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform child = gameObject.transform.GetChild(i);
            Vector2 currentPos = child.position;
            child.position = nextPos;
            nextPos = currentPos;
        }
    }

}
