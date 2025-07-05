using UnityEngine;
using System.Collections.Generic;

//Can be used as a laser source, not just for laser gun
public class DamagingLaserBeamScript : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float laserMaxLength = 100f;
    public int laserMaxBounces = 10;
    public int damageAmount = 1;
    public float damageCooldown = 0.1f; // Защита от частого урона
    
    private float lastDamageTime = -100f;

    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    void Update()
    {
        var linePoints = new List<Vector3>();
        linePoints.Add(Vector3.zero);

        Vector2 currentPosition = transform.position;
        Vector2 currentDirection = transform.right;
        float remainingLength = laserMaxLength;

        for (int i = 0; i < laserMaxBounces; i++)
        {
            const float raycastOffset = 0.001f;
            RaycastHit2D hit = Physics2D.Raycast(currentPosition + currentDirection * raycastOffset, currentDirection, remainingLength);

            if (hit.collider != null)
            {
                if (hit.collider.isTrigger)
                {
                    remainingLength -= hit.distance;
                    currentPosition = hit.point;
                    continue;
                }
                linePoints.Add(transform.InverseTransformPoint(hit.point));
                remainingLength -= hit.distance;
                
                // Проверяем, попал ли лазер в игрока
                PlayerTag player = hit.collider.GetComponent<PlayerTag>();
                if (player != null && Time.time - lastDamageTime >= damageCooldown)
                {
                    if (InventoryScript.Instance != null)
                    {
                        InventoryScript.Instance.TakeDamage(damageAmount);
                        lastDamageTime = Time.time;
                    }
                }
                
                //Added a tag to activate objects with laser
                LaserInteractable interactable = hit.collider.GetComponent<LaserInteractable>();
                if (interactable != null)
                {
                    interactable.Activate();
                }

                currentPosition = hit.point;
                if (hit.collider.GetComponent<Reflective>() != null)
                {
                    currentDirection = Vector2.Reflect(currentDirection, hit.normal);
                }
                else
                {
                    break;
                }
            }
            else
            {
                Vector3 endPoint = transform.InverseTransformPoint(currentPosition + currentDirection * remainingLength);
                linePoints.Add(endPoint);
                break;
            }
        }

        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
    }
} 