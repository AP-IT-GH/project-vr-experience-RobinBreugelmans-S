using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{

    public InputActionReference activate;
    public NearFarInteractor interactor = null;
    private bool selected = false;

    [SerializeField]
    private Material bulletHoleMaterial;

    void Start()
    {
        /*
        interactor.selectEntered.AddListener(OnSelect);
        interactor.selectExited.AddListener(OnExit);

        activate.action.performed += Use;
        */
    }
    private void Use(CallbackContext callback)
    {
        if (!selected)
            return;

        print("USE PRESSED");

        //shoot bullet
        shoot();
    }

    /* //test because XR is broken
    int a = 0;
    private void FixedUpdate()
    {
        if (a++ % 30 == 0)
        {
            shoot();
        }
    }*/

    private void shoot()
    {
        //ammmo--;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.0477f, 0.1637f), transform.forward, out hit, 2048f)) //, maxDistance
        {
            //layer 6 = Target layer
            if (hit.transform.gameObject.layer == 6)
            {
                //give points, etc.
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

    private void OnSelect(SelectEnterEventArgs arg0)
    {
        print(arg0.interactableObject.transform.gameObject.name);

        if (arg0.interactableObject.transform.gameObject == gameObject)
        {
            selected = true;
        }
    }
}
