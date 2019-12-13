using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractCollider : MonoBehaviour
{
    public GameObject affichage;
    public PlayerActions playerActions;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        playerActions = gameObject.GetComponentInParent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Drop")
        {
            playerActions.interactable = other.gameObject;
            text.text = "Press <#16A085>F</color> to pick up \"<#3498DB>" + other.gameObject.name + "</color>\"";
            affichage.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Drop" && (playerActions.interactable == null || other.gameObject != playerActions.interactable && Vector3.Distance(other.transform.position, transform.position) < Vector3.Distance(playerActions.interactable.transform.position, transform.position)))
        {
            playerActions.interactable = other.gameObject;
            text.text = "Press <#16A085>F</color> to pick up \"<#3498DB>" + other.gameObject.name + "</color>\"";
            affichage.SetActive(true);
            Debug.Log("u r in colider");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Drop" && playerActions.interactable == other.gameObject)
        {
            playerActions.interactable = null;
            DisableText();
            Debug.Log("exit");
        }
    }

    public void DisableText()
    {
        affichage.SetActive(false);
        text.text = "";
    }
}
