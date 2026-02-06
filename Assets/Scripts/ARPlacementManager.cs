// Inspiré des notes de cours d'Environnement Immersif
// Auteur : Frédérik Taleb
// https://envimmersif-cegepvicto.github.io/exercice_adaptation_ar/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField]
    private InputActionReference tapAction;

    [SerializeField]
    private ARRaycastManager aRRaycastManager;

    private int nbObjets = 0;

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
        if (nbObjets > 0) { return; }

        Vector2 positionTap = Mouse.current.position.ReadValue();

        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (aRRaycastManager.Raycast(positionTap, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;
            ARPlane plane = hits[0].trackable as ARPlane;

            Vector3 position = pose.position;

            if (plane.alignment == PlaneAlignment.HorizontalUp)
            {
                position += Vector3.up * 0.25f;

                GameObject nouvelObjet = Instantiate(prefabActuel, position, pose.rotation);

                Renderer renderer = nouvelObjet.GetComponentInChildren<Renderer>();

                nouvelObjet.name = nouvelObjet.name + "_" + nbObjets;

                nbObjets++;
            }
        }
    }
}
