using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Canon : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider horizontalSlider; // controla posición X
    [SerializeField] private Slider verticalSlider;   // ángulo de disparo
    [SerializeField] private Slider forceSlider;
    [SerializeField] private TMP_Dropdown massDropdown;
    [SerializeField] private Button shootButton;

    [Header("Partes del cañón")]
    [SerializeField] private Transform canonPivot;  // rota en X
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Movimiento horizontal")]
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;

    private float cooldown = 5f;
    private float lastShootTime = -Mathf.Infinity;

    private void Start()
    {
        shootButton.onClick.AddListener(Shoot);
    }

    private void Update()
    {
        // Movimiento horizontal del cañón
        float targetX = Mathf.Lerp(minX, maxX, horizontalSlider.value); // slider 0-1
        Vector3 pos = transform.position;
        pos.x = targetX;
        transform.position = pos;

        // Rotación vertical del pivot (ángulo de disparo)
        float verticalAngle = verticalSlider.value; // rango 0-80
        canonPivot.localRotation = Quaternion.Euler(-verticalAngle, 0, 0);

        // Controlar el estado del botón según cooldown
        if (Time.time - lastShootTime < cooldown)
        {
            shootButton.interactable = false;
        }
        else
        {
            shootButton.interactable = true;
        }
    }

    private void Shoot()
    {
        if (Time.time - lastShootTime < cooldown)
            return;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        float mass = 1f;
        // Configurar masa
        switch (massDropdown.value)
        {
            case 0: mass = 1f; rb.mass = 1f; break;
            case 1: mass = 2f; rb.mass = 2f; break;
            case 2: mass = 5f; rb.mass = 5f; break;
        }

        // Dirección de disparo desde el shootPoint
        Vector3 dir = shootPoint.forward;

        // Aplicar fuerza
        float force = forceSlider.value;
        rb.AddForce(dir * force, ForceMode.Impulse);

        // Pasar parámetros al ProjectileReport si existe
        var report = proj.GetComponent<ProjectileReport>();
        if (report != null)
        {
            // Ángulo vertical usado (en grados)
            report.initialAngle = verticalSlider.value;
            report.forceUsed = force;
            report.massUsed = mass;
        }

        lastShootTime = Time.time;
        shootButton.interactable = false;
    }
}
