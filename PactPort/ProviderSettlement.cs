using System;
using System.Collections.Generic;
using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.PactPort
{
    public class ProviderSettlement
    {
        private readonly Dictionary<string, Action<PactRequest>> _settleActions;
        private readonly Action<Exception> _exceptionListener;
        public ProviderSettlement(Dictionary<string, Action<PactRequest>> settleActions, Action<Exception> exceptionListener)
        {
            _settleActions = settleActions;
            _exceptionListener = exceptionListener;
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
                        stateAction.Value.Invoke(interaction.Request);
                    }
                    catch (Exception e)
                    {
                        _exceptionListener.Invoke(e);
                    }
                }
            }
        }
    }
}