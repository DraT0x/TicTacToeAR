// Inspiré des notes de cours d'Environnement Immersif
// Auteur : Frédérik Taleb
// https://envimmersif-cegepvicto.github.io/exercice_adaptation_ar/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ARPlacementManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PrefabXSymbol;
    [SerializeField]
    private GameObject PrefabOSymbol;
    [SerializeField]
    private GameObject PrefabGrille;
    private GameObject prefabActuel;

    private GameObject instanceGrille;

    // Gestion de la grille / Repositionnement
    private ARAnchor grilleAnchor;
    private GameObject anchorObject;

    [SerializeField]
    private Image boutonRepositionnement;
    private bool modeRepositionnement;

    [SerializeField]
    private InputActionReference tapAction;

    [SerializeField]
    private ARRaycastManager aRRaycastManager;

    private void Start()
    {
        prefabActuel = PrefabGrille;
    }

    private void OnEnable()
    {
        tapAction.action.canceled += Tap_canceled;
        tapAction.action.Enable();
    }

    private void OnDisable()
    {
        tapAction.action.canceled -= Tap_canceled;
        tapAction.action.Disable();
    }

    private void Tap_canceled(InputAction.CallbackContext ctx)
    {
        Vector2 positionTap = Mouse.current.position.ReadValue();

        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (aRRaycastManager.Raycast(positionTap, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;
            ARPlane plane = hits[0].trackable as ARPlane;

            if (plane.alignment != PlaneAlignment.HorizontalUp) return;

            Vector3 position = pose.position + Vector3.up * 0.25f;


            // Solution Suggéré par l'IA pour le repositionnement 
            if (instanceGrille == null)
            {
                anchorObject = new GameObject("GrilleAnchor");
                grilleAnchor = anchorObject.AddComponent<ARAnchor>();

                anchorObject.transform.SetPositionAndRotation(position, pose.rotation);

                instanceGrille = Instantiate(prefabActuel, position, pose.rotation);
                instanceGrille.transform.SetParent(anchorObject.transform);
                GameController.Instance.NewGame(instanceGrille);
            }
            else
            {
                if (!modeRepositionnement) return;

                anchorObject.transform.SetPositionAndRotation(position, pose.rotation);
            }

        }
    }

    public void ModeRepositionnement()
    {
        modeRepositionnement = !modeRepositionnement;
        Debug.Log(modeRepositionnement);

        if (modeRepositionnement)
        {
            boutonRepositionnement.color = Color.green;
        }
        else
        {
            boutonRepositionnement.color = Color.red;
        }
    }
}
