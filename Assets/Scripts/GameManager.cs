using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
public class GameManager : MonoBehaviour
{
    const float COMBO_TIME = 1f;
    const float COMBO_SCALE = 0.25f;
    const float HIT_STOP_TIME = 0.02f;
    const float HIT_STOP_SCALE = 0.05f;

    public static GameManager Instance;

    private float _comboTime = 0;
    private int _score = 0;

    public int Score => _score;

    [SerializeField] private TextMeshProUGUI _scoreText;

    private PostProcessVolume _ppv;
    private ColorGrading _cd;
    private LensDistortion _ld;
    private ChromaticAberration _ca;
    private Vignette _vi;

    void Awake()
    {
        Instance = this;
        _ppv = GetComponent<PostProcessVolume>();
        _ppv.profile.TryGetSettings<ColorGrading>(out _cd);
        _ppv.profile.TryGetSettings<LensDistortion>(out _ld);
        _ppv.profile.TryGetSettings<ChromaticAberration>(out _ca);
        _ppv.profile.TryGetSettings<Vignette>(out _vi);
    }

    private void Update()
    {
        _cd.contrast.value = 5 * Mathf.Sqrt(_comboTime / COMBO_TIME);
        _cd.saturation.value = -20 * Mathf.Sqrt(_comboTime / COMBO_TIME);
        _cd.temperature.value = -10 * Mathf.Sqrt(_comboTime / COMBO_TIME);
        _ld.intensity.value = -10 * Mathf.Sqrt(_comboTime / COMBO_TIME);
        _vi.intensity.value = 0.5f * Mathf.Sqrt(_comboTime / COMBO_TIME);
        _ca.enabled.value = (_comboTime > 0);
        if (_comboTime > 0)
        {
            if (_comboTime > COMBO_TIME)
                Time.timeScale = HIT_STOP_SCALE;
            else
                Time.timeScale = COMBO_SCALE + (1 - COMBO_SCALE) * (COMBO_TIME - _comboTime);
        }
        else
        {
            Time.timeScale = 1;
        }
        _comboTime -= Time.unscaledDeltaTime;
        _comboTime = Mathf.Max(0, _comboTime);

        transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, -10), Time.unscaledDeltaTime);
    }

    public void HitStop()
    {
        transform.position += (Vector3)Random.insideUnitCircle.normalized * 0.1f;
        _comboTime = COMBO_TIME + HIT_STOP_TIME;
    }

    public void AddScore()
    {
        _score++;
        _scoreText.text = "Score " + _score;
    }
}
