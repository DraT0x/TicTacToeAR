using System;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GrilleCase : MonoBehaviour
{
    [SerializeField]
    private GameObject PrefabXSymbol;
    [SerializeField]
    private GameObject PrefabOSymbol;

    private int index;

    void Start()
    {
        index = Int32.Parse(gameObject.name);
        gameObject.tag = "Libre";
    }

    public void OnTapped()
    {
        if (gameObject.tag != "Libre") return;
        gameObject.tag = GameController.Instance.JoueurActuel;

        Instantiate(gameObject.tag == "X" ? PrefabXSymbol : PrefabOSymbol, transform.position, transform.rotation, transform);
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        GameController.Instance.JouerCase(index);
    }
}
