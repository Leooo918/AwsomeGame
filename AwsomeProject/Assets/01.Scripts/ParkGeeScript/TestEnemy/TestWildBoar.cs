using System.Collections;
using UnityEngine;

public class TestWildBoar : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _checkPlayer;
    private float _distance = 5f;
    private float _dashtime = 0.3f;
    private float _dashSpeed = 30f;
    private float _dashDelay = 3f;
    private bool isDashing = false;
    private bool isCoroutineRunning = false;
    private Vector3 dashDirection;
    private float dashStartTime;

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);

        if (distanceToPlayer < _distance && !isDashing && !isCoroutineRunning)
        {
            StartCoroutine(DelayDash());
        }

        if (isDashing)
        {
            float elapsedTime = Time.time - dashStartTime;
            float distanceToMove = _dashSpeed * Time.deltaTime;
            transform.position += dashDirection * distanceToMove;

            if (elapsedTime >= _dashtime)
            {
                isDashing = false;
                _checkPlayer.SetActive(false);
            }
        }
    }

    private IEnumerator DelayDash()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(_dashDelay - 0.5f);
        _checkPlayer.SetActive(true);
        yield return new WaitForSeconds(1f);

        dashDirection = (_player.transform.position - transform.position).normalized;
        dashDirection = new Vector3(dashDirection.x, 0, dashDirection.z);
        dashStartTime = Time.time;
        isDashing = true;
        isCoroutineRunning = false;
    }
}
