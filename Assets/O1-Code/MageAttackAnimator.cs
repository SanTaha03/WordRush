using UnityEngine;

public class MageAttackAnimator : MonoBehaviour
{
    [System.Serializable]
    public class AttackAnimation
    {
        public string stateName;
        public float projectileDelay = 0.35f;
    }
    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Attack animations")]
    [SerializeField] private AttackAnimation[] attackAnimations =
    {
        new AttackAnimation { stateName = "HumanM@MagicAttackDirect1H01_L", projectileDelay = 0.35f },
        new AttackAnimation { stateName = "HumanM@MagicAttackDirect1H01_R", projectileDelay = 0.35f },
        new AttackAnimation { stateName = "HumanM@MagicAttackDirect2H01", projectileDelay = 0.65f }
    };

    [Header("Settings")]
    [SerializeField] private float transitionDuration = 0.05f;

    private int currentAttackIndex = 0;

    public float PlayNextAttack()
    {
        if (animator == null)
        {
            Debug.LogWarning("MageAttackAnimator : aucun Animator assigné.");
            return 0.35f;
        }

        if (attackAnimations == null || attackAnimations.Length == 0)
        {
            Debug.LogWarning("MageAttackAnimator : aucune animation d'attaque renseignée.");
            return 0.35f;
        }

        AttackAnimation currentAttack = attackAnimations[currentAttackIndex];        float delay = attackAnimations[currentAttackIndex].projectileDelay;

        animator.CrossFadeInFixedTime(currentAttack.stateName, transitionDuration);

        currentAttackIndex++;

        if (currentAttackIndex >= attackAnimations.Length)
        {
            currentAttackIndex = 0;
        }

        return delay;
    }
}