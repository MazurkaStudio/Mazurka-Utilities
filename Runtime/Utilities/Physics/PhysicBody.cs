using UnityEngine;

namespace TheMazurkaStudio.Utilities.Physics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicBody : MonoBehaviour
    {
        [field: Range(0f,70f)] [field: SerializeField] public float MaxAcceleration { get; private set; } = 25f;

        public Rigidbody2D Body { get; private set; }

        private void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
            Body.gravityScale = 0f;
            Body.drag = 0f;
            Body.angularDrag = 0f;
        }

        public float Mass => Body.mass;
        public Vector2 Velocity => Body.velocity;
        public Vector2 VelocityDirection => Velocity.normalized;
        public Vector2 Position => Body.position;
        public Vector3 TransformPosition => transform.position;
        private Vector2 currentGoalVel = Vector2.zero;
        public float CurrentSpeed => Velocity.magnitude;

        private bool moveThisFrame;

        private Vector2 forces;
        
        public void Move(Vector2 targetVel, float acceleration)
        {
            moveThisFrame = true;
            currentGoalVel = Vector2.MoveTowards(currentGoalVel, targetVel, acceleration * Time.fixedDeltaTime);
            forces += currentGoalVel;
        }

        public void Drag(float dragAcceleration, float dragFactor = 1f)
        {
            var d = Mathf.Min(Mathf.Abs(Velocity.magnitude), Mathf.Abs(dragFactor));
            Body.AddForce(-Velocity.normalized * d * Mass, ForceMode2D.Impulse);
            
            return;
            var drag = (Velocity * Velocity).magnitude;
            var needAcceleration = (-VelocityDirection * drag * dragFactor) / Time.fixedDeltaTime;
            needAcceleration = Vector2.ClampMagnitude(needAcceleration, dragAcceleration);
            Body.AddForce(needAcceleration * Mass, ForceMode2D.Force);
        }
        
        public void AddInstantForce(Vector2 force, bool ignoreMass, float massMultiplier  = 1f)
        {
            Body.AddForce(force * (ignoreMass ? Body.mass * massMultiplier : 1f), ForceMode2D.Impulse);
        }
        
        private void FixedUpdate()
        {
            if (overrideVelocity)
            {
                Body.velocity = overrideVelocityValue;
                overrideVelocity = false;
                return;
            }

            if (moveThisFrame)
            {
                var needAcceleration = (forces - Velocity) / Time.fixedDeltaTime;
                needAcceleration = Vector2.ClampMagnitude(needAcceleration, MaxAcceleration);
                Body.AddForce(needAcceleration * Mass, ForceMode2D.Force);
            }

            forces = Vector2.zero;
            moveThisFrame = false;
        }

        public void AddExternalForce(Vector2 targetForce)
        {
            forces += targetForce;
        }


        private bool overrideVelocity;
        private Vector2 overrideVelocityValue;
        
        public void OverrideVelocity(Vector2 vel)
        {
            overrideVelocity = true;
            overrideVelocityValue = vel;
        }

        public void Freeze()
        {
            Body.velocity = Vector2.zero;
            currentGoalVel = Vector2.zero;
            moveThisFrame = false;
        }
    }
}
