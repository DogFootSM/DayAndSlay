using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float duration;
    private bool startedDuration = false;

    private float _delay;
    private float _duration;
    private bool _startedDuration;

    protected abstract void Tick();
    protected virtual void OnEnable()
    {
        _delay = delay;
        _duration = duration;
        _startedDuration = startedDuration;
    }

    private void Update()
    {
        if (!_startedDuration)
        {
            _delay -= Time.deltaTime;
            if (_delay <= 0f)
            {
                _startedDuration = true;
            }
        }
        else
        {
            _duration -= Time.deltaTime;
            if (_duration <= 0f)
            {
                gameObject.SetActive(false);
            }
        }

        Tick();
    }


}