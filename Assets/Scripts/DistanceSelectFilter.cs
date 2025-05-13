using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DistanceSelectFilter : MonoBehaviour, IXRSelectFilter
{   
    public bool canProcess => isActiveAndEnabled;

    public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        return (interactor.transform.position - interactable.transform.position).magnitude < .3f;
    }
}
