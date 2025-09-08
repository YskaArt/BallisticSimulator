using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Canon : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider horizontalSlider; // controla posici�n X
    [SerializeField] private Slider verticalSlider;   // �ngulo de disparo
    [SerializeField] private Slider forceSlider;
    [SerializeField] private TMP_Dropdown massDropdown;
    [SerializeField] private Button shootButton;

    [Header("Partes del ca��n")]
    [SerializeField] private Transform canonPivot;  // rota en X
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Movimiento horizontal")]
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;

    private void Start()
    {
        shootButton.onClick.AddListener(Shoot);
    }

    private void Update()
    {
        // Movimiento horizontal del ca��n
        float targetX = Mathf.Lerp(minX, maxX, horizontalSlider.value); // slider 0-1
        Vector3 pos = transform.position;
        pos.x = targetX;
        transform.position = pos;

        // Rotaci�n vertical del pivot (�ngulo de disparo)
        float verticalAngle = verticalSlider.value; // rango 0-80
        canonPivot.localRotation = Quaternion.Euler(-verticalAngle, 0, 0);
    }

    private void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        // Configurar masa
        switch (massDropdown.value)
        {
            case 0: rb.mass = 1f; break;
            case 1: rb.mass = 2f; break;
            case 2: rb.mass = 5f; break;
        }

        // Direcci�n de disparo desde el shootPoint
        Vector3 dir = shootPoint.forward;

        // Aplicar fuerza
        float force = forceSlider.value;
        rb.AddForce(dir * force, ForceMode.Impulse);
    }
}
