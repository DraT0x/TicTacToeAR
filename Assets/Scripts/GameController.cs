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
    private TextMeshProUGUI JoueurActuelTexte;

    [SerializeField]
    private GameObject finPartiePannel;

    [SerializeField]
    private GameObject controlPannel;

    [SerializeField]
    private GameObject statusPannel;

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

    public string JoueurActuel { get; set; } = "X";

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

        JoueurActuel = "X";
        JoueurActuelTexte.text = $"Tour de {JoueurActuel}";

        finPartie = false;
        finPartiePannel.SetActive(false);
    }

    public void ResetPlacement()
    {
        if (grilleActuelle == null) return;

        JoueurActuel = "X";
        finPartie = false;
        finPartiePannel.SetActive(false);

        for (int i = 0; i < grilleCases.Length; i++)
            grilleCases[i] = "";

        JoueurActuelTexte.text = $"Tour de {JoueurActuel}";

        // Reset de la grille
        for (int i = 0; i < grilleActuelle.transform.childCount; i++)
        {
            Transform caseEnfant = grilleActuelle.transform.GetChild(i);

            for (int j = caseEnfant.childCount - 1; j >= 0; j--)
            {
                Destroy(caseEnfant.GetChild(j).gameObject);
            }

            MeshRenderer meshRenderer = caseEnfant.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                meshRenderer.enabled = true;

            caseEnfant.tag = "Libre";
        }
    }

    // Jeu
    public void JouerCase(int index)
    {
        if (finPartie) return;

        grilleCases[index] = JoueurActuel;

        if (VerifierVictoire())
        {
            FinDePartie($"Victoire de {JoueurActuel}");
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
        JoueurActuel = JoueurActuel == "X" ? "O" : "X";

        JoueurActuelTexte.text = $"Tour de {JoueurActuel}";
    }

    // Vérification Match
    private bool VerifierVictoire()
    {
        // Parcourir toutes les combinaisons gagnantes possibles
        for (int i = 0; i < combosGagnant.GetLength(0); i++)
        {
            int pos1 = combosGagnant[i, 0];
            int pos2 = combosGagnant[i, 1];
            int pos3 = combosGagnant[i, 2];

            // Vérifier si les trois cases de la combinaison contiennent le même symbole et ne sont pas vides
            if (!string.IsNullOrEmpty(grilleCases[pos1]) &&
                grilleCases[pos1] == grilleCases[pos2] &&
                grilleCases[pos2] == grilleCases[pos3])
            {
                Transform grilleTransform = grilleActuelle.transform;

                // Suggestion IA : Méthode pour atteindre l'enfant de la case
                ModificationVictoire(grilleTransform.Find(pos1.ToString()));
                ModificationVictoire(grilleTransform.Find(pos2.ToString()));
                ModificationVictoire(grilleTransform.Find(pos3.ToString()));
                // Fin suggestion

                return true;
            }
        }

        return false;
    }

    // Suggestion IA : Méthode pour modifier la couleur de l'enfant
    private void ModificationVictoire(Transform parent)
    {
        Animator[] animators = parent.GetComponentsInChildren<Animator>();

        foreach (Animator animator in animators)
        {
            animator.SetTrigger("Victoire");
        }
    }
    // Fin suggestion

    private bool VerifierMatchNul()
    {
        if (VerifierVictoire())
            return false;

        // Vérifier que toutes les cases sont remplies
        for (int i = 0; i < grilleCases.Length; i++)
        {
            if (string.IsNullOrEmpty(grilleCases[i]))
                return false;
        }

        return true;
    }

    private void FinDePartie(string texteAffichage)
    {
        finPartie = true;

        controlPannel.SetActive(false);
        statusPannel.SetActive(false);

        victoireTexte.text = texteAffichage;
        finPartiePannel.SetActive(true);
    }
}
