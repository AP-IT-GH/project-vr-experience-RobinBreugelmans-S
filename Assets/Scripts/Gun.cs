using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{

    public InputActionReference activateL;
    public InputActionReference activateR;
    public NearFarInteractor interactor = null;
    private bool selected = false;

    [SerializeField]
    private Material bulletHoleMaterial;

    private Transform fire;

    void Start()
    {
        interactor.selectEntered.AddListener(OnGrab);
        interactor.selectExited.AddListener(OnExit);

        activateL.action.performed += Use;
        activateR.action.performed += Use;

        fire = transform.GetChild(1).transform;
    }

    private void Use(CallbackContext callback)
    {
        if (!selected)
            return;

        shoot();
    }

    private void shoot()
    {
        //ammmo--;
        RaycastHit hit;
        if (Physics.Raycast(fire.position, transform.forward, out hit, 2048f)) //, maxDistance
        {
            //layer 6 = Target layer
            if (hit.transform.gameObject.layer == 6)
            {
                //give points, etc.
                print("hit target");
            }
            else
            {
                GameObject bulletHole = GameObject.CreatePrimitive(PrimitiveType.Quad);
                Destroy(bulletHole.GetComponent<Collider>());
                bulletHole.transform.rotation = Quaternion.FromToRotation(Vector3.back, hit.normal);
                bulletHole.transform.localScale = new Vector3(.11f, .11f, 1f);
                bulletHole.GetComponent<Renderer>().material = bulletHoleMaterial;
                bulletHole.transform.position = hit.point + hit.normal * .001f;
                Destroy(bulletHole, 90); //90s
            }
        }
    }

    private void OnExit(SelectExitEventArgs arg0)
    {
        selected = false;
    }

    private void OnGrab(SelectEnterEventArgs arg0)
    {
        if (arg0.interactableObject.transform.gameObject == gameObject)
        {
            selected = true;

        }
    }
}
