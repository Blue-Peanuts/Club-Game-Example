using UnityEngine;

public class GameManager : MonoBehaviour
{
    const float HIT_STOP_TIME = 0.03f;
    const float HIT_STOP_SCALE = 0.1f;

    public static GameManager Instance;

    private float _hitStopTime = 0;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_hitStopTime > 0)
            Time.timeScale = HIT_STOP_SCALE;
        else
            Time.timeScale = 1;

        _hitStopTime -= Time.unscaledDeltaTime;
    }

    public void HitStop()
    {
        _hitStopTime = HIT_STOP_TIME;
    }
}
