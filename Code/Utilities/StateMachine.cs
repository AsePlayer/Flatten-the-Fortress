using Sandbox;

namespace Utilities {
	public sealed class StateMachine : Component
	{
		// dictionary of string, int to store the states
		private Dictionary<string, int> states = new Dictionary<string, int>();
		
		// current state
		private string currentState;

		// add a state
		public void AddState(string name, int value)
		{
			states[name] = value;
		}

		// remove a state
		public void RemoveState(string name)
		{
			states.Remove(name);
		}

		// get a state
		public int GetState(string name)
		{
			return states[name];
		}

		// set a state

		public void SetState(string name, int value)
		{
			states[name] = value;
		}

		// change the current state
		public void ChangeState(string name)
		{
			currentState = name;
		}

		// get the current state
		public string GetCurrentState()
		{
			return currentState;
		}

	}
}