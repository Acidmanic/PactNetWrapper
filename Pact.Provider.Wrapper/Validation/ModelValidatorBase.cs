using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Pact.Provider.Wrapper.Validation
{
    public abstract class ModelValidatorBase<T>
    {
        private readonly List<Func<T, bool>> _terms = new List<Func<T, bool>>();
        /// <summary>
        /// Adds a delegate which validates the object against some rule
        /// </summary>
        /// <param name="term"></param>
        protected void AddValidationTerm(Func<T, bool> term)
        {
            this._terms.Add(term);
        }
        /// <summary>
        /// First argument will pick a property of the object to be validated separately using the second
        /// argument which is a ModelValidator of the type of picked property.  
        /// </summary>
        /// <param name="propPicker">Expression to select a property from object being validated</param>
        /// <param name="propertyValidator">A ModelValidator object to validate picked property</param>
        /// <typeparam name="TProp"></typeparam>
        protected void AddValidationTerm<TProp>(Expression<Func<T, TProp>> propPicker,
            ModelValidatorBase<TProp> propertyValidator)
        {
            this._terms.Add(value => propertyValidator.Validate(propPicker.Compile().Invoke(value)));
        }
        /// <summary>
        /// First Argument, picks a property of the main object which is a collection type. The collection will be
        /// validated per each element, using the second argument, a ModelValidator for elements of the collection. 
        /// </summary>
        /// <param name="propPicker">Expression for selecting a Collection property.</param>
        /// <param name="propertyValidator">A ModelValidator object to validate each collection element.</param>
        /// <typeparam name="TProp"></typeparam>
        protected void AddValidationTerm<TProp>(Expression<Func<T, ICollection<TProp>>> propPicker,
            ModelValidatorBase<TProp> propertyValidator)
        {
            bool Term(T value)
            {
                if (value == null)
                {
                    return false;
                }
                var list = propPicker.Compile().Invoke(value);

                foreach (var prop in list)
                {
                    if (!propertyValidator.Validate(prop))
                    {
                        return false;
                    }
                }

                return true;
            }
            this._terms.Add(Term);
        }

        public bool Validate(T value)
        {
            foreach (var term in _terms)
            {
                if (!term(value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}