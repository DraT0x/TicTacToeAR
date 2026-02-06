// Inspiré à 100% des notes de cours d'Environnement Immersif
// Auteur : Frédérik Taleb
// https://envimmersif-cegepvicto.github.io/exercice_adaptation_ar/

using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARStatusFeedback : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private TextMeshProUGUI statusText;

    private int planesCount = 0;

    void Update()
    {
        // Compter les plans détectés
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
