// Inspiré des notes de cours d'Environnement Immersif et de la Démo AR
// Auteur : Frédérik Taleb
// https://envimmersif-cegepvicto.github.io/exercice_adaptation_ar/

using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARStatusFeedback : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private ARPlaneManager planeManager;
    
    [Header("GUI")]
    [SerializeField] private TextMeshProUGUI statusText;

    private int planesCount = 0;

    void Update()
    {
        planesCount = planeManager.trackables.count;

        if (planesCount == 0)
        {
            statusText.text = "Scannez une surface...";
            statusText.color = Color.yellow;
        }
        else
        {
            statusText.text = $"Prêt ! {planesCount} surface(s) détectée(s)";
            statusText.color = Color.green;
        }
    }
}
