using System;
using UniRx;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public Animator Animator;
        public AttackRose Rose;
        public PlayerMovement Movement;
        
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int AttackID = Animator.StringToHash("AttackID");
        private readonly SerialDisposable serialDisposable = new SerialDisposable();

        private void Start()
        {
            Observable.EveryUpdate().Where(x => Input.GetMouseButtonUp(0)).Subscribe(x =>
            {
                Animator.SetInteger(AttackID, Rose.Current);
                Animator.SetTrigger(Attack);
                
                serialDisposable.Disposable = Observable.Timer(TimeSpan.FromSeconds(.7f)).Subscribe(x =>
                {
                    Movement.TransitionToState(CharacterState.Default);
                });
            }).AddTo(this);
            
            Observable.EveryUpdate().Where(x => Input.GetMouseButtonDown(0)).Subscribe(x =>
            {
                print("boom");
                Movement.TransitionToState(CharacterState.Attacking);
            }).AddTo(this);

            serialDisposable.AddTo(this);
        }
    }
}