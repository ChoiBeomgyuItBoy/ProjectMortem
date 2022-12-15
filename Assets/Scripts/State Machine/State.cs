using UnityEngine;

namespace Mortem.StateMachine
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick(float deltaTime);
        public abstract void Exit();

        private Animator animator;

        protected readonly string[] impactAnimations = 
        {
            "Character_Impact_1", "Character_Impact_2", "Character_Impact_3"
        };

        protected void SetAnimator(Animator animator)
        {
            this.animator = animator;
        }

        protected void PlayAnimationSmoothly(string name)
        {
            int hash = Animator.StringToHash(name);

            animator.CrossFadeInFixedTime(hash, 0.1f);
        }

        protected void SetAnimationBlendTree(string parameter, float value, float deltaTime)
        {
            int hash = Animator.StringToHash(parameter);

            animator.SetFloat(hash, value, 0.1f, deltaTime);
        }

        protected float GetNormalizedTime(string animationTag)
        {
            AnimatorStateInfo currentAnimationInfo = animator.GetCurrentAnimatorStateInfo(0);

            if(!animator.IsInTransition(0) && currentAnimationInfo.IsTag(animationTag))
            {
                return currentAnimationInfo.normalizedTime;
            }

            return 0f;
        }

        protected void PlayRandomAnimation(string[] animationsArray)
        {
            int randomIndex = Random.Range(0, animationsArray.Length);

            for (int i = 0; i < animationsArray.Length; i++)
            {
                PlayAnimationSmoothly(animationsArray[randomIndex]);
            }
        }
    }
}
