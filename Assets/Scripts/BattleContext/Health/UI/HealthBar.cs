using TMPro;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private bool _showAmount;

    public Entity Entity;

    public void OnUpdate()
    {
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        var health = em.GetComponentData<Health>(Entity);
        var owner = em.GetComponentData<OwnerTag>(Entity);

        var isEnabled = health.showHealthBar && (owner.PlayerId != LocalPlayer.LocalPlayerIndex || health.currentValue != health.maxValue);
        gameObject.SetActive(isEnabled);

        if (isEnabled == false)
        {
            return;
        }

        _progressBar.fillAmount = health.currentValue / health.maxValue;

        if (_showAmount)
        {
            _amountText.gameObject.SetActive(true);
            _amountText.text = health.currentValue.ToString();
        }
        else
        {
            _amountText.gameObject.SetActive(false);
        }

        var localTransform = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(Entity);
        transform.position = Camera.main.WorldToScreenPoint(localTransform.Position);
    }
}
