using UnityEngine;
using System.Collections;

/// <summary>
/// Camera shaker for juicy effects
/// </summary>
public class CameraShaker : MonoBehaviour
{
  public static bool DisableTotally = false;

  private static CameraShaker instance;

  const float maxDuration = 2f;
  const float maxForce = 1f;

  [Tooltip("Factor for each force if you want to enchance/reduce the effect globally")]
  public float factor = 1.75f;
  public bool resetToOriginalPosition = false;

  private Vector2 previousMouvement = Vector2.zero;
  private Vector3 originalPos;

  private float startForce, force;
  private float startTime, time;

  private bool shakeEnabled = true;

  void Start()
  {
    instance = this;

    originalPos = Camera.main.transform.localPosition;
  }

  void AddShake(float newForce, float newDuration, bool cumulative)
  {
    //Logger.Log("SHAKE SHAKE SHAKE " + newForce + " " + newDuration);

    if (cumulative)
    {
      time += newDuration;
      force += newForce * factor;
    }
    else
    {
      time = Mathf.Max(newDuration, time);
      force = Mathf.Max(newForce * factor, force);
    }

    time = Mathf.Clamp(maxDuration, 0f, time);
    force = Mathf.Clamp(maxForce, 0f, force);

    startTime = time;
    startForce = force;
  }

  void Update()
  {
    if (time >= 0f && shakeEnabled)
    {
      //time -= Time.unscaledDeltaTime;
      time -= Time.deltaTime;

      // Cancel previous movement
      AddPositionToCamera(-previousMouvement);

      if (time > 0f)
      {
        force = Mathf.Lerp(startForce, 0f, (startTime - time) / startTime);

        // Slightly move the camera aroung origin
        Vector2 mouvement = Random.insideUnitSphere * force;
        AddPositionToCamera(mouvement);

        previousMouvement = mouvement;
      }
      else
      {
        time = 0f;
        startTime = 0f;
        force = 0f;
        startForce = 0f;

        // Reset only if no other shake are running!
        if (resetToOriginalPosition)
        {
          Camera.main.transform.localPosition = originalPos;
        }
      }
    }
  }

  void AddPositionToCamera(Vector2 pos)
  {
    Camera.main.transform.position = new Vector3(
      Camera.main.transform.position.x + pos.x,
      Camera.main.transform.position.y + pos.y,
      Camera.main.transform.position.z
      );
  }

  /// <summary>
  /// Camera shake
  /// </summary>
  /// <param name="force"></param>
  /// <param name="duration"></param>
  public static void ShakeCamera(float force, float duration)
  {
    ShakeCamera(force, duration, false);
  }

  /// <summary>
  /// Initialize and start the camera shaking
  /// </summary>
  /// <param name="force">Force, between 0 and 1.</param>
  /// <param name="duration">Duration in seconds.</param>
  /// <param name="resetToZero">Reset camera to 0 after shake</param>
  public static void ShakeCamera(float force, float duration, bool cumulative)
  {
    // Create a new game object containing the shaking script
    Initialize();

    if (instance.shakeEnabled)
    {
      instance.AddShake(force, duration, cumulative);
    }
  }

  public static void Initialize()
  {
    if (instance == null)
    {
      GameObject shaker = new GameObject("CameraShake");
      shaker.transform.parent = Camera.main.transform;
      instance = shaker.AddComponent<CameraShaker>();
    }
  }

  public static void Disable()
  {
    if (instance != null) instance.shakeEnabled = false;
  }

  public static void Enable()
  {
    if (instance != null) instance.shakeEnabled = !DisableTotally;
  }

  public static void Clean ()
  {
    if (instance != null) 
    {
      instance.time = 0f;
      instance.startTime = 0f;
      instance.force = 0f;
      instance.startForce = 0f;
      instance.previousMouvement = Vector2.zero;
    }
  }
}
