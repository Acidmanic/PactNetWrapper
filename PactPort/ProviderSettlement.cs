using System;
using System.Collections.Generic;
using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.PactPort
{
    public class ProviderSettlement
    {
        private readonly Dictionary<string, Action> _settleActions;

        public ProviderSettlement(Dictionary<string, Action> settleActions)
        {
            _settleActions = settleActions;
        }

        public void PrepareProvider(Interaction interaction)
        {
            var state = interaction.ProviderState;

            foreach (var stateAction in _settleActions)
            {
                if (state.Matches(stateAction.Key))
                {
                    try
                    {
                        stateAction.Value.Invoke();
                    }
                    catch (Exception e){ }
                }
            }
        }
    }
}