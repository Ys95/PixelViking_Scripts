using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform interactPoint;
    [SerializeField] private float interactionRange;
    [SerializeField] private LayerMask targetLayers;

    public void InteractWithObject()
    {
        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(interactPoint.position, interactionRange, targetLayers);

        foreach (Collider2D target in targetsHit)
        {
            Debug.Log("In interaction range: " + target.name);
            IInteractable interactableTarget = target.GetComponent<IInteractable>();

            if (interactableTarget != null)
            {
                interactableTarget.Interact(this.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (interactPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(interactPoint.position, interactionRange);
    }
}