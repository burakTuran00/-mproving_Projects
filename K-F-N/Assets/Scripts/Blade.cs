using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
public class Blade : MonoBehaviour
{
    private Camera mainCamera;

    private Collider bladeCollider;

    private GameManager gameManager;

    private bool slicing;

    public Vector3 direction { get; private set; }

    public float slicedForce = 5.0f;

    public float minSliceVelocity = 0.01f;

    private TrailRenderer bladeTrail;

    public Text comboText;

    private void Awake()
    {
        bladeCollider = GetComponent<Collider>();
        mainCamera = Camera.main;
        bladeTrail = GetComponentInChildren<TrailRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        StopSlicing();
    }

    private void OnDisable()
    {
        StopSlicing();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSlicing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopSlicing();
        }
        else if (slicing)
        {
            ContinueSlicing();
        }
    }

    private void StartSlicing()
    {
        Vector3 newPosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0.0f;

        transform.position = newPosition;

        slicing = true;

        bladeCollider.enabled = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear();
    }

    private void StopSlicing()
    {
        slicing = false;
        bladeCollider.enabled = false;
        bladeTrail.enabled = false;
    }

    private void ContinueSlicing()
    {
        Vector3 newPosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0.0f;

        direction = newPosition - transform.position;

        float velocity = direction.magnitude / Time.deltaTime;
        bladeCollider.enabled = velocity > minSliceVelocity;

        transform.position = newPosition;
    }
}
