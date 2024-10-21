using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public struct DebugSettings
    {
        public bool debug;
        public bool printEnterState;
        public bool printLeaveState;
    }

    public class StateMachine<TContext> : MonoBehaviour where TContext : new()
    {
        public TContext context;
        public float timeSinceLastOnEnter => Time.time - activationTime;

        private float activationTime = 0.0f;
        private Dictionary<Type, State> stateInstanceMap = new Dictionary<Type, State>();
        protected State activeState { get; private set; }
        protected State previousState { get; private set; }
        public Type nextStateType { get; private set; }

        public State PreviousState => previousState;

        [Header("Debug")]
        public DebugSettings debugSettings;
        public string debugText = "";

        public bool DebugActive => debugSettings.debug;
        public bool DebugEnter => debugSettings.debug && debugSettings.printEnterState;
        public bool DebugLeave => debugSettings.debug && debugSettings.printLeaveState;

        private Dictionary<Type, int> stateColorIndex = new Dictionary<Type, int>();
        private readonly string[] richTextColors = new string[]
        {
            "black",
            "blue",
            "olive",
            "red",
            "cyan",
            "green",
            "grey",
            "lightblue",
            "lime",
            "magenta",
            "maroon",
            "navy",
            "orange",
            "purple",
            "silver",
            "teal",
        };

        public void ActivateState<TState>() where TState : State, new()
        {
            ActivateState(GetStateInstance<TState>());
        }

        public TState GetStateInstance<TState>() where TState : State, new()
        {
            Type behaviourType = typeof(TState);

            if (stateInstanceMap.ContainsKey(behaviourType))
                return stateInstanceMap[behaviourType] as TState;

            TState state = new TState();
            stateInstanceMap.Add(behaviourType, state);
            return state;
        }

        public void ActivatePreviousState()
        {
            if (previousState == null)
                return;

            ActivateState(previousState);
        }

        public void Stop()
        {
            DeactivateState(activeState);
        }

        private void DeactivateState(State state)
        {
            if (state != null && DebugLeave)
                PrintStateChangeToConsole(state, false);
            if (state == null)
                return;

            if (state is EnumeratorState enumeratorState)
                StopCoroutine(enumeratorState.Enumerator());

            state?.OnLeave();
        }

        private void ActivateState(State state)
        {
            if (state != null && !stateColorIndex.ContainsKey(state.GetType()))
            {
                int count = stateColorIndex.Count;
                stateColorIndex.Add(state.GetType(), count);
            }
            nextStateType = state.GetType();
            DeactivateState(activeState);
            previousState = activeState;
            activeState = null;

            state.context = context;
            state.machine = this;

            activationTime = Time.time;
            state.OnEnter();

            if (state != null && DebugEnter)
                PrintStateChangeToConsole(state, true);
            if (state is EnumeratorState newEnumeratorState)
                StartCoroutine(newEnumeratorState.Enumerator());

            activeState = state;
        }

        protected virtual void PreUpdate()
        {

        }

        protected virtual void Update()
        {
            PreUpdate();
            (activeState as UpdateState)?.OnUpdate(Time.deltaTime);
        }

        protected virtual void FixedUpdate()
        {
            (activeState as UpdateState)?.OnFixedUpdate(Time.fixedDeltaTime);
        }

        protected virtual void LateUpdate()
        {
            (activeState as UpdateState)?.OnLateUpdate(Time.deltaTime);
        }

        protected virtual void OnDrawGizmos()
        {
            if (!DebugActive)
                return;

            /*if (activeState != null)
            {
                Shapes.Draw.Text(transform.position + Vector3.up * 1.5f, $"{activeState.GetType().Name}\n{debugText}", 2.0f, Color.white);
            }*/
            activeState?.OnDrawGizmos();
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (!DebugActive)
                return;

            activeState?.OnDrawGizmosSelected();
        }

        public class State
        {
            public TContext context;
            public StateMachine<TContext> machine;
            public float time => machine.timeSinceLastOnEnter;

            public virtual void OnEnter() { }
            public virtual void OnLeave() { }

            public virtual void OnDrawGizmos() { }
            public virtual void OnDrawGizmosSelected() { }
        }

        public class UpdateState : State
        {
            public virtual void OnUpdate(float deltaTime) { }
            public virtual void OnLateUpdate(float fixedDeltaTime) { }
            public virtual void OnFixedUpdate(float deltaTime) { }
        }

        public class EnumeratorState : UpdateState
        {
            public virtual IEnumerator Enumerator() { yield break; }
        }

        private void PrintStateChangeToConsole(State state, bool isEnteringState)
        {
            if (state == null)
                return;

            int colorIndex = stateColorIndex[state.GetType()];
            string colorString = richTextColors[colorIndex];

            string enterOrLeaveString;
            if (state is EnumeratorState)
                enterOrLeaveString = isEnteringState ? "StartCoroutine" : "StopCoroutine";
            else
                enterOrLeaveString = isEnteringState ? "Entered" : "Left";

            string log = $"<color={colorString}>{enterOrLeaveString}</color> [{state.GetType().Name}]";
            Debug.Log(log);
        }
    }
}
