//-----------------------------------------------------------------------
// <copyright file="EffortProjectValidation.cs" company="Lifeprojects.de">
//     Class: EffortProjectValidation
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.07.2021</date>
//
// <summary>
// Static Validation Class zur Validierung von Feldinhalten für 
// die Invention per ViewModel
// </summary>
//-----------------------------------------------------------------------

namespace Solarertrag.Core
{
    using System;
    using System.Linq.Expressions;

    using EasyPrototypingNET.LinqExpressions;
    using EasyPrototypingNET.Pattern;

    public class SolarErtragValidation<TViewModel> where TViewModel : class
    {
        private static SolarErtragValidation<TViewModel> validation;

        private TViewModel ThisObject { get; set; }

        public static SolarErtragValidation<TViewModel> This(TViewModel thisObject)
        {
            validation = new SolarErtragValidation<TViewModel>();
            validation.ThisObject = thisObject;
            return validation;
        }

        public Result<string> NotEmpty(Expression<Func<TViewModel, object>> expression, string message)
        {
            string result = string.Empty;
            bool resultValidError = false;
            string propertyName = ExpressionPropertyName.For<TViewModel>(expression);
            string propertyValue = (string)validation.ThisObject.GetType().GetProperty(propertyName).GetValue(validation.ThisObject);

            if (string.IsNullOrEmpty(propertyValue) == true)
            {
                result = $"Das Feld '{message}' darf nicht leer sein.";
                resultValidError = true;
            }

            return Result<string>.SuccessResult(result, resultValidError);
        }

        public Result<string> InRangeYear(Expression<Func<TViewModel, object>> expression, int min, int max)
        {
            string result = string.Empty;
            bool resultValidError = false;
            string propertyName = ExpressionPropertyName.For<TViewModel>(expression);
            object propertyValue = (object)validation.ThisObject.GetType().GetProperty(propertyName).GetValue(validation.ThisObject, null);

            if (propertyValue == null)
            {
                return Result<string>.SuccessResult(result, resultValidError);
            }

            if (propertyValue.IsEmpty() == false)
            {
                if (propertyValue.ToInt().InRange(min, max) == false)
                {
                    result = $"Das Jahr muß zwischen {min} und {max} liegen";
                    resultValidError = true;
                }
            }
            else
            {
                result = $"Das Feld 'Jahr' darf nicht leer sein.";
                resultValidError = true;
            }

            return Result<string>.SuccessResult(result, resultValidError);
        }

        public Result<string> InRangeMonth(Expression<Func<TViewModel, object>> expression, int min, int max, string description = "Ertragsmonat")
        {
            string result = string.Empty;
            bool resultValidError = false;
            string propertyName = ExpressionPropertyName.For<TViewModel>(expression);
            object propertyValue = (object)validation.ThisObject.GetType().GetProperty(propertyName).GetValue(validation.ThisObject, null);

            if (propertyValue == null)
            {
                return Result<string>.SuccessResult(result, resultValidError);
            }

            if (propertyValue.IsEmpty() == false)
            {
                if (propertyValue.ToInt().InRange(min, max) == false)
                {
                    result = $"Der {description} muß zwischen {min} und {max} liegen";
                    resultValidError = true;
                }
            }
            else
            {
                result = $"Das Feld '{description}' darf nicht leer sein.";
                resultValidError = true;
            }

            return Result<string>.SuccessResult(result, resultValidError);
        }

        public Result<string> GreaterThanZero(Expression<Func<TViewModel, object>> expression, string description = "Ertrag")
        {
            string result = string.Empty;
            bool resultValidError = false;
            string propertyName = ExpressionPropertyName.For<TViewModel>(expression);
            object propertyValue = (object)validation.ThisObject.GetType().GetProperty(propertyName).GetValue(validation.ThisObject, null);

            if (propertyValue == null)
            {
                return Result<string>.SuccessResult(result, resultValidError);
            }

            if (propertyValue.IsEmpty() == false)
            {
                double testDouble;
                if (double.TryParse(propertyValue.ToString(), out testDouble) == true)
                {
                    if (testDouble <= 0)
                    {
                        result = $"Der {description} in KW/h muß gößer 0 sein";
                        resultValidError = true;
                    }
                }
                else
                {
                    result = $"Das Feld '{description} in KW/h' darf nicht leer sein.";
                    resultValidError = true;
                }
            }
            else
            {
                result = $"Das Feld '{description} in KW/h' darf nicht leer sein.";
                resultValidError = true;
            }

            return Result<string>.SuccessResult(result, resultValidError);
        }
    }
}