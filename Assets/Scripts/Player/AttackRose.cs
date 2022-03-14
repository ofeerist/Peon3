using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class AttackRose : MonoBehaviour
    {
        public Image[] Hooks;
        public int Current;
        private Vector3 lastPosition;
        private void Start()
        {
            Observable.EveryUpdate().Where(x => Input.GetMouseButtonDown(0)).Subscribe(x =>
            {
                lastPosition = Input.mousePosition;
            }).AddTo(this);
            
            Observable.EveryUpdate().Where(x => Input.GetMouseButton(0)).Subscribe(x =>
            {
                var currentPosition = Input.mousePosition;
                var direction = (lastPosition - currentPosition).normalized;

                var angle = SignedAngleBetween(direction, Vector3.up, Vector3.forward);

                Current = angle switch
                {
                    < -0 and > -60 => 1,
                    < -60 and > -120 => 2,
                    < -120 and > -180 => 3,
                    > 0 and < 180 => 4,
                    _ => Current
                };

                if (Vector3.Distance(currentPosition, lastPosition) < 50f)
                    Current = 5;

                DisableHooks();
                Hooks[Current - 1].color = Color.red;
            }).AddTo(this);
        }

        private void DisableHooks()
        {
            foreach (var hook in Hooks)
            {
                hook.color = Color.white;
            }
        }

        float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n){
            // angle in [0,180]
            float angle = Vector3.Angle(a,b);
            float sign = Mathf.Sign(Vector3.Dot(n,Vector3.Cross(a,b)));

            // angle in [-179,180]
            float signed_angle = angle * sign;

            // angle in [0,360] (not used but included here for completeness)
            //float angle360 =  (signed_angle + 180) % 360;

            return signed_angle;
        }
    }
}