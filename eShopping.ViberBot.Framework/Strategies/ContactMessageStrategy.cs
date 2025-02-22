﻿using eShopping.ViberBot.Framework.Contexts;
using eShopping.ViberBot.Framework.Attributes;
using System.Reflection;

namespace eShopping.ViberBot.Framework.Strategies
{
    internal class ContactMessageStrategy : IStrategy
    {
        private readonly string? _action;
        private readonly string[] _parameters;
        private readonly string? _activeAction;
        private readonly string[] _activeParameters;

        public ContactMessageStrategy(string? action, string[] parameters, string? activeAction, string[] activeParameters)
        {
            _action = action;
            _parameters = parameters;
            _activeAction = activeAction;
            _activeParameters = activeParameters;
        }

        public object?[] GetParameters(MethodInfo method, Callback callback)
        {
            var context = new ContactMessageContext(callback);
            var parameters = ReflectionUtilities.MatchParameters(method, context, _action != null ? _parameters : _activeParameters);
            return parameters;
        }

        public MethodInfo? PickControllerMethod()
        {
            var method = ReflectionUtilities.FindControllerMethod<ContactMessageAttribute>(attribute
                =>
            {
                if (_action != null)
                {
                    return attribute.Action == _action;
                }
                else
                {
                    return attribute.ActiveAction == _activeAction;
                }
            });
            return method;
        }
    }
}
