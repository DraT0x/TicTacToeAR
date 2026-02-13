// Inspiré des notes de cours d'Environnement Immersif et de la Démo AR
// Auteur : Frédérik Taleb
// https://envimmersif-cegepvicto.github.io/exercice_adaptation_ar/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ARPlacementManager : MonoBehaviour
{
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

        // Solution Suggéré par l'IA pour le raycast au travers du UI
        if (IsPointerOverUI(positionTap))
        {
            return;
        }
        // Fin de la solution


        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (aRRaycastManager.Raycast(positionTap, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;
            ARPlane plane = hits[0].trackable as ARPlane;

            if (plane.alignment != PlaneAlignment.HorizontalUp) return;

            Vector3 position = pose.position + Vector3.up * 0.25f;

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
                if (!modeRepositionnement)
                {
                    Ray ray;
                    RaycastHit hit;

                    ray = Camera.main.ScreenPointToRay(positionTap);
                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        if (hit.collider.GetComponent<GrilleCase>() != null)
                        {
                            hit.collider.GetComponent<GrilleCase>().OnTapped();
                        }
                    }
                    return;
                }

                if (anchorObject == null) return;
                anchorObject.transform.SetPositionAndRotation(position, pose.rotation);
            }

        }
    }

    // Solution Suggéré par l'IA pour le raycast au travers du UI en conséquence du nouveau Input System
    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null) return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = screenPosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.name == "Canvas") continue; // Ignore le canvas

            // Vérifie si c'est vraiment un élément du UI
            if (result.gameObject.GetComponent<UnityEngine.UI.Selectable>() != null)
            {
                Debug.Log($"Blocked by: {result.gameObject.name}");
                return true;
            }
        }

        return false;
    }
    // Fin de la solution

    public void ModeRepositionnement()
    {
        modeRepositionnement = !modeRepositionnement;

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
