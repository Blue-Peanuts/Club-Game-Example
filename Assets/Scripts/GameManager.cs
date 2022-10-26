using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    const float COMBO_TIME = 1f;
    const float COMBO_SCALE = 0.25f;
    const float HIT_STOP_TIME = 0.02f;
    const float HIT_STOP_SCALE = 0.02f;

    public static GameManager Instance;

    private float _comboTime = 0;
    private int _score = 0;

    public int Score => _score;

    [SerializeField] private TextMeshProUGUI _scoreText;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_comboTime > COMBO_TIME)
            Time.timeScale = HIT_STOP_SCALE;
        else if (_comboTime > 0)
            Time.timeScale = COMBO_SCALE + (1 - COMBO_SCALE) * (COMBO_TIME - _comboTime);
        else
            Time.timeScale = 1;
        _comboTime -= Time.unscaledDeltaTime;
    }

    public void HitStop()
    {
        _comboTime = COMBO_TIME + HIT_STOP_TIME;
    }

    public void AddScore()
    {
        _score++;
        _scoreText.text = "Score " + _score;
    }
}
