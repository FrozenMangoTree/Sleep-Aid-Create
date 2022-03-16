using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUpMessage : MonoBehaviour
{
    TextMeshProUGUI title, message;

    void Awake()
    {
        title = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        message = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }
    public void SetMessage(string Message, float LengthOfMessage = 1f, string Title = default)
    {
        message.text = Message;
        title.text = Title;

        Transform canvas = GameObject.Find("Main Canvas").transform;
        transform.parent = canvas;
        transform.position = canvas.transform.position;

        Destroy(gameObject, LengthOfMessage);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
        }
    }
}
