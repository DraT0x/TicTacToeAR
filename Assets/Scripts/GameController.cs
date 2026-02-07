// Inspiré à 100% des notes de cours d'Environnement Immersif
// Auteur : Frédérik Taleb
// https://envimmersif-cegepvicto.github.io/exercice_adaptation_ar/

using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Singleton
    public static GameController Instance { get; private set; }

    // UI UNITY
    [SerializeField]
    private TextMeshProUGUI joueurActuelTexte;

    [SerializeField]
    private GameObject finPartiePannel;

    [SerializeField]
    private TextMeshProUGUI victoireTexte;

    // Références Tic Tac Toe
    private GameObject grilleActuelle;
    private string[] grilleCases = new string[9];

    // Inspiré de : https://stackoverflow.com/questions/549399/c-sharp-creating-an-array-of-arrays
    private static int[,] combosGagnant = {
        {0,1,2}, {3,4,5}, {6,7,8}, // Lignes
        {0,3,6}, {1,4,7}, {2,5,8}, // Colomnes
        {0,4,8}, {2,4,6} // Diagonales
    };

    private string joueurActuel = "X";

    private bool finPartie = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Gestion Partie
    public void NewGame(GameObject nouvelleGrille)
    {
        grilleActuelle = nouvelleGrille;

        for (int i = 0; i < grilleCases.Length; i++)
            grilleCases[i] = "";

        joueurActuel = "X";
        joueurActuelTexte.text = $"Tour de {joueurActuel}";

        finPartie = false;
        finPartiePannel.SetActive(false);
    }

    public void ResetPlacement()
    {
        joueurActuel = "X";
        finPartie = false;

        joueurActuelTexte.text = $"Tour de {joueurActuel}";
    }

    // Jeu
    public void JouerCase(int index)
    {
        if (finPartie) return;

        grilleCases[index] = joueurActuel;

        if (VerifierVictoire())
        {
            FinDePartie($"Victoire de {joueurActuel}");
            return;
        }

        if (VerifierMatchNul())
        {
            FinDePartie("Match Nul !");
            return;
        }

        ChangerTour();
    }

    public void ChangerTour()
    {
        joueurActuel = joueurActuel == "X" ? "O" : "X";

        joueurActuelTexte.text = $"Tour de {joueurActuel}";
    }

    // Vérification Match
    private bool VerifierVictoire()
    {
        return false;
    }

    private bool VerifierMatchNul()
    {
        return true;
    }

    private void FinDePartie(string texteAffichage)
    {
        finPartie = true;

        victoireTexte.text = texteAffichage;
        finPartiePannel.SetActive(true);
    }
}
